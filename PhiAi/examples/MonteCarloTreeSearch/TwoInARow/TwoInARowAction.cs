using PhiAi.Core;

namespace Examples;

public class TwoInARowAction: IAction
{
    public int Box { get; set; }

    public TwoInARowAction(int box)
    {
        Box = box;
    }
    
    public bool Equals(IAction action)
    {
        return Box == ((TwoInARowAction) action).Box;
    }
}