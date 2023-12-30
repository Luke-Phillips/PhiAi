namespace MonteCarloTreeSearch
{
    public interface IMctsState<TState>
    {
        bool Equals(TState state);
    }
}