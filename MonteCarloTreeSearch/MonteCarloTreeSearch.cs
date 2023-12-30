namespace MonteCarloTreeSearch
{
    public class MonteCarloTreeSearch<TDomain, TState, TAction>
        where TDomain : IMctsDomain<TState, TAction>
        where TState : IMctsState<TState>
        where TAction : class
    {
        private TDomain _domain { get; set; }

        private MonteCarloSearchTreeNode _root { get; set; }

        private MonteCarloSearchTreeNode _current { get; set; }

        private double _rolloutValue { get; set; }
        
        private TAction? _bestAction { get; set; }

        public MonteCarloTreeSearch(TDomain domain, TState initialState)
        {
            _domain = domain;
            _root = new MonteCarloSearchTreeNode(initialState);
            _current = _root;
            _bestAction = null;
            Expand();
        }

        public void TakeAction(TAction action)
        {
            TState newState = _domain.GetStateFromAction(action);
            MonteCarloSearchTreeNode newRoot = _root.Children.First(node => node.State.Equals(newState));
            _root = newRoot;
        }

        public TAction? GetBestAction()
        {
            return  _root.Children.Aggregate((best, current) => current.Value > best.Value ? current : best).CausitiveAction;
        }

        public void Search(int numIterations)
        {
            for(int i = 0; i < numIterations; i++)
            {
                Select();
                Expand();
                Rollout();
                Backpropagate(); 
            }
        }

        // Explore and Exploit
        private void Select()
        {
            _current = _root;
            while(_current.Children.Count() > 0)
            {
                // TODO check to make sure ucb1 can't be 0 or less
                double currentBestUcb1Score = 0;
                foreach (MonteCarloSearchTreeNode child in _current.Children)
                {
                    if (child.Visits == 0)
                    {
                        _current = child;
                        return;
                    }

                    double ucb1Score = Ucb1(child);
                    if (ucb1Score > currentBestUcb1Score)
                    {
                        currentBestUcb1Score = ucb1Score;
                        _current = child;
                    }
                }
            }
        }

        // TODO cant expand from terminal node? (not leaf node, terminal node)
        private void Expand()
        {
            if (_current.Visits > 0 && !_domain.IsStateTerminal(_current.State))
            {
                IEnumerable<TAction> actions = _domain.GetActionsFromState(_current.State);
                foreach (TAction action in actions)
                {
                    _current.Children.Add(new MonteCarloSearchTreeNode(_domain.GetStateFromAction(action), _current, action));
                }
                _current = _current.Children.First();
            }
        }

        private void Rollout()
        {
            TState state = _current.State;
            while (!_domain.IsStateTerminal(state))
            {
                state = _domain.GetStateFromAction(_domain.RolloutPolicy(state));
            }
            _rolloutValue = _domain.GetTerminalStateValue(state);
        }

        public void Backpropagate()
        {
            _current.Visits += 1;
            _current.Value += _rolloutValue;
            if (_current != _root && _current.Parent != null) // redundant (only root should have null parent) to get rid of null warning whilst maintaining readability
            {
                _current = _current.Parent;
                Backpropagate();
            }
            return;
        }

        // UCB1(Si) = Vi + 2 sqrt(lnN/ni) 
        // This method assumes proper use (node has parents and has at least one visit)
        private double Ucb1(MonteCarloSearchTreeNode node)
        {
            return (node.Value / node.Visits) + 2 * Math.Sqrt(Math.Log(node.Parent.Visits) / node.Visits);
        }
        
        public class MonteCarloSearchTreeNode
        {
            public MonteCarloSearchTreeNode(TState state)
            {
                State = state;
                Value = 0;
                Visits = 0;

                Parent = null;
                Children = new List<MonteCarloSearchTreeNode> {};
                CausitiveAction = null;
            }

            public MonteCarloSearchTreeNode(TState state, MonteCarloSearchTreeNode parent, TAction causitiveAction)
            {
                State = state;
                Value = 0;
                Visits = 0;

                Parent = parent;
                Children = new List<MonteCarloSearchTreeNode> {};
                CausitiveAction = causitiveAction;
            }

            public TState State { get; set; }

            public double Value { get; set; }
            
            public int Visits { get; set; }

            public MonteCarloSearchTreeNode? Parent { get; set; }

            public List<MonteCarloSearchTreeNode> Children { get; set; }

            public TAction? CausitiveAction { get; set; }
        }
    }
}