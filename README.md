# PhiAI

Implement AI for you games with PhiAI! Currently under development, PhiAI's v1 will feature the Monte Carlo Tree Search Algorithm.

# Algorithms

## Monte Carlo Tree Search

### Overview

Monte Carlo Tree Search is an algorithm best suited for finite, two-person, zero-sum, perfect-information, sequential games. It is the algorithm between the highly successful and well-known AlphaGo and AlphaZero.
It utilizes a game state tree. Each node in the tree represent a game state. The root node is the starting state. Edges between nodes represent an action, a transition from one state to the next.
As an example, let's use a simple game we'll call Two-in-a-Row. Two-in-a-Row features 4 boxes that start off empty. Two players take turns marking an empty box with their color (black or white). Black goes first. The first player to mark adjacent boxes wins. If neither player has adjacent boxes, the game is a draw. While a simple and incredibly boring game, it makes a good example. Here is a visualization of the game state tree.

![Tic Tac Toe Game State Tree from Wikipedia](/PhiAi/docs/images/tree.png)

The Monte Carlo Tree Search Algorithm will automatically search this tree, exploring paths with potential and exploiting rewarding ones.

### Usage

We will implement the Monte Carlo Tree Search Algorithm for Two-in-a-Row.

PhiAI exposes the `MonteCarloTreeSearchAlgorithm` class. Before instantiating, a domain must be defined. Our domain is Two-in-a-Row. We'll first create a representation of Two-in-a-row state by implementing the `IState` interface.

```csharp
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
```

We've added four members to this class:

| Member       | Type / Return Type     | Explanation                                                                                                                                                                                                                                                                      |
| ------------ | ---------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `Boxes`      | string array of size 4 | The core of Two-in-a-Row state. An empty string represents an empty box, and `"black"` or `"white"` represent a box that has been marked by black or white respectively                                                                                                          |
| `Agent`      | string                 | `IState.Agent` represents which player will be taking the next action from this state                                                                                                                                                                                            |
| `IsTerminal` | bool                   | `IState.IsTerminal` represents whether or not the state is terminal, meaning, no further actions can be taken from this state. However, `IsTerminal` does can remain false as long as the `IDomain.IsStateTerminal` method is implemented which we'll be doing for Two-in-a-Row. |
| `Equals`     | bool                   | `IState.Equals` should return true only if the state is equivalent to the state provided as an argument                                                                                                                                                                          |

Next, we'll create a representation of a Two-in-a-Row action (transition between two state) by implementing `IAction`.

```csharp
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
```

A Two-in-a-row action is simple. It only requires an integer to represent which box is being marked. `IState.Equals` must also be implemented and should return true when two actions represent the same action (from the same state).

Finally, we finish the Two-in-a-Row domain by implementation `IDomain<TState, TAction>`.

```csharp
public class TwoInARowDomain : IDomain<TwoInARowState, TwoInARowAction>
{
    public IEnumerable<TwoInARowAction> GetActionsFromState(TwoInARowState state)
    {
        var actions = new List<TwoInARowAction>();

        for (int box = 0; box < TwoInARowState.NumBoxes; box++)
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
        var newBoxes = new string[TwoInARowState.NumBoxes];
        for (int box = 0; box < TwoInARowState.NumBoxes; box++)
        {
            newBoxes[box] = state.Boxes[box];
        }

        newBoxes[action.Box] = state.Agent;

        var newState= new TwoInARowState()
        {
            Boxes = newBoxes,
            Agent = state.Agent == "black" ? "white" : "black",
        };

        return newState;
    }

    public bool IsStateTerminal(TwoInARowState state) => GetTerminalStateAndValue(state).Item1;

    public double GetTerminalStateValue(TwoInARowState state) => GetTerminalStateAndValue(state).Item2;

    private (bool, double) GetTerminalStateAndValue(TwoInARowState state)
    {
        bool draw = true;
        string previousPlayer = string.Empty;
        for (int box = 0; box < TwoInARowState.NumBoxes; box++)
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
                    case "black":
                        return (true, 1);
                    case "white":
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
```

We've implemented all 5 methods of `IDomain` and one helper function. Let's go over each one.

| Member                       | Explanation                                                                                                                                                                                                                                                                                                                                                                                                                     |
| ---------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `GetActionsFromState`        | Returns an `IEnumerable` of all `TwoInARowAction`s that are possible from the given `TwoInARowState`. For Two-in-a-Row, it's one action per empty box.                                                                                                                                                                                                                                                                          |
| `GetStateFromStateAndAction` | Given a `TwoInARowState` and `TwoInARowAction` taken from that state, returns the new `TwoInARowState`. For Two-in-a-Row, new state is the same as old state, but with one additional box marked, and the the agent flipped to the next player.                                                                                                                                                                                 |
| `IsStateTerminal`            | Returns true if and only if the given `TwoInARowState` is terminal (game over, no more actions can be taken). For simplicity, we've decided a state is terminal when all boxes are filled (even though it's possible to infer a draw after the third move in many cases)                                                                                                                                                        |
| `GetTerminalStateValue`      | Returns a `double` that represents the "value" of the terminal state. Two-in-a-row is a zero sum game, so the values for each player should sum to zero. `1` is a win for black. `-1` is a win for white. `0` is a draw. These values are important as they help the algorithm know which actions increase the probability of winning. In PhiAi v0.x.x, it's important to not use values outside of the [-1, 1] range inclusive |
| `GetTerminalStateAndValue`   | Because `IsStateTerminal` and `GetTerminalStateAndValue` have similar code, we've created a helper method that combines the two.                                                                                                                                                                                                                                                                                                |

Now that we've created a domain for Two-in-a-Row, all we need to do is plug it into the Monte Carlo Tree Search Algorithm and we can use it however we want

```csharp
// Instantiate the algorithm
var twoInARowMcts = new MonteCarloTreeSearchAlgorithm<TwoInARowDomain, TwoInARowState, TwoInARowAction>();

// Let the algorithm do some searching. More iterations means better results.
twoInARowMcts.SearchForIterations(1000);

// Get all the actions we can take from the current state. They are sorted by best move to worst.
List<TwoInARowAction> actions = twoInARowMcts.GetActions().ToList();
if (actions.Count() == 0)
{
    // game over (terminal state reached if there are no actions)
}

// actions are sorted by probability of resulting in a win for the current agent/player (black in this case)
var bestAction = actions[0];
var worstAction = actions[actions.Count()-1];

// Decided on an action? Let black take it and update current state
twoInARowMcts.TakeAction(bestAction);

// Monte Carlo Tree Search works for both agents. It's now white's turn, but we can still use the algorithm
twoInARowMcts.SearchForIterations(1000);
var bestActionForWhite = twoInARowMcts.GetActions().First();
twoInARowMcts.TakeAction(bestActionForWhite);

// Is a human playing?
int boxToPlayIn;
if (int.TryParse(Console.ReadLine(), out boxToPlayIn))
{
    var humanAction = new TwoInARowAction(boxToPlayIn);
    try
    {
        twoInARowMcts.TakeAction(humanAction);
    }
    catch (Exception e)
    {
        // action was not possible from current state
    }
}

// Need to get the current state?
TwoInARowState state = twoInARowMcts.CurrentState;
```

# Versioning

PhiAI uses Semantic Versioning. Pre-1.0.0 versions will likely include breaking changes
