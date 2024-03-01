namespace PhiAi.Internal.MonteCarloTreeSearch;

public enum RolloutPolicyType
{
    /// <summary>
    /// First (or index 0) action policy (intended use is testing)
    /// </summary>
    Default,

    /// <summary>
    /// Random action policy
    /// </summary>
    Random,

    /// <summary>
    /// User-defined policy
    /// </summary>
    Custom,
}

// public class RandomPolicy<TDomain, TState, TAction> : IPolicy<TState, TAction>
// where TDomain : IDomain<TState, TAction>
// {
//     private readonly TDomain _domain;
//     private readonly Random _rand;
//     public RandomPolicy(TDomain domain)
//     {
//         _domain = domain;
//         _rand = new Random();
//     }

//     public RandomPolicy(TDomain domain, int seed)
//     {
//         _domain = domain;
//         _rand = new Random(seed);
//     }

//     public TAction Execute(TState state)
//     {
//         IEnumerable<TAction> actions = _domain.GetActionsFromState(state);
//         int i = _rand.Next(actions.Count());
//         return actions.ToList()[i];
//     }
// }