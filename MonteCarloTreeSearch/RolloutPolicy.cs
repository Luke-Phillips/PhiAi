namespace MonteCarloTreeSearch
{
    public enum RolloutPolicy
    {
        // User-defined policy
        Custom,

        // First (or index 0) action policy (intended use is testing)
        // IMctsDomain.GetActionsFromState should be implemented to be referentially transparent
        Default,

        // Random action policy
        Random
    }
}