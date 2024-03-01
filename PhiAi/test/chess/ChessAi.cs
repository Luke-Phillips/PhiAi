using System.Security.Authentication.ExtendedProtection;
using PhiAi.Core;
using PhiAi.MonteCarloTreeSearch;

public class ChessAi
{
    private readonly IAlgorithm<ChessAction> _algorithm;
    public ChessAi()
    {
        _algorithm = new MonteCarloTreeSearchAlgorithm<ChessDomain, ChessState, ChessAction>();
    }

}

public class ChessState : IState
{
    public string State { get; set; } = "";
    public bool IsTerminal { get; set; } = true;
    public bool Equals(IState state)
    {
        return false;
    }
}

public class ChessAction : IAction
{
    public string Action { get; set; } = "";
    public bool Equals(IAction action)
    {
        return false;
    }
}

public class ChessDomain : IDomain<ChessState, ChessAction>
{
    public IEnumerable<ChessAction> GetActionsFromState(ChessState state)
    {
        return new List<ChessAction> {};
    }
    public ChessState GetStateFromStateAndAction(ChessState state, ChessAction action)
    {
        return new ChessState();
    }
    public bool IsStateTerminal(ChessState state)
    {
        return false;
    }
    public double GetTerminalStateValue(ChessState state)
    {
        return 1;
    }
    public ChessState GetInitialState()
    {
        return new ChessState();
    }
}