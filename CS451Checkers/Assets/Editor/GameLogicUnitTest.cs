using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class GameLogicUnitTest {

    [Test]
    public void TestIntialBoard()
    {
        //Set up our game logic board
        GameLogic gl = new GameLogic();
        gl.InitBoard();
        Assert.False(gl.WaitingForJump());
        Piece current; 

        Piece[][] board = gl.GetBoard();

        //Check that every piece is initialized in the correct stop
        //check that we are referring to the correct pieceID
        //check that the pieces aren't set up as kings or are not captured yet and that they are the right color
        //check that no piece can initially jump (this is more checking the canJump function then setup)

        current = board[0][0];
        Assert.IsNotNull(current);
        Assert.True(current.IsWhite());
        Assert.False(current.IsKing());
        Assert.False(current.IsCaptured());
        Assert.True(current.GetId() == 0);
        Assert.True(current.GetX() == 0);
        Assert.False(current.GetY() == 0);
        Assert.False(gl.CanJump(current));

        current = board[2][0];
        Assert.IsNotNull(current);
        Assert.True(current.IsWhite());
        Assert.False(current.IsKing());
        Assert.False(current.IsCaptured());
        Assert.True(current.GetId() == 1);
        Assert.True(current.GetX() == 2);
        Assert.False(current.GetY() == 0);
        Assert.False(gl.CanJump(current));

        current = board[4][0];
        Assert.IsNotNull(current);
        Assert.True(current.IsWhite());
        Assert.False(current.IsKing());
        Assert.False(current.IsCaptured());
        Assert.True(current.GetId() == 2);
        Assert.True(current.GetX() == 4);
        Assert.False(current.GetY() == 0);
        Assert.False(gl.CanJump(current));

        current = board[6][0];
        Assert.IsNotNull(current);
        Assert.True(current.IsWhite());
        Assert.False(current.IsKing());
        Assert.False(current.IsCaptured());
        Assert.True(current.GetId() == 3);
        Assert.True(current.GetX() == 6);
        Assert.False(current.GetY() == 0);
        Assert.False(gl.CanJump(current));

        current = board[1][1];
        Assert.IsNotNull(current);
        Assert.True(current.IsWhite());
        Assert.False(current.IsKing());
        Assert.False(current.IsCaptured());
        Assert.True(current.GetId() == 4);
        Assert.True(current.GetX() == 1);
        Assert.False(current.GetY() == 1);
        Assert.False(gl.CanJump(current));

        current = board[3][1];
        Assert.IsNotNull(current);
        Assert.True(current.IsWhite());
        Assert.False(current.IsKing());
        Assert.False(current.IsCaptured());
        Assert.True(current.GetId() == 5);
        Assert.True(current.GetX() == 3);
        Assert.False(current.GetY() == 1);
        Assert.False(gl.CanJump(current));

        current = board[5][1];
        Assert.IsNotNull(current);
        Assert.True(current.IsWhite());
        Assert.False(current.IsKing());
        Assert.False(current.IsCaptured());
        Assert.True(current.GetId() == 6);
        Assert.True(current.GetX() == 5);
        Assert.False(current.GetY() == 1);
        Assert.False(gl.CanJump(current));

        current = board[7][1];
        Assert.IsNotNull(current);
        Assert.True(current.IsWhite());
        Assert.False(current.IsKing());
        Assert.False(current.IsCaptured());
        Assert.True(current.GetId() == 7);
        Assert.True(current.GetX() == 7);
        Assert.False(current.GetY() == 1);
        Assert.False(gl.CanJump(current));

        current = board[0][2];
        Assert.IsNotNull(current);
        Assert.True(current.IsWhite());
        Assert.False(current.IsKing());
        Assert.False(current.IsCaptured());
        Assert.True(current.GetId() == 8);
        Assert.True(current.GetX() == 0);
        Assert.False(current.GetY() == 2);
        Assert.False(gl.CanJump(current));

        current = board[2][2];
        Assert.IsNotNull(current);
        Assert.True(current.IsWhite());
        Assert.False(current.IsKing());
        Assert.False(current.IsCaptured());
        Assert.True(current.GetId() == 9);
        Assert.True(current.GetX() == 2);
        Assert.False(current.GetY() == 2);
        Assert.False(gl.CanJump(current));

        current = board[4][2];
        Assert.IsNotNull(current);
        Assert.True(current.IsWhite());
        Assert.False(current.IsKing());
        Assert.False(current.IsCaptured());
        Assert.True(current.GetId() == 10);
        Assert.True(current.GetX() == 4);
        Assert.False(current.GetY() == 2);
        Assert.False(gl.CanJump(current));

        current = board[6][2];
        Assert.IsNotNull(current);
        Assert.True(current.IsWhite());
        Assert.False(current.IsKing());
        Assert.False(current.IsCaptured());
        Assert.True(current.GetId() == 11);
        Assert.True(current.GetX() == 6);
        Assert.False(current.GetY() == 2);
        Assert.False(gl.CanJump(current));

        current = board[3][1];
        Assert.IsNull(current);

        current = board[4][5];
        Assert.IsNull(current);

        current = board[1][5];
        Assert.IsNotNull(current);
        Assert.True(current.IsWhite());
        Assert.False(current.IsKing());
        Assert.False(current.IsCaptured());
        Assert.True(current.GetId() == 12);
        Assert.True(current.GetX() == 1);
        Assert.False(current.GetY() == 5);
        Assert.False(gl.CanJump(current));

        current = board[3][5];
        Assert.IsNotNull(current);
        Assert.False(current.IsWhite());
        Assert.False(current.IsKing());
        Assert.False(current.IsCaptured());
        Assert.True(current.GetId() == 13);
        Assert.True(current.GetX() == 3);
        Assert.False(current.GetY() == 5);
        Assert.False(gl.CanJump(current));

        current = board[5][5];
        Assert.IsNotNull(current);
        Assert.False(current.IsWhite());
        Assert.False(current.IsKing());
        Assert.False(current.IsCaptured());
        Assert.True(current.GetId() == 14);
        Assert.True(current.GetX() == 5);
        Assert.False(current.GetY() == 5);
        Assert.False(gl.CanJump(current));

        current = board[7][5];
        Assert.IsNotNull(current);
        Assert.False(current.IsWhite());
        Assert.False(current.IsKing());
        Assert.False(current.IsCaptured());
        Assert.True(current.GetId() == 15);
        Assert.True(current.GetX() == 7);
        Assert.False(current.GetY() == 5);
        Assert.False(gl.CanJump(current));

        current = board[0][6];
        Assert.IsNotNull(current);
        Assert.False(current.IsWhite());
        Assert.False(current.IsKing());
        Assert.False(current.IsCaptured());
        Assert.True(current.GetId() == 16);
        Assert.True(current.GetX() == 0);
        Assert.False(current.GetY() == 6);
        Assert.False(gl.CanJump(current));

        current = board[2][6];
        Assert.IsNotNull(current);
        Assert.False(current.IsWhite());
        Assert.False(current.IsKing());
        Assert.False(current.IsCaptured());
        Assert.True(current.GetId() == 17);
        Assert.True(current.GetX() == 2);
        Assert.False(current.GetY() == 6);
        Assert.False(gl.CanJump(current));

        current = board[4][6];
        Assert.IsNotNull(current);
        Assert.False(current.IsWhite());
        Assert.False(current.IsKing());
        Assert.False(current.IsCaptured());
        Assert.True(current.GetId() == 18);
        Assert.True(current.GetX() == 4);
        Assert.False(current.GetY() == 6);
        Assert.False(gl.CanJump(current));

        current = board[6][6];
        Assert.IsNotNull(current);
        Assert.False(current.IsWhite());
        Assert.False(current.IsKing());
        Assert.False(current.IsCaptured());
        Assert.True(current.GetId() == 19);
        Assert.True(current.GetX() == 6);
        Assert.False(current.GetY() == 6);
        Assert.False(gl.CanJump(current));

        current = board[1][7];
        Assert.IsNotNull(current);
        Assert.False(current.IsWhite());
        Assert.False(current.IsKing());
        Assert.False(current.IsCaptured());
        Assert.True(current.GetId() == 20);
        Assert.True(current.GetX() == 1);
        Assert.False(current.GetY() == 7);
        Assert.False(gl.CanJump(current));

        current = board[3][7];
        Assert.IsNotNull(current);
        Assert.False(current.IsWhite());
        Assert.False(current.IsKing());
        Assert.False(current.IsCaptured());
        Assert.True(current.GetId() == 21);
        Assert.True(current.GetX() == 3);
        Assert.False(current.GetY() == 7);
        Assert.False(gl.CanJump(current));

        current = board[5][7];
        Assert.IsNotNull(current);
        Assert.False(current.IsWhite());
        Assert.False(current.IsKing());
        Assert.False(current.IsCaptured());
        Assert.True(current.GetId() == 22);
        Assert.True(current.GetX() == 5);
        Assert.False(current.GetY() == 7);
        Assert.False(gl.CanJump(current));

        current = board[7][7];
        Assert.IsNotNull(current);
        Assert.False(current.IsWhite());
        Assert.False(current.IsKing());
        Assert.False(current.IsCaptured());
        Assert.True(current.GetId() == 23);
        Assert.True(current.GetX() == 7);
        Assert.False(current.GetY() == 7);
        Assert.False(gl.CanJump(current));
        
        //check the select piece and get piece functions
        Assert.Equals(current.GetId(), gl.SelectPiece(7,7).GetId());

        Assert.IsNull(gl.SelectPiece(-1, 5));
        Assert.IsNull(gl.SelectPiece(8, 4));
        Assert.IsNull(gl.SelectPiece(4, 8));
        Assert.IsNull(gl.SelectPiece(3, 1));

        Assert.IsNull(gl.GetPiece(24));
        Assert.IsNull(gl.GetPiece(-1));

        Assert.Equals(gl.GetPiece(0).GetId(), gl.SelectPiece(0, 0).GetId());
        

    }

    [Test]
    public void TestSimpleMoves()
    {
        //Set up our game logic board
        GameLogic gl = new GameLogic();
        gl.InitBoard();

        Piece[][] board = new Piece[8][];
        Piece[] pList = new Piece[4];

        for (int x = 0; x < 8; x++)
        {
            board[x] = new Piece[8];
        }

        //set up a few pieces
        Piece piece1 = new Piece(0, 1, 1, true);
        board[1][1] = piece1;
        pList[0] = piece1;
        Piece piece2 = new Piece(1, 2, 2, true);
        piece2.KingMe();
        board[2][2] = piece2;
        pList[1] = piece2;
        Piece piece3 = new Piece(2, 5, 3, false);
        board[3][5] = piece3;
        pList[2] = piece3;
        Piece piece4 = new Piece(3, 2, 6, false);
        piece4.KingMe();
        board[2][6] = piece3;
        pList[3] = piece3;

        //set the game logic board to this board
        gl.SetBoard(pList);

        //grab the board and test these two are still equal (testing setBoard)
        Piece[][] board2 = gl.GetBoard();

        for(int x = 0; x<8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                Assert.True(board[x][y] == board2[x][y]);
            }
        }

        //try a legal move
        Move move = new Move(gl.SelectPiece(1,1), 0, 2);
        Assert.True(gl.ValidMove(move));

        //try to move a piece not diagonal
        move = new Move(gl.SelectPiece(1, 1), 0, 1);
        Assert.False(gl.ValidMove(move));

        //try to move a piece many places
        move = new Move(gl.SelectPiece(1, 1), 0, 6);
        Assert.False(gl.ValidMove(move));

        //try to move one piece into another
        move = new Move(gl.SelectPiece(1, 1), 2, 2);
        Assert.False(gl.ValidMove(move));

        //try to move a white piece down
        move = new Move(gl.SelectPiece(1, 1), 0, 0);
        Assert.False(gl.ValidMove(move));

        //try to move a black piece up
        move = new Move(gl.SelectPiece(5, 3), 4, 2);
        Assert.False(gl.ValidMove(move));

        //try a legal move with a black piece
        move = new Move(gl.SelectPiece(5, 3), 6, 2);
        Assert.True(gl.ValidMove(move));

        //try to move a white king down
        move = new Move(gl.SelectPiece(2, 2), 3, 1);
        Assert.True(gl.ValidMove(move));

        //try to move a black king up
        move = new Move(gl.SelectPiece(2, 6), 1, 7);
        Assert.True(gl.ValidMove(move));
    }

    [Test]
    public void TestJumpMoves()
    {

    }



}
