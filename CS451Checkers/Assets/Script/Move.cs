using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move {

    private Piece movePiece;
    private int startX;
    private int startY;
    private int moveX;
    private int moveY;

    public Move(Piece piece, int x, int y)
    {
        this.movePiece = piece;
        this.startX = piece.getX();
        this.startY = piece.getY();
        this.moveX = x;
        this.moveY = y;
    }

    public Move(Piece piece, int startX, int startY, int moveX, int moveY)
    {
        this.movePiece = piece;
        this.startX = piece.getX();
        this.startY = piece.getY();
        this.moveX = moveX;
        this.moveY = moveY;
    }

    public Piece getPiece()
    {
        return movePiece;
    }

    public int getMoveX()
    {
        return moveX;
    }

    public int getMoveY()
    {
        return moveY;
    }

    public int getStartX()
    {
        return startX;
    }

    public int getStartY()
    {
        return startY;
    }
}
