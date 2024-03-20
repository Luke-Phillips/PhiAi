using PhiAi.Core;
using Xunit.Sdk;

namespace PhiAi.Test;

public class ConnectFourState: IState
{
    public string Agent { get; set; } = "red";
    public bool IsTerminal { get; set; } = false;
    
    public ConnectFourPlayer[,] Grid { get; set; } =
    {
        {ConnectFourPlayer.None, ConnectFourPlayer.None, ConnectFourPlayer.None, ConnectFourPlayer.None, ConnectFourPlayer.None, ConnectFourPlayer.None, ConnectFourPlayer.None},
        {ConnectFourPlayer.None, ConnectFourPlayer.None, ConnectFourPlayer.None, ConnectFourPlayer.None, ConnectFourPlayer.None, ConnectFourPlayer.None, ConnectFourPlayer.None},
        {ConnectFourPlayer.None, ConnectFourPlayer.None, ConnectFourPlayer.None, ConnectFourPlayer.None, ConnectFourPlayer.None, ConnectFourPlayer.None, ConnectFourPlayer.None},
        {ConnectFourPlayer.None, ConnectFourPlayer.None, ConnectFourPlayer.None, ConnectFourPlayer.None, ConnectFourPlayer.None, ConnectFourPlayer.None, ConnectFourPlayer.None},
        {ConnectFourPlayer.None, ConnectFourPlayer.None, ConnectFourPlayer.None, ConnectFourPlayer.None, ConnectFourPlayer.None, ConnectFourPlayer.None, ConnectFourPlayer.None},
        {ConnectFourPlayer.None, ConnectFourPlayer.None, ConnectFourPlayer.None, ConnectFourPlayer.None, ConnectFourPlayer.None, ConnectFourPlayer.None, ConnectFourPlayer.None},
    };

    public bool Equals(IState state)
    {
        ConnectFourPlayer[,] grid = ((ConnectFourState) state).Grid;
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                if (Grid[i,j] != grid[i,j])
                {
                    return false;
                }
            }
        }
        return true;
    }

    public static void PrettyPrint(ConnectFourState state)
    {
        for (int row = 0; row < 6; row++)
        {
            Console.Write("|");
            for (int col = 0; col < 7; col++)
            {
                switch (state.Grid[row,col])
                {
                    case ConnectFourPlayer.None:
                        Console.Write("     ");
                        break;
                    case ConnectFourPlayer.Red:
                        Console.Write(" (R) ");
                        break;
                    case ConnectFourPlayer.Yellow:
                        Console.Write(" ( ) ");
                        break;
                    
                }
            }
            Console.WriteLine("|");
            if (row != 5)
            {
                Console.WriteLine("|-----|-----|-----|-----|-----|-----|-----|");
            }
        }
    }

    public static ConnectFourState Copy(ConnectFourState state)
    {
        ConnectFourState copyState = new ConnectFourState
        {
            Agent = state.Agent
        };
        for (int row = 0; row < 6; row++)
        {
            for (int column = 0; column < 7; column++)
            {
                copyState.Grid[row,column] = state.Grid[row,column];
            }
        }
        return copyState;
    }

    public static ConnectFourPlayer AgentToPlayer(string agent)
    {
        switch (agent)
        {
            case "red":
                return ConnectFourPlayer.Red;
            case "yellow":
                return ConnectFourPlayer.Yellow;
            default:
                return ConnectFourPlayer.None;
        }
    }
}

public class ConnectFourAction: IAction
{
    public int Column { get; set; }
    public bool Equals(IAction action) => Column == ((ConnectFourAction) action).Column;
    public ConnectFourAction(int column)
    {
        Column = column;
    }
}

public class ConnectFourDomain: IDomain<ConnectFourState, ConnectFourAction>
{
    public ConnectFourState GetInitialState() => new ConnectFourState();
    public IEnumerable<ConnectFourAction> GetActionsFromState(ConnectFourState state)
    {
        var actions = new List<ConnectFourAction>();
        for (int column = 0; column < 7; column++)
        {
            if (state.Grid[0,column] == ConnectFourPlayer.None)
            {
                actions.Add(new ConnectFourAction(column));
            }
        }
        return actions;
    }

    public ConnectFourState GetStateFromStateAndAction(ConnectFourState state, ConnectFourAction action)
    {
        ConnectFourState newState;
        for (int row = 5; row >= 0; row--)
        {
            if (state.Grid[row, action.Column] == ConnectFourPlayer.None)
            {
                newState = ConnectFourState.Copy(state);
                newState.Agent = state.Agent == "red" ? "yellow" : "red";
                newState.Grid[row, action.Column] = ConnectFourState.AgentToPlayer(state.Agent);
                return newState;
            }
        }
        throw new TestClassException("column is full");
    }

    public bool IsStateTerminal(ConnectFourState state) => GetTerminalTuple(state).Item1;
    public double GetTerminalStateValue(ConnectFourState state) => GetTerminalTuple(state).Item2;

    public (bool, double) GetTerminalTuple(ConnectFourState state)
    {
        bool isDraw = true;

        ConnectFourPlayer player;
        int numInRow;

        // horizontal and draw
        for (int row = 0; row < 6; row++)
        {
            player = ConnectFourPlayer.None;
            numInRow = 0;
            for (int column = 0; column < 7; column++)
            {
                if (isDraw && row == 0 && state.Grid[row,column] == ConnectFourPlayer.None)
                {
                    isDraw = false;
                }
                if (row > 0 && isDraw)
                {
                    return (true, 0);
                }
                if (state.Grid[row,column] != ConnectFourPlayer.None && state.Grid[row,column] == player)
                {
                    numInRow++;
                }
                else
                {
                    player = state.Grid[row,column];
                    numInRow = 1;
                }
                if (numInRow == 4)
                {
                    return (true, player == ConnectFourPlayer.Red ? 1 : -1);
                }
            }
        }

        // vertical
        for (int column = 0; column < 7; column++)
        {
            player = ConnectFourPlayer.None;
            numInRow = 0;
            for (int row = 0; row < 6; row++)
            {
                if (state.Grid[row,column] != ConnectFourPlayer.None && state.Grid[row,column] == player)
                {
                    numInRow++;
                }
                else
                {
                    player = state.Grid[row, column];
                    numInRow = 1;
                }
                if (numInRow == 4)
                {
                    return (true, player == ConnectFourPlayer.Red ? 1 : -1);
                }
            }
        }

        // diagonal down
        for (int rowOffset = -3; rowOffset < 3; rowOffset++)
        {
            player = ConnectFourPlayer.None;
            numInRow = 0;
            for (int column = 0; column < 7; column++)
            {
                int row = column + rowOffset;
                if (row < 0 || row > 5)
                {
                    continue;
                }
                if (state.Grid[row,column] != ConnectFourPlayer.None && state.Grid[row,column] == player)
                {
                    numInRow++;
                }
                else
                {
                    player = state.Grid[row, column];
                    numInRow = 1;
                }
                if (numInRow == 4)
                {
                    return (true, player == ConnectFourPlayer.Red ? 1 : -1);
                }
            }
        }

        // diagonal up
        for (int rowOffset = 3; rowOffset < 9; rowOffset++)
        {
            player = ConnectFourPlayer.None;
            numInRow = 0;
            for (int column = 0; column < 7; column++)
            {
                int row = rowOffset - column;
                if (row < 0 || row > 5)
                {
                    continue;
                }
                if (state.Grid[row,column] != ConnectFourPlayer.None && state.Grid[row,column] == player)
                {
                    numInRow++;
                }
                else
                {
                    player = state.Grid[row, column];
                    numInRow = 1;
                }
                if (numInRow == 4)
                {
                    return (true, player == ConnectFourPlayer.Red ? 1 : -1);
                }
            }
        }

        return (false, 0);
    }
}

public enum ConnectFourPlayer
{
    None,
    Red,
    Yellow
}