using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {


    public class GameLogic
    {
        protected Piece[][] myBoard;


        void initBoard()
        {
            myBoard = new Piece[8][];

            for (int x = 0; x < 8; x++)
            {
                myBoard[x] = new Piece[8];
            }


            //add black pieces
            int z = 0;
            for (int y = 0; y < 3; y++)
            {
                int x = 0;
                while (x < 8)
                {
                    if (y % 2 == 0)
                    {
                        myBoard[y][x] = new Piece(z, y, x + 1, false);
                        x += 2;
                    }
                    else
                    {
                        myBoard[y][x] = new Piece(z, y, x, false);
                        x += 2;
                    }
                    z++;
                }
            }

            //add white peices
            for (int y = 5; y < 8; y++)
            {
                int x = 0;
                while (x < 8)
                {
                    if (y % 2 == 0)
                    {
                        myBoard[y][x] = new Piece(z, y, x + 1, true);
                        x += 2;
                    }
                    else
                    {
                        myBoard[y][x] = new Piece(z, y, x, true);
                        x += 2;
                    }
                    z++;
                }
            }
        }

        bool validMove(Piece piece, int x, int y)
        {
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

            int xdiff = oldx - x;
            int ydiff = oldy - y;
            //check if the movement is diagonal
            if (xdiff != ydiff)
            {
                return false;
            }

            //check if the movement is more than one space that there is a piece to capture in between NOT DONE

            return false;
        }

        bool makeMove(Piece piece, int x, int y)
        {
            //if(!validMove(piece, x, y)){
            //return false;
            //}

            //if there are peices in between new and old capture peices NOT DONE

            //if need be king the piece
            if (y == 7)
            {
                piece.kingMe();
            }


            //update x and y
            piece.updateX(x);
            piece.updateY(y);

            return false;
        }

        public Piece[][] getBoard()
        {
            return myBoard;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


}
