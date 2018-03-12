using UnityEngine;

/*
 * This classes provides the logic utilies needed to play a checkers game. This class sets up a board of pieces and then determines whether moves are valid, performs the logic of executing moves, etc.
 */

public class GameLogic {
    
    private Piece[][] _myBoard;
    private Piece[] _pieceList;
    public checkerBoard CheckBoard;
    public bool WaitForJump;

    public void Start(checkerBoard check)
    {
        CheckBoard = check;
    }

    //Used to initialize the board to the starting position
    public void InitBoard()
    {

        _myBoard = new Piece[8][];
        _pieceList = new Piece[24];
        WaitForJump = false;

        for (int x = 0; x < 8; x++)
        {
            _myBoard[x] = new Piece[8];
        }


        //add white pieces
        int z = 0;
        for (int y = 0; y < 3; y++)
        {
            if (y % 2 == 0)
            {
                for (int x = 0; x < 8; x += 2)
                {
                    _pieceList[z] = new Piece(z, y, x, true);
                    _myBoard[x][y] = _pieceList[z];
                    z++;
                }

            }
            else
            {
                for (int x = 1; x < 8; x += 2)
                {
                    _pieceList[z] = new Piece(z, y, x, true);
                    _myBoard[x][y] = _pieceList[z];
                    z++;
                }
            }
        }

        //generate black pieces
        for (int y = 7; y > 4; y--)
        {
            if (y % 2 == 0)
            {
                for (int x = 0; x < 8; x += 2)
                {
                    _pieceList[z] = new Piece(z, y, x, false);
                    _myBoard[x][y] = _pieceList[z];
                    z++;
                }

            }
            else
            {
                for (int x = 1; x < 8; x += 2)
                {
                    _pieceList[z] = new Piece(z, y, x, false);
                    _myBoard[x][y] = _pieceList[z];
                    z++;
                }
            }
        }
        
    }

    //Used to select a piece by the position on the board
    public Piece SelectPiece(int x, int y)
    {
        if (x < 0 || x >= 8 || y < 0 || y >= 8)
           return null;

        return _myBoard[x][y];
    }

    //tell whether a move is valid or not
    public bool ValidMove(Move move)
    {

        Piece piece = move.GetPiece();
        int x = move.GetMoveX();
        int y = move.GetMoveY();

        int oldx = move.GetStartX();
        int oldy = move.GetStartY();



        if (piece == null)
        {
            return false;
        }

        //check if its a king, otherwise
        //check its moving foward

        if (!piece.IsKing())
        {
            if (piece.IsWhite() && y < oldy)
            {
                return false;
            }
            else if ((!piece.IsWhite()) && y > oldy)
            {
                return false;
            }
        }

        if(_myBoard[x][y] != null)
        {
            return false;
        }

        int xdiff = Mathf.Abs(oldx - x);
        int ydiff = Mathf.Abs(oldy - y);
        //check if the movement is diagonal
        if (xdiff != ydiff)
        {
            return false;
        }

        //check if the movement is more than one space that there is a piece to capture in between NOT DONE
        
        if (xdiff > 1 || ydiff > 1)
        {
            if (!CanJumpNext(move))
            {
                return false;
            }
        }
        else
        {
            if (WaitForJump)
            {
                return false;
            }
        }
        

        return true;
    }

    //given a valid move, applies the move to the board
    public bool MakeMove(Move move)
    {
        Piece piece = move.GetPiece();
        int x = move.GetMoveX();
        int y = move.GetMoveY();

        int oldx = move.GetStartX();
        int oldy = move.GetStartY();

        if (!ValidMove(move)){
            return false;
        }
        
        int xdiff = Mathf.Abs(oldx - x);
        int ydiff = Mathf.Abs(oldy - y);

        if (xdiff > 1 || ydiff > 1)
        {
            if (CanJumpNext(move))
            {
                int x1 = move.GetStartX();
                int y1 = move.GetStartY();

                int x3 = move.GetMoveX();
                int y3 = move.GetMoveY();

                int x2, y2;

                if (x1 > x3)
                {
                    x2 = x1 - 1;
                }
                else
                {
                    x2 = x3 - 1;
                }

                if (y1 > y3)
                {
                    y2 = y1 - 1;
                }
                else
                {
                    y2 = y3 - 1;
                }

                if (_myBoard[x2][y2] == null)
                {
                    Debug.Log("Error in jump logic");
                }
                else
                {
                    int pieceId = _myBoard[x2][y2].GetId();
                    _pieceList[_myBoard[x2][y2].GetId()].Capture();
                    _myBoard[x2][y2] = null;
                }

                //update x and y
                piece.UpdateX(x);
                piece.UpdateY(y);

                _myBoard[x][y] = piece;
                _myBoard[oldx][oldy] = null;

                if (CanJump(piece))
                {
                    CheckBoard.UpdateBoard(_pieceList);
                    WaitForJump = true;
                    return true;
                }

                else
                {
                    //if need be king the piece
                    if (y == 7 || y == 0)
                    {
                        piece.KingMe();
                    }

                    WaitForJump = false;
                    CheckBoard.UpdateBoard(_pieceList);
                    CheckBoard.ChangeTurn();
                    CheckWin();

                    return true;
                }

            }
        }

        //if need be king the piece
        if (y == 7 || y == 0)
        {
            piece.KingMe();
        }

        WaitForJump = false;

        //update x and y
        piece.UpdateX(x);
        piece.UpdateY(y);

        _myBoard[x][y] = piece;
        _myBoard[oldx][oldy] = null;

        CheckBoard.UpdateBoard(_pieceList);
        CheckBoard.ChangeTurn();
        CheckWin();

        return true;
    }

    //returns the current board
    public Piece[][] GetBoard()
    {
        return _myBoard;
    }

    //returns a piece based on id
    public Piece GetPiece(int id)
    {
        if(id > 23 || id < 0)
        {
            return null;
        }

        return _pieceList[id];
    }

    //checks if win conditions are met
    public void CheckWin()
    {

        bool blackWin = true;
        bool whiteWin = true;

        foreach (Piece piece in _pieceList)
        {
            if (!piece.IsCaptured())
            {
                if (piece.IsWhite())
                {
                    blackWin = false;
                }
                else
                {
                    whiteWin = false;
                }
            }
        }

        if (blackWin && whiteWin)
        {
            Debug.Log("Both players win, something went wrong..."); 
        }

        if (blackWin)
        {
            CheckBoard.declareWinner(2);
        }

        if (whiteWin)
        {
            CheckBoard.declareWinner(1);
        }
    }

    //determines whether or not a piece has an available jump
    public bool CanJump(Piece piece)
    {
        int x1 = piece.GetX();
        int y1 = piece.GetY();

        if (piece.IsKing() || piece.IsWhite())
        {

            if (CanJumpNext(new Move(piece, x1, y1, x1 + 2, y1 + 2)))
            {
                return true;
            }

            if (CanJumpNext(new Move(piece, x1, y1, x1 - 2, y1 + 2)))
            {
                return true;
            }

        }

        if (piece.IsKing() || (!piece.IsWhite()))
        {

            if (CanJumpNext(new Move(piece, x1, y1, x1 - 2, y1 - 2)))
            {
                return true;
            }

            if (CanJumpNext(new Move(piece, x1, y1, x1 + 2, y1 - 2)))
            {
                return true;
            }

        }

        return false;
    }

    //takes a single length jump and determines if it is valid
    public bool CanJumpNext(Move move)
    {
        Piece piece = move.GetPiece();
        int x1 = move.GetStartX();
        int y1 = move.GetStartY();

        int x3 = move.GetMoveX();
        int y3 = move.GetMoveY();

        int x2, y2;

        if(x1 > x3)
        {
            x2 = x1 - 1;
        }
        else
        {
            x2 = x3 - 1;
        }

        if (y1 > y3)
        {
            y2 = y1 - 1;
        }
        else
        {
            y2 = y3 - 1;
        }

        if (x3 < 0 || x3 >= 8 || y3 < 0 || y3 >= 8)
            return false;  

        if (_myBoard[x3][y3] != null)
            return false;

        if (piece.IsKing())
        {
            if (_myBoard[x2][y2] == null)
                return false;
            if (piece.IsWhite() == _myBoard[x2][y2].IsWhite())
                return false;
            return true;
        }

        if (piece.IsWhite())
        {
            if (y3 < y1)
                return false;
            if (_myBoard[x2][y2] == null)
                return false;
            if (_myBoard[x2][y2].IsWhite())
                return false;
            return true; 
        }
        else
        {
            if (y3 > y1)
                return false;
            if (_myBoard[x2][y2] == null)
                return false;
            if (!_myBoard[x2][y2].IsWhite())
                return false;
            return true;
        }
    }

    //returns whether the board is frozen waiting for double jump input
    public bool WaitingForJump()
    {
        return WaitForJump;
    }


    //set the board with provided pieces (used mainly for testing)
    public void SetBoard(Piece[] pList)
    {
        _myBoard = new Piece[8][];

        for(int x = 0; x < 8; x++)
        {
            _myBoard[x] = new Piece[8];
        }

        _pieceList = new Piece[pList.Length];

        int z = 0;

        foreach(Piece piece in pList)
        {
            _myBoard[piece.GetX()][piece.GetY()] = piece;
            _pieceList[z] = piece;
            z++;
        }
    }


}
