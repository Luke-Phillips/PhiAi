using PhiAi.Core;

namespace Examples;

public class TwoInARowState: IState
{
    public string[] Boxes { get; set; } = { string.Empty, string.Empty, string.Empty, string.Empty };
    public string Agent { get; set; } = TwoInARowDomain.Black;
    public bool IsTerminal { get; set; } = false;

    public bool Equals(IState state)
    {
        TwoInARowState twoInARowState = (TwoInARowState) state;
        for (int box = 0; box < TwoInARowDomain.NumBoxes; box++)
        {
            if (Boxes[box] != twoInARowState.Boxes[box])
            {
                return false;
            }
        }

        return true;
    }
}