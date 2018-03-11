using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {
    
    private Piece[][] myBoard;
    private Piece[] pieceList;
    public checkerBoard checkBoard;
    public bool waitForJump;

    private void initBoard()
    {

        checkBoard = GetComponent<checkerBoard>();
        myBoard = new Piece[8][];
        pieceList = new Piece[24];
        waitForJump = false;

        for (int x = 0; x < 8; x++)
        {
            myBoard[x] = new Piece[8];
        }


        //add white pieces
        int z = 0;
        for (int y = 0; y < 3; y++)
        {
            if (y % 2 == 0)
            {
                for (int x = 0; x < 8; x += 2)
                {
                    pieceList[z] = new Piece(z, y, x, true);
                    myBoard[x][y] = pieceList[z];
                    z++;
                }

            }
            else
            {
                for (int x = 1; x < 8; x += 2)
                {
                    pieceList[z] = new Piece(z, y, x, true);
                    myBoard[x][y] = pieceList[z];
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
                    pieceList[z] = new Piece(z, y, x, false);
                    myBoard[x][y] = pieceList[z];
                    z++;
                }

            }
            else
            {
                for (int x = 1; x < 8; x += 2)
                {
                    pieceList[z] = new Piece(z, y, x, false);
                    myBoard[x][y] = pieceList[z];
                    z++;
                }
            }
        }
        
    }

    public Piece selectPiece(int x, int y)
    {
        if (x < 0 || x >= 8 || y < 0 || y >= 8)
           return null;

        return myBoard[x][y];
    }

    public bool validMove(Move move)
    {

        Piece piece = move.getPiece();
        int x = move.getMoveX();
        int y = move.getMoveY();

        int oldx = move.getStartX();
        int oldy = move.getStartY();



        if (piece == null)
        {
            Debug.Log("We got a null piece in valid");
        }

        //check if its a king, otherwise
        //check its moving foward

        if (!piece.isKing())
        {
            if (piece.isWhite() && y < oldy)
            {
                return false;
            }
            else if ((!piece.isWhite()) && y > oldy)
            {
                return false;
            }
        }

        if(myBoard[x][y] != null)
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
            if (!canJumpNext(move))
            {
                return false;
            }
        }
        else
        {
            if (waitForJump)
            {
                return false;
            }
        }
        

        return true;
    }

    public bool makeMove(Move move)
    {
        Piece piece = move.getPiece();
        int x = move.getMoveX();
        int y = move.getMoveY();

        int oldx = move.getStartX();
        int oldy = move.getStartY();

        if (!validMove(move)){
            return false;
        }
        
        int xdiff = Mathf.Abs(oldx - x);
        int ydiff = Mathf.Abs(oldy - y);

        Debug.Log("diffs x " + xdiff + " y " + ydiff);

        if (xdiff > 1 || ydiff > 1)
        {
            if (canJumpNext(move))
            {
                Debug.Log("trying to remove pieces");
                int x1 = move.getStartX();
                int y1 = move.getStartY();

                int x3 = move.getMoveX();
                int y3 = move.getMoveY();

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

                Debug.Log("Start x:" + x1 + " y:" + y1);
                Debug.Log("End x:" + x3 + " y:" + y3);
                Debug.Log("Removing x:" + x2 + " y:" + y2);

                if (myBoard[x2][y2] == null)
                {
                    Debug.Log("Error in jump logic");
                }
                else
                {
                    int pieceID = myBoard[x2][y2].getID();
                    pieceList[myBoard[x2][y2].getID()].capture();
                    myBoard[x2][y2] = null;
                    Debug.Log("removing x:" + x2 + " y:" + y2 + " pieceID:" + pieceID);
                }

                //update x and y
                piece.updateX(x);
                piece.updateY(y);

                myBoard[x][y] = piece;
                myBoard[oldx][oldy] = null;

                if (canJump(piece))
                {
                    checkBoard.updateBoard(pieceList);
                    waitForJump = true;
                    return true;
                }

                else
                {
                    //if need be king the piece
                    if (y == 7 || y == 0)
                    {
                        piece.kingMe();
                    }

                    waitForJump = false;
                    checkBoard.updateBoard(pieceList);
                    checkBoard.changeTurn();
                    checkWin();

                    return true;
                }

            }
        }

        //if need be king the piece
        if (y == 7 || y == 0)
        {
            piece.kingMe();
        }

        waitForJump = false;

        //update x and y
        piece.updateX(x);
        piece.updateY(y);

        myBoard[x][y] = piece;
        myBoard[oldx][oldy] = null;

        checkBoard.updateBoard(pieceList);
        checkBoard.changeTurn();
        checkWin();

        return true;
    }

    public Piece[][] getBoard()
    {
        return myBoard;
    }

    public Piece getPiece(int ID)
    {
        if(ID > 23 || ID < 0)
        {
            return null;
        }

        Debug.Log("We got number " + ID + " and that is piece " + pieceList[ID].getID());

        return pieceList[ID];
    }

    public void checkWin()
    {

        bool blackWin = true;
        bool whiteWin = true;

        foreach (Piece piece in pieceList)
        {
            if (!piece.isCaptured())
            {
                if (piece.isWhite())
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
            Debug.Log("Both players win, something went wrong..."); //do something
        }

        if (blackWin)
        {
            Debug.Log("Black Wins"); //do something
        }

        if (whiteWin)
        {
            Debug.Log("White Wins"); //do something
        }
    }

    public bool canJump(Piece piece) //not finished
    {
        int x1 = piece.getX();
        int y1 = piece.getY();

        if (piece.isKing() || piece.isWhite())
        {

            if (canJumpNext(new Move(piece, x1, y1, x1 + 2, y1 + 2)))
            {
                return true;
            }

            if (canJumpNext(new Move(piece, x1, y1, x1 - 2, y1 + 2)))
            {
                return true;
            }

        }

        if (piece.isKing() || (!piece.isWhite()))
        {

            if (canJumpNext(new Move(piece, x1, y1, x1 - 2, y1 - 2)))
            {
                return true;
            }

            if (canJumpNext(new Move(piece, x1, y1, x1 + 2, y1 - 2)))
            {
                return true;
            }

        }

        return false;
    }

    public bool canJumpNext(Move move) //this only takes jumps that are single jumps
    {
        Piece piece = move.getPiece();
        int x1 = move.getStartX();
        int y1 = move.getStartY();

        int x3 = move.getMoveX();
        int y3 = move.getMoveY();

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

        if (myBoard[x3][y3] != null)
            return false;

        if (piece.isKing())
        {
            if (myBoard[x2][y2] == null)
                return false;
            if (piece.isWhite() == myBoard[x2][y2].isWhite())
                return false;
            return true;
        }

        if (piece.isWhite())
        {
            if (y3 < y1)
                return false;
            if (myBoard[x2][y2] == null)
                return false;
            if (myBoard[x2][y2].isWhite())
                return false;
            return true; 
        }
        else
        {
            if (y3 > y1)
                return false;
            if (myBoard[x2][y2] == null)
                return false;
            if (!myBoard[x2][y2].isWhite())
                return false;
            return true;
        }

        Debug.Log("We got here");

        return false;
    }

    public bool waitingForJump()
    {
        return waitForJump;
    }
    

    // Use this for initialization
    void Start()
    {
        initBoard();
    }

    // Update is called once per frame
    void Update()
    {

    }


}
