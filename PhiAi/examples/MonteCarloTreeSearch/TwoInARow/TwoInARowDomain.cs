using PhiAi.Core;

namespace Examples;

public class TwoInARowDomain : IDomain<TwoInARowState, TwoInARowAction>
{
    public static int NumBoxes = 4;
    public const string Black = "black";
    public const string White = "white";

    public IEnumerable<TwoInARowAction> GetActionsFromState(TwoInARowState state)
    {
        var actions = new List<TwoInARowAction>();

        for (int box = 0; box < NumBoxes; box++)
        {
            if (state.Boxes[box] == string.Empty)
            {
                actions.Add(new TwoInARowAction(box));
            }
        }
        
        return actions;
    }

    public TwoInARowState GetStateFromStateAndAction(TwoInARowState state, TwoInARowAction action)
    {
        var newBoxes = new string[NumBoxes];
        for (int box = 0; box < NumBoxes; box++)
        {
            newBoxes[box] = state.Boxes[box];
        }
        
        newBoxes[action.Box] = state.Agent;

        var newState= new TwoInARowState()
        {
            Boxes = newBoxes,
            Agent = state.Agent == Black ? White : Black,
        };

        return newState;
    }

    public bool IsStateTerminal(TwoInARowState state) => GetTerminalStateAndValue(state).Item1;

    public double GetTerminalStateValue(TwoInARowState state) => GetTerminalStateAndValue(state).Item2;

    private (bool, double) GetTerminalStateAndValue(TwoInARowState state)
    {
        bool draw = true;
        string previousPlayer = string.Empty;
        for (int box = 0; box < NumBoxes; box++)
        {
            string player = state.Boxes[box];
            if (player == string.Empty)
            {
                draw = false;
            }
            if (player == previousPlayer)
            {
                switch (player)
                {
                    case Black:
                        return (true, 1);
                    case White:
                        return (true, -1);
                    default:
                        continue;
                }
            }
        }
        return (draw, 0);
    }

    public TwoInARowState GetInitialState() => new TwoInARowState();
}