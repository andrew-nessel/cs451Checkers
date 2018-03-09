using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {
    
    private Piece[][] myBoard;
    private Piece[] pieceList;
    public checkerBoard checkBoard;

    private void initBoard()
    {

        checkBoard = GetComponent<checkerBoard>();
        myBoard = new Piece[8][];
        pieceList = new Piece[24];

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

        //generate black pieces
        for (int y = 7; y > 4; y--)
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
        int x = move.getX();
        int y = move.getY();

        if (piece == null)
        {
            Debug.Log("We got a null piece in valid");
        }

        int oldx = piece.getX();
        int oldy = piece.getY();

        //check if its a king, otherwise
        //check its moving foward

        if (!piece.isKing())
        {
            if (piece.isWhite() && y > oldy)
            {
                return false;
            }
            else if ((!piece.isWhite()) && y < oldy)
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
        /*
        if (xdiff > 1 || ydiff > 1)
        {
            if (!canJump(move))
            {
                return false;
            }
        }
        */

        return true;
    }

    public bool makeMove(Move move)
    {
        Piece piece = move.getPiece();
        int x = move.getX();
        int y = move.getY();

        if(!validMove(move)){
            return false;
        }

        int oldx = piece.getX();
        int oldy = piece.getY();
        int xdiff = Mathf.Abs(oldx - x);
        int ydiff = Mathf.Abs(oldy - y);

        if (xdiff > 2 || ydiff > 2)
        {
            Move[] jumpMoves = getJumpMoves(move);
            //finish working with jump moves to capture pieces
        }


        //if need be king the piece
        if (y == 7 || y == 0)
        {
            piece.kingMe();
        }


        //update x and y
        piece.updateX(x);
        piece.updateY(y);

        myBoard[x][y] = piece;
        myBoard[oldx][oldy] = null;

        checkBoard.updateBoard(pieceList);

        return false;
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

    public bool canJump(Move move) //not finished
    {
        return false;
    }

    public bool canJumpNext(Move move) //not finished
    {
        return false;
    }

    public Move[] getJumpMoves(Move move) //not finished
    {
        return null;
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
