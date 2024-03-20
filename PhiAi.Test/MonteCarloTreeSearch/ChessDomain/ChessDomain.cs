using System.ComponentModel;
using PhiAi.Core;

namespace PhiAi.Test;

public class ChessState : IState
{
    public ChessPiece?[][] Board { get; set; } =
    [
        [new ChessPiece(ChessPieceType.Rook), new ChessPiece(ChessPieceType.Knight), new ChessPiece(ChessPieceType.Bishop), new ChessPiece(ChessPieceType.Queen), new ChessPiece(ChessPieceType.King), new ChessPiece(ChessPieceType.Bishop), new ChessPiece(ChessPieceType.Knight), new ChessPiece(ChessPieceType.Rook)],
        [new ChessPiece(), new ChessPiece(), new ChessPiece(), new ChessPiece(), new ChessPiece(), new ChessPiece(), new ChessPiece(), new ChessPiece()],
        [null, null, null, null, null, null, null, null],
        [null, null, null, null, null, null, null, null],
        [null, null, null, null, null, null, null, null],
        [null, null, null, null, null, null, null, null],
        [new ChessPiece(ChessPlayer.Black), new ChessPiece(ChessPlayer.Black), new ChessPiece(ChessPlayer.Black), new ChessPiece(ChessPlayer.Black), new ChessPiece(ChessPlayer.Black), new ChessPiece(ChessPlayer.Black), new ChessPiece(ChessPlayer.Black), new ChessPiece(ChessPlayer.Black)],
        [new ChessPiece(ChessPieceType.Rook, ChessPlayer.Black), new ChessPiece(ChessPieceType.Knight, ChessPlayer.Black), new ChessPiece(ChessPieceType.Bishop, ChessPlayer.Black), new ChessPiece(ChessPieceType.Queen, ChessPlayer.Black), new ChessPiece(ChessPieceType.King, ChessPlayer.Black), new ChessPiece(ChessPieceType.Bishop, ChessPlayer.Black), new ChessPiece(ChessPieceType.Knight, ChessPlayer.Black), new ChessPiece(ChessPieceType.Rook)],
    ];
    public int? EnPassantFile = null;
    public int MovesSinceLastCapture = 0;
    public bool IsTerminal { get; set; } = false;
    public string Agent { get; set; } = "white";
    public bool Equals(IState state) => false;
}

public class ChessAction : IAction
{
    public ChessSquare From { get; set; }
    public ChessSquare To { get; set; }
    public ChessPieceType? PromotionType { get; set; } = null;
    public bool IsEnPassant { get; set; } = false;
    public bool Equals(IAction action)
    {
        ChessAction chessAction = (ChessAction) action;
        return
            chessAction.PromotionType == PromotionType
            && chessAction.From.Rank == From.Rank
            && chessAction.From.File == From.File
            && chessAction.To.Rank == To.Rank
            && chessAction.To.File == To.File;
    }
}

public class ChessDomain : IDomain<ChessState, ChessAction>
{
    public IEnumerable<ChessAction> GetActionsFromState(ChessState state)
    {
        for (int file = 0; file < 8; file++)
        {
            for (int rank = 0; rank < 8; rank++)
            {
                ChessPiece? piece = state.Board[file][rank];
                if (piece == null || (state.Agent == "white" ? piece.Player == ChessPlayer.Black : piece.Player == ChessPlayer.White))
                {
                    continue;
                }

            }
        }
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

    private IEnumerable<ChessAction> GetActionsForPiece(ChessState state, ChessPiece piece, ChessSquare square)
    {
        var actions = new List<ChessAction>();
        bool isWhite = piece.Player == ChessPlayer.White;
        switch (piece.Type)
        {
            case ChessPieceType.Pawn:
                actions.AddRange(GetActionsForPawn(state, square, isWhite));
                break;
            case ChessPieceType.Knight:
                actions.AddRange(GetActionsForKnight(state, square, isWhite));
                break;
            case ChessPieceType.Bishop:
                actions.AddRange(GetActionsForBishop(state, square, isWhite));
                break;
            case ChessPieceType.Rook:
                actions.AddRange(GetActionsForBishop(state, square, isWhite));
                break;
            case ChessPieceType.Queen:
                actions.AddRange(GetActionsForBishop(state, square, isWhite));
                break;
            case ChessPieceType.King:
                actions.AddRange(GetActionsForBishop(state, square, isWhite));
                break;
            default:
                return new List<ChessAction> {};
        }
        foreach (var action in actions)
        {
            if (IsIncheck(isWhite, GetStateFromStateAndAction(state, action)) == ChessCheckStatus.Check)
            {
                actions.Remove(action);
            }
        }
        return actions;
    }

    private IEnumerable<ChessAction> GetActionsForPawn(ChessState state, ChessSquare square, bool isWhite)
    {
        var actions = new List<ChessAction>();
        if (state.Board[isWhite ? square.Rank + 1 : square.Rank - 1][square.File] is null)
        {
            // promote pawn
            if (square.Rank == (isWhite ? 6 : 1))
            {
                foreach (ChessPieceType type in new List<ChessPieceType> { ChessPieceType.Knight, ChessPieceType.Bishop, ChessPieceType.Rook, ChessPieceType.Queen})
                {
                    actions.Add(
                        new ChessAction
                        {
                            From = square,
                            To = new ChessSquare
                            {
                                Rank = isWhite ? square.Rank + 1 : square.Rank - 1,
                                File = square.File
                            },
                            PromotionType = type
                        }
                    );
                }
            }
            // advance 1 square
            else
            {
                actions.Add(
                    new ChessAction
                    {
                        From = square,
                        To = new ChessSquare
                        {
                            Rank = isWhite ? square.Rank + 1 : square.Rank - 1,
                            File = square.File
                        }
                    }
                );
            }

            // advance 2 squares
            if (isWhite ? square.Rank == 1 : square.Rank == 6 && state.Board[isWhite ? 3 : 4][square.File] is null)
            {
                actions.Add(
                    new ChessAction
                    {
                        From = square,
                        To = new ChessSquare
                        {
                            Rank = 3,
                            File = square.File
                        }
                    }
                );
            }
        }

        // standard capture left
        if (square.File != 0)
        {
            var capturablePiece = state.Board[isWhite ? square.Rank + 1 : square.Rank - 1][square.File - 1];
            if (capturablePiece is not null && capturablePiece.Player == (isWhite ? ChessPlayer.Black : ChessPlayer.White))
            {
                actions.Add(
                    new ChessAction
                    {
                        From = square,
                        To = new ChessSquare
                        {
                            Rank = isWhite ? square.Rank + 1 : square.Rank - 1,
                            File = square.File - 1
                        }
                    }
                );            
            }
        }

        // standard capture right
        if (square.File != 7)
        {
            var capturablePiece = state.Board[isWhite ? square.Rank + 1 : square.Rank - 1][square.File + 1];
            if (capturablePiece is not null && capturablePiece.Player == (isWhite ? ChessPlayer.Black : ChessPlayer.White))
            {
                actions.Add(
                    new ChessAction
                    {
                        From = square,
                        To = new ChessSquare
                        {
                            Rank = isWhite ? square.Rank + 1 : square.Rank - 1,
                            File = square.File + 1
                        }
                    }
                );     
            }
        }

        // en passant
        if (square.Rank == (isWhite ? 4 : 3) && (state.EnPassantFile == square.File - 1 || state.EnPassantFile == square.File + 1))
        {
            actions.Add(
                new ChessAction
                {
                    From = square,
                    To = new ChessSquare
                    {
                        Rank = isWhite ? square.Rank + 1 : square.Rank - 1,
                        File = state.EnPassantFile == square.File - 1 ? square.File - 1 : square.File + 1,
                    },
                    IsEnPassant = true
                }
            );     
        }

        return actions;
    }
    
    private IEnumerable<ChessAction> GetActionsForKnight(ChessState state, ChessSquare square, bool isWhite)
    {
        var actions = new List<ChessAction>();
        var jumps = new List<(int, int)> {(1, 2), (2, 1), (2, -1), (1, -2), (-1, -2), (-2, -1), (-2, 1), (-1, 2)};

        // todo make sure friendlies aren't on square
        foreach (var jump in jumps)
        {
            var newRank = square.Rank + jump.Item1;
            var newFile = square.File + jump.Item2;
            if (newRank >= 0 && newRank <= 7 && newFile >= 0 && newFile <= 7)
            {
                var toSquarePiece = state.Board[newRank][newFile];
                if (toSquarePiece is null || toSquarePiece.Player  == (isWhite ? ChessPlayer.Black : ChessPlayer.White))
                {
                    actions.Add(
                        new ChessAction
                        {
                            From = square,
                            To = new ChessSquare
                            {
                                Rank = newRank,
                                File = newFile
                            }
                        }
                    );     
                }
            }
        }

        return actions;
    }
    private IEnumerable<ChessAction> GetActionsForBishop(ChessState state, ChessSquare square, bool isWhite)
    {
        var actions = new List<ChessAction>();
        for (int distance = 1; distance < 7; distance++)
        {

            var toSquarePiece = state.Board[distance][distance];
            if (toSquarePiece is not null && toSquarePiece.Player == (isWhite ? ChessPlayer.White : ChessPlayer.Black))
            {
                break;
            }
            actions.Add(
                new ChessAction
                {
                    From = square,
                    To = new ChessSquare
                    {
                        Rank = distance,
                        File = distance
                    }
                }
            );     
        }
        for (int distance = 1; distance < 7; distance++)
        {
            var toSquarePiece = state.Board[distance][-distance];
            if (toSquarePiece is not null && toSquarePiece.Player == (isWhite ? ChessPlayer.White : ChessPlayer.Black))
            {
                break;
            }
            actions.Add(
                new ChessAction
                {
                    From = square,
                    To = new ChessSquare
                    {
                        Rank = distance,
                        File = -distance
                    }
                }
            );     
        }
        for (int distance = 1; distance < 7; distance++)
        {
            var toSquarePiece = state.Board[-distance][distance];
            if (toSquarePiece is not null && toSquarePiece.Player == (isWhite ? ChessPlayer.White : ChessPlayer.Black))
            {
                break;
            }
            actions.Add(
                new ChessAction
                {
                    From = square,
                    To = new ChessSquare
                    {
                        Rank = -distance,
                        File = distance
                    }
                }
            );     
        }
        for (int distance = 1; distance < 7; distance++)
        {
            var toSquarePiece = state.Board[-distance][-distance];
            if (toSquarePiece is not null && toSquarePiece.Player == (isWhite ? ChessPlayer.White : ChessPlayer.Black))
            {
                break;
            }
            actions.Add(
                new ChessAction
                {
                    From = square,
                    To = new ChessSquare
                    {
                        Rank = -distance,
                        File = -distance
                    }
                }
            );     
        }
        
        return actions;
    }
    private IEnumerable<ChessAction> GetActionsForRook(ChessState state, ChessSquare square, bool isWhite)
    {
        var actions = new List<ChessAction>();
        for (int distance = 1; distance < 7; distance++)
        {
            var toSquarePiece = state.Board[distance][square.File];
            if (toSquarePiece is not null && toSquarePiece.Player == (isWhite ? ChessPlayer.White : ChessPlayer.Black))
            {
                break;
            }
            actions.Add(
                new ChessAction
                {
                    From = square,
                    To = new ChessSquare
                    {
                        Rank = distance,
                        File = square.File
                    }
                }
            );     
        }
        for (int distance = 1; distance < 7; distance++)
        {
            var toSquarePiece = state.Board[-distance][square.File];
            if (toSquarePiece is not null && toSquarePiece.Player == (isWhite ? ChessPlayer.White : ChessPlayer.Black))
            {
                break;
            }
            actions.Add(
                new ChessAction
                {
                    From = square,
                    To = new ChessSquare
                    {
                        Rank = -distance,
                        File = square.File
                    }
                }
            );     
        }
        for (int distance = 1; distance < 7; distance++)
        {
            var toSquarePiece = state.Board[square.Rank][distance];
            if (toSquarePiece is not null && toSquarePiece.Player == (isWhite ? ChessPlayer.White : ChessPlayer.Black))
            {
                break;
            }
            actions.Add(
                new ChessAction
                {
                    From = square,
                    To = new ChessSquare
                    {
                        Rank = square.Rank,
                        File = distance
                    }
                }
            );     
        }
        for (int distance = 1; distance < 7; distance++)
        {
            var toSquarePiece = state.Board[square.Rank][-distance];
            if (toSquarePiece is not null && toSquarePiece.Player == (isWhite ? ChessPlayer.White : ChessPlayer.Black))
            {
                break;
            }
            actions.Add(
                new ChessAction
                {
                    From = square,
                    To = new ChessSquare
                    {
                        Rank = square.Rank,
                        File = -distance
                    }
                }
            );     
        }
        
        return actions;
    }
    private IEnumerable<ChessAction> GetActionsForQueen(ChessState state, ChessSquare square, bool isWhite)
    {
        var actions = new List<ChessAction>();
        actions.AddRange(GetActionsForBishop(state, square, isWhite));
        actions.AddRange(GetActionsForRook(state, square, isWhite));
        return actions;
    }
    private IEnumerable<ChessAction> GetActionsForKing(ChessState state, ChessSquare square, bool isWhite)
    {
        var actions = new List<ChessAction>();
        
        return actions;
    }

    private ChessCheckStatus IsIncheck(bool isWhite, ChessState state)
    {
        return ChessCheckStatus.Protected;
    }
}

public class ChessSquare
{
    public int Rank { get; set; }
    public int File { get; set; }
}

public class ChessPiece
{
    public ChessPieceType Type { get; set; }
    public ChessPlayer Player { get; set; }
    public ChessPiece()
    {
        Type = ChessPieceType.Pawn;
        Player = ChessPlayer.White;
    }
    public ChessPiece(ChessPlayer player)
    {
        Type = ChessPieceType.Pawn;
        Player = player;
    }
    public ChessPiece(ChessPieceType type)
    {
        Type = type;
        Player = ChessPlayer.White;
    }
    public ChessPiece(ChessPieceType type, ChessPlayer player)
    {
        Type = type;
        Player = player;
    }
}

public enum ChessPieceType
{
    Pawn,
    Knight,
    Bishop,
    Rook,
    Queen,
    King
}

public enum ChessPlayer
{
    White,
    Black
}

public enum ChessCheckStatus
{
    Protected,
    Check,
    Mate
}