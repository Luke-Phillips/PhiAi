using PhiAi.MonteCarloTreeSearch;
using PhiAi.Internal.MonteCarloTreeSearch;

namespace PhiAi.Test;

public class ConnectFourDomainTests
{
    private readonly ConnectFourDomain _connectFourDomain;
    
    public ConnectFourDomainTests()
    {
        _connectFourDomain = new ConnectFourDomain();
    }

    [Fact]
    public void ConnectFourState_Equals_NewConnectFourStates_True()
    {  
        var state1 = new ConnectFourState();
        var state2 = new ConnectFourState();
        Assert.True(state1.Equals(state2));
    }

    [Fact]
    public void ConnectFourState_Equals_EquavilentConnectFourStates_True()
    {  
        var state1 = new ConnectFourState();
        var state2 = new ConnectFourState();
        state1.Grid[0,0] = ConnectFourPlayer.Red;
        state2.Grid[0,0] = ConnectFourPlayer.Red;
        Assert.True(state1.Equals(state2));
    }

    [Fact]
    public void ConnectFourState_Equals_CopiedConnectFourStates_True()
    {  
        var state1 = new ConnectFourState();
        var state2 = ConnectFourState.Copy(state1);
        Assert.True(state1.Equals(state2));
    }

    [Fact]
    public void ConnectFourState_Equals_DifferentConnectFourStates1_False()
    {  
        var state1 = new ConnectFourState();
        var state2 = new ConnectFourState();
        state1.Grid[0,0] = ConnectFourPlayer.Red;
        state2.Grid[0,0] = ConnectFourPlayer.Yellow;
        Assert.False(state1.Equals(state2));
    }

    [Fact]
    public void ConnectFourState_Equals_DifferentConnectFourStates2_False()
    {  
        var state1 = new ConnectFourState();
        var state2 = new ConnectFourState();
        state1.Grid[0,0] = ConnectFourPlayer.Red;
        state2.Grid[0,1] = ConnectFourPlayer.Red;
        Assert.False(state1.Equals(state2));
    }
    
    [Fact]
    public void ConnectFourDomain_GetInitialState_EqualsNewConnectFourState()
    {  
        var state1 = new ConnectFourState();
        var state2 = _connectFourDomain.GetInitialState();
        Assert.True(state1.Equals(state2));
        Assert.True(state2.Equals(state1));
    }

    [Fact]
    public void ConnectFourAction_Equals_EquivalentActions1_True()
    {  
        var action1 = new ConnectFourAction(0);
        var action2 = new ConnectFourAction(0);
        Assert.True(action1.Equals(action2));
        Assert.True(action2.Equals(action1));
    }

    [Fact]
    public void ConnectFourAction_Equals_EquivalentActions2_True()
    {  
        var action1 = new ConnectFourAction(6);
        var action2 = new ConnectFourAction(6);
        Assert.True(action1.Equals(action2));
        Assert.True(action2.Equals(action1));
    }

    [Fact]
    public void ConnectFourAction_Equals_DifferentActions_False()
    {  
        var action1 = new ConnectFourAction(1);
        var action2 = new ConnectFourAction(2);
        Assert.False(action1.Equals(action2));
        Assert.False(action2.Equals(action1));
    }

    [Fact]
    public void ConnectFourDomain_GetActionsFromInitialState_ListOf7Actions()
    {  
        var expectedActions = new List<ConnectFourAction>
        {
            new ConnectFourAction(0),
            new ConnectFourAction(1),
            new ConnectFourAction(2),
            new ConnectFourAction(3),
            new ConnectFourAction(4),
            new ConnectFourAction(5),
            new ConnectFourAction(6)
        };
        var actualActions = _connectFourDomain.GetActionsFromState(_connectFourDomain.GetInitialState()).ToList();
        Assert.Equal(expectedActions.Count, actualActions.Count);
        for (int i = 0; i < expectedActions.Count; i++)
        {
            Assert.True(expectedActions[i].Equals(actualActions[i]));
        };
    }
    
    [Fact]
    public void ConnectFourDomain_GetActionsFromStateWithFullColumn_ListOf6Actions()
    {  
        var state = _connectFourDomain.GetInitialState();
        state.Grid[5,0] = ConnectFourPlayer.Red;
        state.Grid[4,0] = ConnectFourPlayer.Yellow;
        state.Grid[3,0] = ConnectFourPlayer.Red;
        state.Grid[2,0] = ConnectFourPlayer.Yellow;
        state.Grid[1,0] = ConnectFourPlayer.Red;
        state.Grid[0,0] = ConnectFourPlayer.Yellow;

        var expectedActions = new List<ConnectFourAction>
        {
            new ConnectFourAction(1),
            new ConnectFourAction(2),
            new ConnectFourAction(3),
            new ConnectFourAction(4),
            new ConnectFourAction(5),
            new ConnectFourAction(6)
        };
        
        var actualActions = _connectFourDomain.GetActionsFromState(state).ToList();
        Assert.Equal(expectedActions.Count, actualActions.Count);
        for (int i = 0; i < expectedActions.Count; i++)
        {
            Assert.True(expectedActions[i].Equals(actualActions[i]));
        };
    }

    [Fact]
    public void ConnectFourDomain_GetActionsFromStateWithTriangularlyFilled_ListOf6AccurateActions()
    {  
        var state = _connectFourDomain.GetInitialState();
        state.Grid[5,0] = ConnectFourPlayer.Red;
        state.Grid[4,0] = ConnectFourPlayer.Yellow;
        state.Grid[3,0] = ConnectFourPlayer.Red;
        state.Grid[2,0] = ConnectFourPlayer.Yellow;
        state.Grid[1,0] = ConnectFourPlayer.Red;
        state.Grid[0,0] = ConnectFourPlayer.Yellow; 

        state.Grid[5,1] = ConnectFourPlayer.Red;
        state.Grid[4,1] = ConnectFourPlayer.Yellow;
        state.Grid[3,1] = ConnectFourPlayer.Red;
        state.Grid[2,1] = ConnectFourPlayer.Yellow;
        state.Grid[1,1] = ConnectFourPlayer.Red;

        state.Grid[5,2] = ConnectFourPlayer.Yellow;
        state.Grid[4,2] = ConnectFourPlayer.Red;
        state.Grid[3,2] = ConnectFourPlayer.Yellow;
        state.Grid[2,2] = ConnectFourPlayer.Red;

        state.Grid[5,3] = ConnectFourPlayer.Yellow;
        state.Grid[4,3] = ConnectFourPlayer.Red;
        state.Grid[3,3] = ConnectFourPlayer.Yellow;
        
        state.Grid[5,2] = ConnectFourPlayer.Red;
        state.Grid[4,2] = ConnectFourPlayer.Yellow;

        state.Grid[5,1] = ConnectFourPlayer.Red;

        var expectedActions = new List<ConnectFourAction>
        {
            new ConnectFourAction(1),
            new ConnectFourAction(2),
            new ConnectFourAction(3),
            new ConnectFourAction(4),
            new ConnectFourAction(5),
            new ConnectFourAction(6)
        };

        var actualActions = _connectFourDomain.GetActionsFromState(state).ToList();
        Assert.Equal(expectedActions.Count, actualActions.Count);
        for (int i = 0; i < expectedActions.Count; i++)
        {
            Assert.True(expectedActions[i].Equals(actualActions[i]));
        };
    }
    
    [Fact]
    public void ConnectFourDomain_GetStateFromStateAndAction_InitialState_Column0_AccurateState()
    {  
        var state = _connectFourDomain.GetInitialState();
        var action = new ConnectFourAction(0);
        var expectedState = new ConnectFourState()
        {
            Agent = "yellow"
        };
        expectedState.Grid[5,0] = ConnectFourPlayer.Red;
        ConnectFourState actualState = _connectFourDomain.GetStateFromStateAndAction(state, action);
        Assert.True(expectedState.Equals(actualState));
    }

    [Fact]
    public void ConnectFourDomain_GetStateFromStateAndAction_OnePieceInColumn3_Column3_AccurateState()
    {  
        var state = _connectFourDomain.GetInitialState();
        state.Grid[5,3] = ConnectFourPlayer.Red;
        state.Agent = "yellow";
        var action = new ConnectFourAction(3);

        var expectedState = ConnectFourState.Copy(state);
        expectedState.Agent = "red";
        expectedState.Grid[4,3] = ConnectFourPlayer.Yellow;
        
        ConnectFourState actualState = _connectFourDomain.GetStateFromStateAndAction(state, action);
        
        Assert.True(expectedState.Equals(actualState));
    }

    [Fact]
    public void ConnectFourDomain_GetStateFromStateAndAction_OnePieceInColumn3_Column2_AccurateState()
    {  
        var state = _connectFourDomain.GetInitialState();
        state.Grid[5,3] = ConnectFourPlayer.Red;
        state.Agent = "yellow";
        var action = new ConnectFourAction(2);

        var expectedState = ConnectFourState.Copy(state);
        expectedState.Agent = "red";
        expectedState.Grid[5,2] = ConnectFourPlayer.Yellow;
        
        ConnectFourState actualState = _connectFourDomain.GetStateFromStateAndAction(state, action);

        Assert.True(expectedState.Equals(actualState));
    }

    [Fact]
    public void ConnectFourDomain_IsTerminalState_NonTerminalState_False()
    {  
        var state = _connectFourDomain.GetInitialState();
        Assert.False(_connectFourDomain.IsStateTerminal(state));
    }

    [Fact]
    public void ConnectFourDomain_IsTerminalState_Draw_True()
    {  
        var state = _connectFourDomain.GetInitialState();
        // column 0
        state.Grid[5,0] = ConnectFourPlayer.Red;
        state.Grid[4,0] = ConnectFourPlayer.Yellow;
        state.Grid[3,0] = ConnectFourPlayer.Red;
        state.Grid[2,0] = ConnectFourPlayer.Yellow;
        state.Grid[1,0] = ConnectFourPlayer.Red;
        state.Grid[0,0] = ConnectFourPlayer.Yellow;
        // column 1
        state.Grid[5,1] = ConnectFourPlayer.Red;
        state.Grid[4,1] = ConnectFourPlayer.Yellow;
        state.Grid[3,1] = ConnectFourPlayer.Red;
        state.Grid[2,1] = ConnectFourPlayer.Yellow;
        state.Grid[1,1] = ConnectFourPlayer.Red;
        state.Grid[0,1] = ConnectFourPlayer.Yellow; 
        // column 2
        state.Grid[5,2] = ConnectFourPlayer.Yellow;
        state.Grid[4,2] = ConnectFourPlayer.Red;
        state.Grid[3,2] = ConnectFourPlayer.Yellow;
        state.Grid[2,2] = ConnectFourPlayer.Red;
        state.Grid[1,2] = ConnectFourPlayer.Yellow;
        state.Grid[0,2] = ConnectFourPlayer.Red; 
        // column 3
        state.Grid[5,3] = ConnectFourPlayer.Yellow;
        state.Grid[4,3] = ConnectFourPlayer.Red;
        state.Grid[3,3] = ConnectFourPlayer.Yellow;
        state.Grid[2,3] = ConnectFourPlayer.Red;
        state.Grid[1,3] = ConnectFourPlayer.Yellow;
        state.Grid[0,3] = ConnectFourPlayer.Red; 
        // column 4
        state.Grid[5,4] = ConnectFourPlayer.Red;
        state.Grid[4,4] = ConnectFourPlayer.Yellow;
        state.Grid[3,4] = ConnectFourPlayer.Red;
        state.Grid[2,4] = ConnectFourPlayer.Yellow;
        state.Grid[1,4] = ConnectFourPlayer.Red;
        state.Grid[0,4] = ConnectFourPlayer.Yellow; 
        // column 5
        state.Grid[5,5] = ConnectFourPlayer.Red;
        state.Grid[4,5] = ConnectFourPlayer.Yellow;
        state.Grid[3,5] = ConnectFourPlayer.Red;
        state.Grid[2,5] = ConnectFourPlayer.Yellow;
        state.Grid[1,5] = ConnectFourPlayer.Red;
        state.Grid[0,5] = ConnectFourPlayer.Yellow; 
        // column 6
        state.Grid[5,6] = ConnectFourPlayer.Red;
        state.Grid[4,6] = ConnectFourPlayer.Yellow;
        state.Grid[3,6] = ConnectFourPlayer.Red;
        state.Grid[2,6] = ConnectFourPlayer.Yellow;
        state.Grid[1,6] = ConnectFourPlayer.Red;
        state.Grid[0,6] = ConnectFourPlayer.Yellow; 

        Assert.True(_connectFourDomain.IsStateTerminal(state));
    }

    [Fact]
    public void ConnectFourDomain_GetTerminalStateValue_Draw_0()
    {  
        var state = _connectFourDomain.GetInitialState();
        // column 0
        state.Grid[5,0] = ConnectFourPlayer.Red;
        state.Grid[4,0] = ConnectFourPlayer.Yellow;
        state.Grid[3,0] = ConnectFourPlayer.Red;
        state.Grid[2,0] = ConnectFourPlayer.Yellow;
        state.Grid[1,0] = ConnectFourPlayer.Red;
        state.Grid[0,0] = ConnectFourPlayer.Yellow;
        // column 1
        state.Grid[5,1] = ConnectFourPlayer.Red;
        state.Grid[4,1] = ConnectFourPlayer.Yellow;
        state.Grid[3,1] = ConnectFourPlayer.Red;
        state.Grid[2,1] = ConnectFourPlayer.Yellow;
        state.Grid[1,1] = ConnectFourPlayer.Red;
        state.Grid[0,1] = ConnectFourPlayer.Yellow; 
        // column 2
        state.Grid[5,2] = ConnectFourPlayer.Yellow;
        state.Grid[4,2] = ConnectFourPlayer.Red;
        state.Grid[3,2] = ConnectFourPlayer.Yellow;
        state.Grid[2,2] = ConnectFourPlayer.Red;
        state.Grid[1,2] = ConnectFourPlayer.Yellow;
        state.Grid[0,2] = ConnectFourPlayer.Red; 
        // column 3
        state.Grid[5,3] = ConnectFourPlayer.Yellow;
        state.Grid[4,3] = ConnectFourPlayer.Red;
        state.Grid[3,3] = ConnectFourPlayer.Yellow;
        state.Grid[2,3] = ConnectFourPlayer.Red;
        state.Grid[1,3] = ConnectFourPlayer.Yellow;
        state.Grid[0,3] = ConnectFourPlayer.Red; 
        // column 4
        state.Grid[5,4] = ConnectFourPlayer.Red;
        state.Grid[4,4] = ConnectFourPlayer.Yellow;
        state.Grid[3,4] = ConnectFourPlayer.Red;
        state.Grid[2,4] = ConnectFourPlayer.Yellow;
        state.Grid[1,4] = ConnectFourPlayer.Red;
        state.Grid[0,4] = ConnectFourPlayer.Yellow; 
        // column 5
        state.Grid[5,5] = ConnectFourPlayer.Red;
        state.Grid[4,5] = ConnectFourPlayer.Yellow;
        state.Grid[3,5] = ConnectFourPlayer.Red;
        state.Grid[2,5] = ConnectFourPlayer.Yellow;
        state.Grid[1,5] = ConnectFourPlayer.Red;
        state.Grid[0,5] = ConnectFourPlayer.Yellow; 
        // column 6
        state.Grid[5,6] = ConnectFourPlayer.Red;
        state.Grid[4,6] = ConnectFourPlayer.Yellow;
        state.Grid[3,6] = ConnectFourPlayer.Red;
        state.Grid[2,6] = ConnectFourPlayer.Yellow;
        state.Grid[1,6] = ConnectFourPlayer.Red;
        state.Grid[0,6] = ConnectFourPlayer.Yellow; 

        double expectedValue = 0;
        double actualValue = _connectFourDomain.GetTerminalStateValue(state);

        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public void ConnectFourDomain_IsTerminalState_RedWinsHorizontal_True()
    {  
        var state = _connectFourDomain.GetInitialState();

        state.Grid[5,0] = ConnectFourPlayer.Red;
        state.Grid[5,1] = ConnectFourPlayer.Red;
        state.Grid[5,2] = ConnectFourPlayer.Red;
        state.Grid[5,3] = ConnectFourPlayer.Red;
        
        state.Grid[4,0] = ConnectFourPlayer.Yellow;
        state.Grid[4,1] = ConnectFourPlayer.Yellow;
        state.Grid[4,2] = ConnectFourPlayer.Yellow;
        
        Assert.True(_connectFourDomain.IsStateTerminal(state));
    }

    [Fact]
    public void ConnectFourDomain_GetTerminalStateValue_RedWinsHorizontal_1()
    {  
        var state = _connectFourDomain.GetInitialState();

        state.Grid[5,0] = ConnectFourPlayer.Red;
        state.Grid[5,1] = ConnectFourPlayer.Red;
        state.Grid[5,2] = ConnectFourPlayer.Red;
        state.Grid[5,3] = ConnectFourPlayer.Red;
        
        state.Grid[4,0] = ConnectFourPlayer.Yellow;
        state.Grid[4,1] = ConnectFourPlayer.Yellow;
        state.Grid[4,2] = ConnectFourPlayer.Yellow;
        
        double expectedValue = 1;
        double actualValue = _connectFourDomain.GetTerminalStateValue(state);

        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public void ConnectFourDomain_IsTerminalState_YellowWinsHorizontal_True()
    {  
        var state = _connectFourDomain.GetInitialState();

        state.Grid[5,0] = ConnectFourPlayer.Red;
        state.Grid[5,1] = ConnectFourPlayer.Red;
        state.Grid[5,2] = ConnectFourPlayer.Red;
        state.Grid[4,0] = ConnectFourPlayer.Red;
        
        state.Grid[5,3] = ConnectFourPlayer.Yellow;
        state.Grid[5,4] = ConnectFourPlayer.Yellow;
        state.Grid[5,5] = ConnectFourPlayer.Yellow;
        state.Grid[5,6] = ConnectFourPlayer.Yellow;
        
        Assert.True(_connectFourDomain.IsStateTerminal(state));
    }

    [Fact]
    public void ConnectFourDomain_GetTerminalStateValue_YellowWinsHorizontal_Negative1()
    {  
        var state = _connectFourDomain.GetInitialState();

        state.Grid[5,0] = ConnectFourPlayer.Red;
        state.Grid[5,1] = ConnectFourPlayer.Red;
        state.Grid[5,2] = ConnectFourPlayer.Red;
        state.Grid[4,0] = ConnectFourPlayer.Red;
        
        state.Grid[5,3] = ConnectFourPlayer.Yellow;
        state.Grid[5,4] = ConnectFourPlayer.Yellow;
        state.Grid[5,5] = ConnectFourPlayer.Yellow;
        state.Grid[5,6] = ConnectFourPlayer.Yellow;
        
        double expectedValue = -1;
        double actualValue = _connectFourDomain.GetTerminalStateValue(state);

        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public void ConnectFourDomain_IsTerminalState_RedWinsVertical_True()
    {  
        var state = _connectFourDomain.GetInitialState();

        state.Grid[2,0] = ConnectFourPlayer.Red;
        state.Grid[3,0] = ConnectFourPlayer.Red;
        state.Grid[4,0] = ConnectFourPlayer.Red;
        state.Grid[5,0] = ConnectFourPlayer.Red;
        
        state.Grid[3,1] = ConnectFourPlayer.Yellow;
        state.Grid[4,1] = ConnectFourPlayer.Yellow;
        state.Grid[5,1] = ConnectFourPlayer.Yellow;
        
        Assert.True(_connectFourDomain.IsStateTerminal(state));
    }

    [Fact]
    public void ConnectFourDomain_GetTerminalStateValue_RedWinsVertical_1()
    {  
        var state = _connectFourDomain.GetInitialState();

        state.Grid[2,0] = ConnectFourPlayer.Red;
        state.Grid[3,0] = ConnectFourPlayer.Red;
        state.Grid[4,0] = ConnectFourPlayer.Red;
        state.Grid[5,0] = ConnectFourPlayer.Red;
        
        state.Grid[3,1] = ConnectFourPlayer.Yellow;
        state.Grid[4,1] = ConnectFourPlayer.Yellow;
        state.Grid[5,1] = ConnectFourPlayer.Yellow;
        
        double expectedValue = 1;
        double actualValue = _connectFourDomain.GetTerminalStateValue(state);

        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public void ConnectFourDomain_IsTerminalState_RedWinsDiagonalUp_True()
    {  
        var state = _connectFourDomain.GetInitialState();

        state.Grid[5,0] = ConnectFourPlayer.Red;
        state.Grid[4,1] = ConnectFourPlayer.Red;
        state.Grid[3,2] = ConnectFourPlayer.Red;
        state.Grid[2,3] = ConnectFourPlayer.Red;
                
        Assert.True(_connectFourDomain.IsStateTerminal(state));
    }

    [Fact]
    public void ConnectFourDomain_GetTerminalStateValue_RedWinsDiagonalUp_1()
    {  
        var state = _connectFourDomain.GetInitialState();

        state.Grid[5,0] = ConnectFourPlayer.Red;
        state.Grid[4,1] = ConnectFourPlayer.Red;
        state.Grid[3,2] = ConnectFourPlayer.Red;
        state.Grid[2,3] = ConnectFourPlayer.Red;
                
        double expectedValue = 1;
        double actualValue = _connectFourDomain.GetTerminalStateValue(state);

        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public void ConnectFourDomain_IsTerminalState_RedWinsDiagonalDown_True()
    {  
        var state = _connectFourDomain.GetInitialState();

        state.Grid[2,0] = ConnectFourPlayer.Red;
        state.Grid[3,1] = ConnectFourPlayer.Red;
        state.Grid[4,2] = ConnectFourPlayer.Red;
        state.Grid[5,3] = ConnectFourPlayer.Red;
                
        Assert.True(_connectFourDomain.IsStateTerminal(state));
    }

    [Fact]
    public void ConnectFourDomain_GetTerminalStateValue_RedWinsDiagonalDown_1()
    {  
        var state = _connectFourDomain.GetInitialState();

        state.Grid[2,0] = ConnectFourPlayer.Red;
        state.Grid[3,1] = ConnectFourPlayer.Red;
        state.Grid[4,2] = ConnectFourPlayer.Red;
        state.Grid[5,3] = ConnectFourPlayer.Red;
                
        double expectedValue = 1;
        double actualValue = _connectFourDomain.GetTerminalStateValue(state);

        Assert.Equal(expectedValue, actualValue);
    }
}