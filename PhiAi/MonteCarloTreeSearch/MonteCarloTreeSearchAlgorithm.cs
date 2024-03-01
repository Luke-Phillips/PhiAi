using PhiAi.Core;
using PhiAi.Internal.MonteCarloTreeSearch;
using PhiAi.Util;

namespace PhiAi.MonteCarloTreeSearch;

// Todo custom rollouts
public partial class MonteCarloTreeSearchAlgorithm<TDomain, TState, TAction> : IAlgorithm<TAction>
    where TDomain : IDomain<TState, TAction>, new()
    where TState : IState
    where TAction : IAction
{
    private readonly TDomain _domain;
    private readonly RolloutPolicyType _policy = RolloutPolicyType.Random;
    private readonly Random _rand;
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
    /// Scout node looks ahead by exploring, exploiting, and rolling out.
    /// </summary>
    private MonteCarloSearchTreeNode _scoutNode;


    public MonteCarloTreeSearchAlgorithm()
    {
        _domain = new TDomain();
        _rootNode = new MonteCarloSearchTreeNode(_domain.GetInitialState());
        _currentNode = _rootNode;
        _scoutNode = _currentNode;
        _policy = RolloutPolicyType.Random;
        _rand = new Random((int) DateTime.Now.Ticks);
    }

    // TODO docs may use domain if domain requires any special constructor
    public MonteCarloTreeSearchAlgorithm(TDomain domain)
    {
        _domain = domain;
        _rootNode = new MonteCarloSearchTreeNode(_domain.GetInitialState());
        _currentNode = _rootNode;
        _scoutNode = _currentNode;
        _policy = RolloutPolicyType.Random;
        _rand = new Random((int) DateTime.Now.Ticks);
    }

    public MonteCarloTreeSearchAlgorithm(RolloutPolicyType policy)
    {
        _domain = new TDomain();
        _rootNode = new MonteCarloSearchTreeNode(_domain.GetInitialState());
        _currentNode = _rootNode;
        _scoutNode = _currentNode;
        _policy = policy;
        _rand = new Random((int) DateTime.Now.Ticks);
    }

    public MonteCarloTreeSearchAlgorithm(TDomain domain, RolloutPolicyType policy)
    {
        _domain = domain;
        _rootNode = new MonteCarloSearchTreeNode(_domain.GetInitialState());
        _currentNode = _rootNode;
        _scoutNode = _currentNode;
        _policy = policy;
        _rand = new Random((int) DateTime.Now.Ticks);
    }

    public void TakeAction(TAction action)
    {
        EnsureChildrenExist();
        _currentNode = _currentNode.Children.First(node => 
        {
            if (node.EntryAction == null)
            {
                throw new Exception(); // TODO custom exceptions
            }
            return node.EntryAction.Equals(action);
        });
        Reset();
        return;
    }

    public IEnumerable<TAction> GetActions()
    {
        EnsureChildrenExist();
        return _currentNode.Children
            .OrderByDescending(node => node.GetAverageValue())
            .Select(node => 
                {
                    if (node.EntryAction == null)
                    {
                        throw new Exception(); // TODO custom exceptions
                    }
                    return node.EntryAction;
                }
            );
    }

    private void EnsureChildrenExist() // TODO rename or rethink why this needs to exist
    {
        if (_currentNode.State.IsTerminal || _domain.IsStateTerminal(_currentNode.State))
        {
            return;
        }
        if (_currentNode.Children.Count() == 0)
        {
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

    public void SearchForIterations(int numIterations)
    {
        for(int i = 0; i < numIterations; i++)
        {
            Search();
        }
    }

    public void SearchForMilliseconds(int milliseconds)
    {
        int ellapsedTime = milliseconds; // TODO
        while (ellapsedTime < milliseconds)
        {
            Search();
        }
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
            // TODO check to make sure ucb1 can't be 0 or less
            double currentBestUcb1Score = 0;
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

    private void Rollout()
    {
        TState state = _scoutNode.State;
        while (!(state.IsTerminal || _domain.IsStateTerminal(state)))
        {
            // TODO if user doesnt implement terminal check, action[] can index OOB
            var actions = _domain.GetActionsFromState(state).ToList();
            if (_policy == RolloutPolicyType.Random)
            {
                state = _domain.GetStateFromStateAndAction(state, actions[_rand.Next(actions.Count)]);
            }
            else if (_policy == RolloutPolicyType.Default)
            {
                state = _domain.GetStateFromStateAndAction(state, actions[0]);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        _rolloutValue = _domain.GetTerminalStateValue(state);
    }

    private void Backpropagate()
    {
        while (_scoutNode.Parent != null) // redundant (only root should have null parent) to get rid of null warning whilst maintaining readability
        {
            _scoutNode.Visits += 1;
            _scoutNode.Value += _rolloutValue;
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
            throw new NotImplementedException(); // TODO
        }
        // TODO unhard code constant C?
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

        public double GetAverageValue()
        {
            if (Visits < 1)
            {
                return 0;
            }
            return Value / Visits;
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
    }
}