using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move {

    private Piece movePiece;
    private int moveX;
    private int moveY;

    public Move(Piece piece, int x, int y)
    {
        this.movePiece = piece;
        this.moveX = x;
        this.moveY = y;
    }

    public Piece getPiece()
    {
        return movePiece;
    }

    public int getX()
    {
        return moveX;
    }

    public int getY()
    {
        return moveY;
    }
}
