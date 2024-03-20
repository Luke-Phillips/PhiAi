using PhiAi.MonteCarloTreeSearch;
using PhiAi.Internal.MonteCarloTreeSearch;

namespace PhiAi.Test;

public class MonteCarloTreeSearchAlgorithmTicTacToeDomainTests
{
    private readonly MonteCarloTreeSearchAlgorithm<TicTacToeDomain, TicTacToeState, TicTacToeAction> _ticTacToeMcts_default;
    private readonly MonteCarloTreeSearchAlgorithm<TicTacToeDomain, TicTacToeState, TicTacToeAction> _ticTacToeMcts_random;
    
    public MonteCarloTreeSearchAlgorithmTicTacToeDomainTests()
    {
        _ticTacToeMcts_default = new MonteCarloTreeSearchAlgorithm<TicTacToeDomain, TicTacToeState, TicTacToeAction>(RolloutPolicyType.Default);
        _ticTacToeMcts_random = new MonteCarloTreeSearchAlgorithm<TicTacToeDomain, TicTacToeState, TicTacToeAction>();
    }

    // Unit Test Naming Scheme
    // <domain>_<primaryMethod>_<expectedResult>
    [Fact]
    public void GetActions_TicTacToeDomain_ActionCount9()
    {
        int expectedActionCount = 9;
        int actualActionCount = _ticTacToeMcts_default.GetActions().Count();
        Assert.Equal(expectedActionCount, actualActionCount);
    }

    // SearchForIterations_<numIterations>_<domain>_<expectedAction>
    [Fact]
    public void SearchForIterations_1_TicTacToeDomain_row0col0()
    {
        string expectedAgent = "x"; 
        int expectedRow = 0; 
        int expectedColumn = 0; 
        _ticTacToeMcts_default.SearchForIterations(1);
        TicTacToeAction action = _ticTacToeMcts_default.GetActions().ToList()[0];
        string actualAgent = action.Agent;
        int actualRow = action.Row;
        int actualColumn = action.Column;
        Assert.Equal(expectedAgent, actualAgent);
        Assert.Equal(expectedRow, actualRow);
        Assert.Equal(expectedColumn, actualColumn);
    }

    [Fact]
    public void SearchForIterations_9_TicTacToeDomain_row0col0()
    {
        string expectedAgent = "o"; 
        int expectedRow = 0; 
        int expectedColumn = 0; 
        _ticTacToeMcts_default.SearchForIterations(9);
        TicTacToeAction action = _ticTacToeMcts_default.GetActions().ToList()[0];
        string actualAgent = action.Agent;
        int actualRow = action.Row;
        int actualColumn = action.Column;
        Assert.Equal(expectedAgent, actualAgent);
        Assert.Equal(expectedRow, actualRow);
        Assert.Equal(expectedColumn, actualColumn);
    }
    
    [Fact]
    public void SearchForIterations_10_TicTacToeDomain_row0col0()
    {
        string expectedAgent = "o"; 
        int expectedRow = 0; 
        int expectedColumn = 0; 
        _ticTacToeMcts_default.SearchForIterations(10);
        TicTacToeAction action = _ticTacToeMcts_default.GetActions().ToList()[0];
        string actualAgent = action.Agent;
        int actualRow = action.Row;
        int actualColumn = action.Column;
        Assert.Equal(expectedAgent, actualAgent);
        Assert.Equal(expectedRow, actualRow);
        Assert.Equal(expectedColumn, actualColumn);
    }
    
    [Fact]
    public void TakeAction_1_GetActions_TicTacToeDomain_ActionRow0Col1()
    {
        string expectedAgent = "x"; 
        int expectedRow = 0; 
        int expectedColumn = 1; 
        TicTacToeAction action = new TicTacToeAction("o", 0, 0);
        _ticTacToeMcts_default.TakeAction(action);
        var bestActions = _ticTacToeMcts_default.GetActions().ToList();
        TicTacToeAction bestAction = bestActions[0];
        string actualAgent = bestAction.Agent;
        int actualRow = bestAction.Row;
        int actualColumn = bestAction.Column;
        Assert.Equal(expectedAgent, actualAgent);
        Assert.Equal(expectedRow, actualRow);
        Assert.Equal(expectedColumn, actualColumn);
    }
    
    //  o | o | ?
    // ---|---|---
    //    | x |   
    // ---|---|---
    //  x | x | o
    [Fact]
    public void TakeAction_6_GetActions_TicTacToeDomain_ActionRow0Col2()
    {
        var actions = new List<TicTacToeAction> {
            new TicTacToeAction("o", 0, 0),
            new TicTacToeAction("x", 1, 1),
            new TicTacToeAction("o", 2, 2),
            new TicTacToeAction("x", 2, 1),
            new TicTacToeAction("o", 0, 1),
            new TicTacToeAction("x", 2, 0),
        };
        foreach (var action in actions)
        {
            _ticTacToeMcts_default.TakeAction(action);
        }
        var bestActions = _ticTacToeMcts_default.GetActions().ToList();
        TicTacToeAction bestAction = bestActions[0];
        string expectedAgent = "o"; 
        int expectedRow = 0; 
        int expectedColumn = 2; 
        string actualAgent = bestAction.Agent;
        int actualRow = bestAction.Row;
        int actualColumn = bestAction.Column;
        Assert.Equal(expectedAgent, actualAgent);
        Assert.Equal(expectedRow, actualRow);
        Assert.Equal(expectedColumn, actualColumn);
    }

    // AI is X
    // Expected next move is "?" row 2 column 2 (bottom right)
    //  o | o | 
    // ---|---|---
    //  o | o | x 
    // ---|---|---
    //  x | x | ?
    [Fact]
    public void TicTacToeXDomain_TakeAction_7_SearchForIterations_3_GetActions_ActionRow2Col2()
    {
        var actions = new List<TicTacToeAction> {
            new TicTacToeAction("o", 0, 0),
            new TicTacToeAction("x", 2, 0),
            new TicTacToeAction("o", 0, 1),
            new TicTacToeAction("x", 2, 1),
            new TicTacToeAction("o", 1, 0),
            new TicTacToeAction("x", 1, 2),
            new TicTacToeAction("o", 1, 1),
        };
        foreach (var action in actions)
        {
            _ticTacToeMcts_default.TakeAction(action);
        }
        _ticTacToeMcts_default.SearchForIterations(3);
        var bestActions = _ticTacToeMcts_default.GetActions().ToList();
        TicTacToeAction bestAction = bestActions[0];
        string expectedAgent = "x"; 
        int expectedRow = 2; 
        int expectedColumn = 2; 
        string actualAgent = bestAction.Agent;
        int actualRow = bestAction.Row;
        int actualColumn = bestAction.Column;
        Assert.Equal(expectedAgent, actualAgent);
        Assert.Equal(expectedRow, actualRow);
        Assert.Equal(expectedColumn, actualColumn);
    }

    // Expected next move is "?" row 2 column 2 (bottom right)
    //    |   | 
    // ---|---|---
    //    |   |   
    // ---|---|---
    //    |   | 
    [Fact]
    public void TicTacToeDomain_SelfPlay_100Iterations_Draw()
    {
        while(true)
        {
            _ticTacToeMcts_random.SearchForIterations(100);
            var actions = _ticTacToeMcts_random.GetActions();
            if (actions.Count() == 0)
            {
                break;
            }
            var action = actions.ToList()[0];
            _ticTacToeMcts_random.TakeAction(action);
        }
    }
}