namespace MonteCarloTreeSearch
{
    public abstract class MctsDomain<TState, TAction>
    {
        private RolloutPolicy _rolloutPolicy = RolloutPolicy.Random;
        public abstract IEnumerable<TAction> GetActionsFromState(TState state);
        public abstract TState GetStateFromStateAndAction(TState previousState, TAction action);
        public abstract bool IsStateTerminal(TState state);

        public abstract TAction GetRolloutPolicy(TState state);
        public abstract double GetTerminalStateValue(TState state);
        // TODO add IsLegalAction of some sort?
        
        public SetRolloutPolicy(RolloutPolicy rolloutPolicy)
        {
            _rolloutPolicy = rolloutPolicy;
        }


    }
}
