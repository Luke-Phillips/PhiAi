using PhiAi.Core;
using PhiAi.Internal.MonteCarloTreeSearch;
using PhiAi.Util;
using System.Timers;

namespace PhiAi.MonteCarloTreeSearch;

/// <summary>
/// Algorithm best suited for finite, two-person, zero-sum, perfect-information, sequential games 
/// </summary>
public class MonteCarloTreeSearchAlgorithm<TDomain, TState, TAction> : IAlgorithm<TAction>
    where TDomain : IDomain<TState, TAction>, new()
    where TState : IState
    where TAction : IAction
{
    private readonly TDomain _domain;
    private readonly RolloutPolicyType _policy = RolloutPolicyType.Random;
    private readonly Random _rand;
    private readonly string _startingAgent;
    private readonly System.Timers.Timer _timer;
    private bool _isTimerGoing = false;
    private double _rolloutValue;

    /// <summary>
    /// Root node should always be the initial state
    /// </summary>
    private MonteCarloSearchTreeNode _rootNode;
    /// <summary>
    /// Current node points to the current state
    /// </summary>
    private MonteCarloSearchTreeNode _currentNode;
    /// <summary>
    /// Scout node looks ahead by exploring, exploiting, and performing rollouts (simulations).
    /// </summary>
    private MonteCarloSearchTreeNode _scoutNode;

    /// <summary>
    /// Access to the current state. DO NOT mutate data.
    /// </summary>
    public TState CurrentState
    {
        get => _currentNode.State;
    }

    public MonteCarloTreeSearchAlgorithm()
    {
        _domain = new TDomain();
        _rootNode = new MonteCarloSearchTreeNode(_domain.GetInitialState());
        _currentNode = _rootNode;
        _scoutNode = _currentNode;
        _startingAgent = _rootNode.State.Agent;
        _policy = RolloutPolicyType.Random;
        _rand = new Random((int) DateTime.Now.Ticks);
        _timer = new System.Timers.Timer();
        _timer.Elapsed += OnElapsed;
    }

    /// <summary>
    /// Constructor if <c>TDomain</c> must be instantiated by user
    /// </summary>
    /// <param name="domain"></param>
    public MonteCarloTreeSearchAlgorithm(TDomain domain)
    {
        _domain = domain;
        _rootNode = new MonteCarloSearchTreeNode(_domain.GetInitialState());
        _currentNode = _rootNode;
        _scoutNode = _currentNode;
        _startingAgent = _rootNode.State.Agent;
        _policy = RolloutPolicyType.Random;
        _rand = new Random((int) DateTime.Now.Ticks);
        _timer = new System.Timers.Timer();
        _timer.Elapsed += OnElapsed;
    }

    public MonteCarloTreeSearchAlgorithm(RolloutPolicyType policy)
    {
        _domain = new TDomain();
        _rootNode = new MonteCarloSearchTreeNode(_domain.GetInitialState());
        _currentNode = _rootNode;
        _scoutNode = _currentNode;
        _startingAgent = _rootNode.State.Agent;
        _policy = policy;
        _rand = new Random((int) DateTime.Now.Ticks);
        _timer = new System.Timers.Timer();
        _timer.Elapsed += OnElapsed;
    }

    public MonteCarloTreeSearchAlgorithm(TDomain domain, RolloutPolicyType policy)
    {
        _domain = domain;
        _rootNode = new MonteCarloSearchTreeNode(_domain.GetInitialState());
        _currentNode = _rootNode;
        _scoutNode = _currentNode;
        _startingAgent = _rootNode.State.Agent;
        _policy = policy;
        _rand = new Random((int) DateTime.Now.Ticks);
        _timer = new System.Timers.Timer();
        _timer.Elapsed += OnElapsed;
    }

    /// <summary>
    /// Will advance current state to some next state determined by the action taken
    /// </summary>
    /// <param name="action">The action to take from the current state</param>
    /// <exception cref="NoEntryActionException"></exception>
    public void TakeAction(TAction action)
    {
        EnsureChildrenExist();
        _currentNode = _currentNode.Children.First(node => 
        {
            if (node.EntryAction == null)
            {
                throw new NoEntryActionException("A child state of the current state is missing a reference to its entry action which is necessary to match against supplied action. Report as bug.");
            }
            return node.EntryAction.Equals(action);
        });
        Reset();
        return;
    }

    /// <summary>
    /// Gets all possible actions that can be taken from the current state
    /// </summary>
    /// <returns>IEnumerable of actions</returns>
    /// <exception cref="NoEntryActionException"></exception>
    public IEnumerable<TAction> GetActions()
    {
        EnsureChildrenExist();
        return _currentNode.Children
            .OrderByDescending(node => node.GetAverageValue())
            .Select(node => 
                {
                    if (node.EntryAction == null)
                    {
                        throw new NoEntryActionException("A child state of the current state is missing a reference to its entry action which is necessary to get all possible actions. Report as bug.");
                    }
                    return node.EntryAction;
                }
            );
    }

    private void EnsureChildrenExist()
    {
        if (_currentNode.State.IsTerminal || _domain.IsStateTerminal(_currentNode.State))
        {
            return;
        }
        if (_currentNode.Children.Count() == 0)
        {
            // rollout  
            if (_currentNode.Visits == 0)
            {
                Rollout();
                Backpropagate();
                Reset();
            }
            Expand();
            Reset();
        }
    }

    /// <summary>
    /// Searches the tree for the given amount of iterations. 
    /// </summary>
    /// <param name="numIterations">Number of searches to do</param>
    public void SearchForIterations(int numIterations)
    {
        for(int i = 0; i < numIterations; i++)
        {
            Search();
        }
    }

    /// <summary>
    /// Searches the tree until the number of milliseconds has elapsed. The current iteration will complete before returning. 
    /// </summary>
    /// <param name="milliseconds">Number of milliseconds to search for</param>
    public void SearchForMilliseconds(int milliseconds)
    {
        _isTimerGoing = true;
        _timer.Interval = milliseconds;
        _timer.Start();
        while (_isTimerGoing)
        {
            Search();
        }
        _timer.Stop();
    }

    private void OnElapsed(object? source, ElapsedEventArgs e)
    {
        _isTimerGoing = false;
    }
    
    private void Search()
    {
        Select();
        Expand();
        Rollout();
        Backpropagate();
        Reset();   
    }

    // Explore and Exploit
    // get to a leaf node
    private void Select()
    {
        while(_scoutNode.Children.Count() > 0)
        {
            double currentBestUcb1Score = -1;
            foreach (MonteCarloSearchTreeNode child in _scoutNode.Children)
            {
                if (child.Visits == 0)
                {
                    _scoutNode = child;
                    return;
                }

                double ucb1Score = CalculateUcb1(child);
                
                if (ucb1Score > currentBestUcb1Score)
                {
                    currentBestUcb1Score = ucb1Score;
                    _scoutNode = child;
                }
            }
        }
    }

    // add children to selected leaf node if leaf node has been rolled out at least once
    private void Expand()
    {
        if (_scoutNode.Visits > 0 && !(_scoutNode.State.IsTerminal || _domain.IsStateTerminal(_scoutNode.State)))
        {
            IEnumerable<TAction> actions = _domain.GetActionsFromState(_scoutNode.State);
            foreach (TAction action in actions)
            {
                _scoutNode.Children.Add(new MonteCarloSearchTreeNode(_domain.GetStateFromStateAndAction(_scoutNode.State, action), _scoutNode, action));
            }
            _scoutNode = _scoutNode.Children.First();
        }
    }

    // Rollout (simulate) state and actions via a policy until a terminal state is reached and value is recorded
    private void Rollout()
    {
        TState state = _scoutNode.State;
        while (!(state.IsTerminal || _domain.IsStateTerminal(state)))
        {
            var actions = _domain.GetActionsFromState(state).ToList();
            if (_policy == RolloutPolicyType.Random)
            {
                try
                {
                    state = _domain.GetStateFromStateAndAction(state, actions[_rand.Next(actions.Count)]);
                }
                catch (IndexOutOfRangeException)
                {
                    // refactoring the way terminal checks work may eliminate the possibility of IOOR exceptions that occur because of users improperly implementing terminal checks
                    break;
                }
            }
            else if (_policy == RolloutPolicyType.Default)
            {
                try
                {
                    state = _domain.GetStateFromStateAndAction(state, actions[0]);
                }
                catch
                {
                    // refactoring the way terminal checks work may eliminate the possibility of IOOR exceptions that occur because of users improperly implementing terminal checks
                    break;
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        _rolloutValue = _domain.GetTerminalStateValue(state);
    }

    // Backpropogate terminal state value up the tree until current state is reached
    private void Backpropagate()
    {
        while (_scoutNode.Parent != null) // redundant (only root should have null parent) to get rid of null warning whilst maintaining readability
        {
            _scoutNode.Visits += 1;
            _scoutNode.Value += _scoutNode.Parent.State.Agent == _startingAgent ? _rolloutValue : -_rolloutValue;
            if (_scoutNode == _currentNode) break;
            _scoutNode = _scoutNode.Parent;
        }
        return;
    }

    private void Reset()
    {
        _scoutNode = _currentNode;
    }

    // UCB1(Si) = Vi + C sqrt(lnN/ni) where C = 2 
    // This method assumes proper use (node has parents and has at least one visit)
    private double CalculateUcb1(MonteCarloSearchTreeNode node)
    {
        if (node.Parent == null)
        {
            throw new NotImplementedException(); 
        }
        return MathUtility.CalculateUpperConfidenceBound1(
            node.GetAverageValue(),
            2,
            node.Parent.Visits,
            node.Visits
        );
    }

    private class MonteCarloSearchTreeNode
    {
        private readonly TState _state;

        // constructor for initial state only
        public MonteCarloSearchTreeNode(TState state)
        {
            _state = state;
            Value = 0;
            Visits = 1;

            Parent = null;
            Children = new List<MonteCarloSearchTreeNode> {};
            EntryAction = default;
        }
        public MonteCarloSearchTreeNode(TState state, MonteCarloSearchTreeNode parent, TAction entryAction)
        {
            _state = state;
            Value = 0;
            Visits = 0;

            Parent = parent;
            Children = new List<MonteCarloSearchTreeNode> {};
            EntryAction = entryAction;
        }

        public TState State
        {
            get => _state;
        }

        public double Value { get; set; }
        public int Visits { get; set; }
        public MonteCarloSearchTreeNode? Parent { get; set; }
        public ICollection<MonteCarloSearchTreeNode> Children { get; set; }
        public TAction? EntryAction { get; set; }
        
        public double GetAverageValue()
        {
            if (Visits < 1)
            {
                return 0;
            }
            return Value / Visits;
        }
    }
}