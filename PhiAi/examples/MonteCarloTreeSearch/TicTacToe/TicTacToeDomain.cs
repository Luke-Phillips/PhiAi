using PhiAi.Core;

namespace PhiAi.Test;

// TODO - X (first player) is playing weird at 5000 iterations on move 3 (ply 5)
public class TicTacToeState : IState
{
    public bool IsTerminal { get; set; } = false;
    public string Agent { get; set; } = "x";
    public string[][] Grid { get; set; } =
    [
        [string.Empty, string.Empty, string.Empty],
        [string.Empty, string.Empty, string.Empty],
        [string.Empty, string.Empty, string.Empty]
    ];

    // Not necessary for state since we can loop through the grid,
    // but it will make other code easier to write
    public int SpacesRemaining { get; set; } = 9;

    public TicTacToeState(){}

    public TicTacToeState(TicTacToeState state)
    {
        Agent = state.Agent;
        Grid = new string[3][];
        SpacesRemaining = state.SpacesRemaining;
        for (int i = 0; i < 3; i++)
        {
            Grid[i] = new string[3];
            for (int j = 0; j < 3; j++)
            {
                Grid[i][j] = state.Grid[i][j];
            }
        }
    }

    public bool Equals(IState state)
    {
        TicTacToeState ticTacToeState = (TicTacToeState) state;
        if (Agent != ticTacToeState.Agent || SpacesRemaining != ticTacToeState.SpacesRemaining || IsTerminal != ticTacToeState.IsTerminal)
        {
            return false;
        }
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; i < 3; j++)
            {
                if (Grid[i][j] != ticTacToeState.Grid[i][j])
                {
                    return false;
                }
            }
        }
        return true;
    } 
}

public class TicTacToeAction : IAction
{
    public string Agent { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }

    public TicTacToeAction(string agent, int row, int column)
    {
        Agent = agent;
        Row = row;
        Column = column;
    }

    public bool Equals(IAction action)
    {
        TicTacToeAction ticTacToeAction = (TicTacToeAction) action;
        return (
            Agent == ticTacToeAction.Agent
            && Row == ticTacToeAction.Row
            && Column == ticTacToeAction.Column
        );
    }
}

public class TicTacToeDomain : IDomain<TicTacToeState, TicTacToeAction>
{
    public const string X = "x"; 
    public const string O = "o"; 
    private readonly string _player = X;

    public TicTacToeDomain() {}
    public TicTacToeDomain(string player)
    {
        if (player == X) _player = X;
    }

    public IEnumerable<TicTacToeAction> GetActionsFromState(TicTacToeState state)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (string.IsNullOrEmpty(state.Grid[i][j]))
                {
                   yield return new TicTacToeAction(state.Agent, i, j);
                }
            }
        }
    }

    public TicTacToeState GetStateFromStateAndAction(TicTacToeState state, TicTacToeAction action)
    {
        var newState = new TicTacToeState(state);
        if (newState.Agent == O)
        {
            newState.Agent = X;
        }
        else
        {
            newState.Agent = O;
        }
        newState.SpacesRemaining -= 1;
        newState.Grid[action.Row][action.Column] = action.Agent;

        return newState;
    }

    public bool IsStateTerminal(TicTacToeState state)
    {
        (bool isTerminal, double value)  = GetIsTerminalAndValue(state);
        return isTerminal;
    }

    public double GetTerminalStateValue(TicTacToeState state)
    {
        (bool isTerminal, double value)  = GetIsTerminalAndValue(state);
        return value;
    }

    public TicTacToeState GetInitialState()
    {
        return new TicTacToeState();
    }

    private (bool, double) GetIsTerminalAndValue(TicTacToeState state)
    {
        if (
            // across
            (state.Grid[0][0] == X && state.Grid[0][1] == X && state.Grid[0][2] == X) 
            || (state.Grid[1][0] == X && state.Grid[1][1] == X && state.Grid[1][2] == X) 
            || (state.Grid[2][0] == X && state.Grid[2][1] == X && state.Grid[2][2] == X) 
            // down
            || (state.Grid[0][0] == X && state.Grid[1][0] == X && state.Grid[2][0] == X) 
            || (state.Grid[0][1] == X && state.Grid[1][1] == X && state.Grid[2][1] == X) 
            || (state.Grid[0][2] == X && state.Grid[1][2] == X && state.Grid[2][2] == X) 
            // diagonal
            || (state.Grid[0][0] == X && state.Grid[1][1] == X && state.Grid[2][2] == X) 
            || (state.Grid[0][2] == X && state.Grid[1][1] == X && state.Grid[2][0] == X) 
        )
        {
            return (true, 1);
        }
        else if (
            // across
            (state.Grid[0][0] == O && state.Grid[0][1] == O && state.Grid[0][2] == O) 
            || (state.Grid[1][0] == O && state.Grid[1][1] == O && state.Grid[1][2] == O) 
            || (state.Grid[2][0] == O && state.Grid[2][1] == O && state.Grid[2][2] == O) 
            // down
            || (state.Grid[0][0] == O && state.Grid[1][0] == O && state.Grid[2][0] == O) 
            || (state.Grid[0][1] == O && state.Grid[1][1] == O && state.Grid[2][1] == O) 
            || (state.Grid[0][2] == O && state.Grid[1][2] == O && state.Grid[2][2] == O) 
            // diagonal
            || (state.Grid[0][0] == O && state.Grid[1][1] == O && state.Grid[2][2] == O) 
            || (state.Grid[0][2] == O && state.Grid[1][1] == O && state.Grid[2][0] == O) 
        )
        {
            return (true, -1);
        }
        else if (state.SpacesRemaining == 0)
        {
            return (true, 0);
        }
        else
        {
            return(false, 0);
        }
    }
}