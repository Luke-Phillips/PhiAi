using PhiAi.Core;
using PhiAi.MonteCarloTreeSearch;
using PhiAi.Internal.MonteCarloTreeSearch;

namespace PhiAi.Test;

public class MonteCarloTreeSearchAlgorithmTests
{
    private readonly MonteCarloTreeSearchAlgorithm<TestDomain, TestState, TestAction> _testMcts;
    private readonly MonteCarloTreeSearchAlgorithm<TicTacToeDomain, TicTacToeState, TicTacToeAction> _ticTacToeMcts_default_o;
    private readonly MonteCarloTreeSearchAlgorithm<TicTacToeDomain, TicTacToeState, TicTacToeAction> _ticTacToeMcts_default_x;
    private readonly MonteCarloTreeSearchAlgorithm<TicTacToeDomain, TicTacToeState, TicTacToeAction> _ticTacToeMcts_random_o;
    private readonly MonteCarloTreeSearchAlgorithm<TicTacToeDomain, TicTacToeState, TicTacToeAction> _ticTacToeMcts_random_x;
    
    public MonteCarloTreeSearchAlgorithmTests()
    {
        _testMcts = new MonteCarloTreeSearchAlgorithm<TestDomain, TestState, TestAction>();
        _ticTacToeMcts_default_o = new MonteCarloTreeSearchAlgorithm<TicTacToeDomain, TicTacToeState, TicTacToeAction>(RolloutPolicyType.Default);
        _ticTacToeMcts_default_x = new MonteCarloTreeSearchAlgorithm<TicTacToeDomain, TicTacToeState, TicTacToeAction>(new TicTacToeDomain("x"), RolloutPolicyType.Default);
        _ticTacToeMcts_random_o = new MonteCarloTreeSearchAlgorithm<TicTacToeDomain, TicTacToeState, TicTacToeAction>();
        _ticTacToeMcts_random_x = new MonteCarloTreeSearchAlgorithm<TicTacToeDomain, TicTacToeState, TicTacToeAction>(new TicTacToeDomain("x"));
    }

    [Fact]
    public void GetActions_TestDomain_ActionCount0()
    {
        int expectedActionCount = 0;
        int actualActionCount = _testMcts.GetActions().Count();
        Assert.Equal(expectedActionCount, actualActionCount);
    }

    [Fact]
    public void GetActions_TicTacToeDomain_ActionCount9()
    {
        int expectedActionCount = 9;
        int actualActionCount = _ticTacToeMcts_default_o.GetActions().Count();
        Assert.Equal(expectedActionCount, actualActionCount);
    }

    // SearchForIterations_<numIterations>_<domain>_<expectedAction>
    [Fact]
    public void SearchForIterations_1_TicTacToeDomain_row0col0()
    {
        string expectedPlayer = "o"; 
        int expectedRow = 0; 
        int expectedColumn = 0; 
        _ticTacToeMcts_default_o.SearchForIterations(1);
        TicTacToeAction action = _ticTacToeMcts_default_o.GetActions().ToList()[0];
        string actualPlayer = action.Player;
        int actualRow = action.Row;
        int actualColumn = action.Column;
        Assert.Equal(expectedPlayer, actualPlayer);
        Assert.Equal(expectedRow, actualRow);
        Assert.Equal(expectedColumn, actualColumn);
    }

    [Fact]
    public void SearchForIterations_9_TicTacToeDomain_row0col0()
    {
        string expectedPlayer = "o"; 
        int expectedRow = 0; 
        int expectedColumn = 0; 
        _ticTacToeMcts_default_o.SearchForIterations(9);
        TicTacToeAction action = _ticTacToeMcts_default_o.GetActions().ToList()[0];
        string actualPlayer = action.Player;
        int actualRow = action.Row;
        int actualColumn = action.Column;
        Assert.Equal(expectedPlayer, actualPlayer);
        Assert.Equal(expectedRow, actualRow);
        Assert.Equal(expectedColumn, actualColumn);

    }
    
    [Fact]
    public void SearchForIterations_10_TicTacToeDomain_row0col0()
    {
        string expectedPlayer = "o"; 
        int expectedRow = 0; 
        int expectedColumn = 0; 
        _ticTacToeMcts_default_o.SearchForIterations(10);
        TicTacToeAction action = _ticTacToeMcts_default_o.GetActions().ToList()[0];
        string actualPlayer = action.Player;
        int actualRow = action.Row;
        int actualColumn = action.Column;
        Assert.Equal(expectedPlayer, actualPlayer);
        Assert.Equal(expectedRow, actualRow);
        Assert.Equal(expectedColumn, actualColumn);
    }
    
    [Fact]
    public void TakeAction_1_GetActions_TicTacToeDomain_ActionRow0Col1()
    {
        string expectedPlayer = "x"; 
        int expectedRow = 0; 
        int expectedColumn = 1; 
        TicTacToeAction action = new TicTacToeAction("o", 0, 0);
        _ticTacToeMcts_default_o.TakeAction(action);
        var bestActions = _ticTacToeMcts_default_o.GetActions().ToList();
        TicTacToeAction bestAction = bestActions[0];
        string actualPlayer = bestAction.Player;
        int actualRow = bestAction.Row;
        int actualColumn = bestAction.Column;
        Assert.Equal(expectedPlayer, actualPlayer);
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
            _ticTacToeMcts_default_o.TakeAction(action);
        }
        var bestActions = _ticTacToeMcts_default_o.GetActions().ToList();
        TicTacToeAction bestAction = bestActions[0];
        string expectedPlayer = "o"; 
        int expectedRow = 0; 
        int expectedColumn = 2; 
        string actualPlayer = bestAction.Player;
        int actualRow = bestAction.Row;
        int actualColumn = bestAction.Column;
        Assert.Equal(expectedPlayer, actualPlayer);
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
            _ticTacToeMcts_default_x.TakeAction(action);
        }
        _ticTacToeMcts_default_x.SearchForIterations(3);
        var bestActions = _ticTacToeMcts_default_x.GetActions().ToList();
        TicTacToeAction bestAction = bestActions[0];
        string expectedPlayer = "x"; 
        int expectedRow = 2; 
        int expectedColumn = 2; 
        string actualPlayer = bestAction.Player;
        int actualRow = bestAction.Row;
        int actualColumn = bestAction.Column;
        Assert.Equal(expectedPlayer, actualPlayer);
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
    public void TicTacToeDomain_RandomO_v_RandomX_100Iterations_Draw()
    {
        double oExpectedTerminalValue = 0;
        double xExpectedTerminalValue = 0;

        while(true)
        {
            _ticTacToeMcts_random_o.SearchForIterations(5000);
            var actions = _ticTacToeMcts_random_o.GetActions();
            if (actions.Count() == 0)
            {
                break;
            }
            var action = actions.ToList()[0];
            _ticTacToeMcts_random_o.TakeAction(action);
            _ticTacToeMcts_random_x.TakeAction(action);
            
            _ticTacToeMcts_random_x.SearchForIterations(5000);
            actions = _ticTacToeMcts_random_x.GetActions();
            if (actions.Count() == 0)
            {
                break;
            }
            action = actions.ToList()[0];
            _ticTacToeMcts_random_x.TakeAction(action);
            _ticTacToeMcts_random_o.TakeAction(action);
        }
    }

    

    // try take illegal action
}