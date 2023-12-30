namespace MonteCarloTreeSearch
{
    public interface IMctsDomain<TState, TAction>
        where TState : IMctsState<TState>
    {
        IEnumerable<TAction> GetActionsFromState(TState state);
        TState GetStateFromAction(TAction action);
        bool IsStateTerminal(TState state);
        TAction RolloutPolicy(TState state);
        

        double GetTerminalStateValue(TState state);
        // TODO add IsLegalAction of some sort?
    }
}