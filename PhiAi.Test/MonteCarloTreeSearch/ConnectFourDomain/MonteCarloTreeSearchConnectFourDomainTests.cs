using PhiAi.MonteCarloTreeSearch;
using PhiAi.Internal.MonteCarloTreeSearch;

namespace PhiAi.Test;

public class MonteCarloTreeSearchAlgorithmConnectFourDomainTests
{
    private readonly MonteCarloTreeSearchAlgorithm<ConnectFourDomain, ConnectFourState, ConnectFourAction> _connectFourMcts_default;
    private readonly MonteCarloTreeSearchAlgorithm<ConnectFourDomain, ConnectFourState, ConnectFourAction> _connectFourMcts_random;
    
    public MonteCarloTreeSearchAlgorithmConnectFourDomainTests()
    {
        _connectFourMcts_default = new MonteCarloTreeSearchAlgorithm<ConnectFourDomain, ConnectFourState, ConnectFourAction>(RolloutPolicyType.Default);
        _connectFourMcts_random = new MonteCarloTreeSearchAlgorithm<ConnectFourDomain, ConnectFourState, ConnectFourAction>();
    }

    // Unit Test Naming Scheme
    // <domain>_<primaryMethod>_<expectedResult>
    [Fact]
    public void ConnectFourDomain_CurrentState_EmptyGrid()
    {   
        ConnectFourState expectedState = new ConnectFourState();
        ConnectFourState actualState = _connectFourMcts_default.CurrentState;
        Assert.True(expectedState.Equals(actualState));
    }

    [Fact]
    public void ConnectFourDomain_SelfPlay_1_Iterations_ReachTerminalState ()
    {   
        while (true)
        {
            _connectFourMcts_random.SearchForIterations(1);
            var actions = _connectFourMcts_random.GetActions();
            if (actions.Count() == 0)
            {
                break;
            }
            _connectFourMcts_random.TakeAction(actions.First());
        }
        var c4d = new ConnectFourDomain();
        Assert.True(c4d.IsStateTerminal(_connectFourMcts_random.CurrentState));
    }

    [Fact]
    public void ConnectFourDomain_SelfPlay_10_Iterations_ReachTerminalState ()
    {   
        IEnumerable<ConnectFourAction> actions;
        while (true)
        {
            _connectFourMcts_random.SearchForIterations(10);
            actions = _connectFourMcts_random.GetActions();
            if (actions.Count() == 0)
            {
                break;
            }
            _connectFourMcts_random.TakeAction(actions.First());
        }
        var c4d = new ConnectFourDomain();
        Assert.True(c4d.IsStateTerminal(_connectFourMcts_random.CurrentState));
    }

    [Fact]
    public void ConnectFourDomain_SelfPlay_100_Iterations_ReachTerminalState ()
    {   
        IEnumerable<ConnectFourAction> actions;
        while (true)
        {
            _connectFourMcts_random.SearchForIterations(100);
            actions = _connectFourMcts_random.GetActions();
            if (actions.Count() == 0)
            {
                break;
            }
            _connectFourMcts_random.TakeAction(actions.First());
        }
        var c4d = new ConnectFourDomain();
        Assert.True(c4d.IsStateTerminal(_connectFourMcts_random.CurrentState));
    }

    [Fact]
    public void ConnectFourDomain_SelfPlay_1000_Iterations_ReachTerminalState ()
    {   
        IEnumerable<ConnectFourAction> actions;
        while (true)
        {
            _connectFourMcts_random.SearchForIterations(1000);   
            actions = _connectFourMcts_random.GetActions();
            if (actions.Count() == 0)
            {
                break;
            }
            _connectFourMcts_random.TakeAction(actions.First());
        }
        var c4d = new ConnectFourDomain();
        Assert.True(c4d.IsStateTerminal(_connectFourMcts_random.CurrentState));
    }

    [Fact]
    public void ConnectFourDomain_SelfPlay_10000_Iterations_ReachTerminalState ()
    {   
        IEnumerable<ConnectFourAction> actions;
        while (true)
        {
            _connectFourMcts_random.SearchForIterations(10000);
            actions = _connectFourMcts_random.GetActions();
            if (actions.Count() == 0)
            {
                break;
            }
            _connectFourMcts_random.TakeAction(actions.First());
        }
        var c4d = new ConnectFourDomain();
        Assert.True(c4d.IsStateTerminal(_connectFourMcts_random.CurrentState));
    }

    [Fact]
    public void ConnectFourDomain_SelfPlay_100000_Iterations_ReachTerminalState ()
    {   
        IEnumerable<ConnectFourAction> actions;
        while (true)
        {
            _connectFourMcts_random.SearchForIterations(100000);
            actions = _connectFourMcts_random.GetActions();
            if (actions.Count() == 0)
            {
                break;
            }
            _connectFourMcts_random.TakeAction(actions.First());
        }
        var c4d = new ConnectFourDomain();
        Assert.True(c4d.IsStateTerminal(_connectFourMcts_random.CurrentState));
    }
}