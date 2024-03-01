using PhiAi.Core;

namespace PhiAi.Test;

public class TicTacToeState : IState
{
    public bool IsTerminal { get; set; } = false;
    public string Turn { get; set; } = "o";
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
        Turn = state.Turn;
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
        if (Turn != ticTacToeState.Turn)
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
    public string Player { get; set; }
    public int Row { get; set; }
    public int Column { get; set; }

    public TicTacToeAction(string player, int row, int column)
    {
        Player = player;
        Row = row;
        Column = column;
    }

    public bool Equals(IAction action)
    {
        TicTacToeAction ticTacToeAction = (TicTacToeAction) action;
        return (
            Player == ticTacToeAction.Player
            && Row == ticTacToeAction.Row
            && Column == ticTacToeAction.Column
        );
    }
}

public class TicTacToeDomain : IDomain<TicTacToeState, TicTacToeAction>
{
    public const string X = "x"; 
    public const string O = "o"; 
    private readonly string _player = O;

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
                   yield return new TicTacToeAction(state.Turn, i, j);
                }
            }
        }
    }

    public TicTacToeState GetStateFromStateAndAction(TicTacToeState state, TicTacToeAction action)
    {
        var newState = new TicTacToeState(state);
        if (newState.Turn == O)
        {
            newState.Turn = X;
        }
        else
        {
            newState.Turn = O;
        }
        newState.SpacesRemaining -= 1;
        newState.Grid[action.Row][action.Column] = action.Player;

        return newState;
    }

    public bool IsStateTerminal(TicTacToeState state)
    {
        (bool isTerminal, double value)  = IsStateTerminalValue(state);
        return isTerminal;
    }

    public double GetTerminalStateValue(TicTacToeState state)
    {
        (bool isTerminal, double value)  = IsStateTerminalValue(state);
        return value;
    }

    public TicTacToeState GetInitialState()
    {
        return new TicTacToeState();
    }

    private (bool, double) IsStateTerminalValue(TicTacToeState state)
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
            return (true, _player == O ? -1 : 1);
        }
        else if (
            (state.Grid[0][0] == O && state.Grid[0][1] == O && state.Grid[0][2] == O) 
            || (state.Grid[0][2] == O && state.Grid[1][1] == O && state.Grid[2][0] == O) 
            || (state.Grid[1][0] == O && state.Grid[1][1] == O && state.Grid[1][2] == O) 
            || (state.Grid[0][0] == O && state.Grid[1][1] == O && state.Grid[2][2] == O) 
            || (state.Grid[2][0] == O && state.Grid[2][1] == O && state.Grid[2][2] == O) 
            || (state.Grid[0][2] == O && state.Grid[1][2] == O && state.Grid[2][2] == O) 
            || (state.Grid[0][1] == O && state.Grid[1][1] == O && state.Grid[2][1] == O) 
            || (state.Grid[0][0] == O && state.Grid[1][0] == O && state.Grid[2][0] == O) 
        )
        {
            return (true, _player == O ? 1 : -1);
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