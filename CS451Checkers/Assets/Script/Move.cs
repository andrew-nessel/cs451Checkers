
/*
 * This classes acts as a container for the logical moves used by the board. Holds a piece, the piece's current position and where it intends to move
 */

public class Move {

    private Piece movePiece;
    private int startX;
    private int startY;
    private int moveX;
    private int moveY;

    public Move(Piece piece, int x, int y)
    {
        this.movePiece = piece;
        this.startX = piece.GetX();
        this.startY = piece.GetY();
        this.moveX = x;
        this.moveY = y;
    }

    public Move(Piece piece, int startX, int startY, int moveX, int moveY)
    {
        this.movePiece = piece;
        this.startX = piece.GetX();
        this.startY = piece.GetY();
        this.moveX = moveX;
        this.moveY = moveY;
    }

    public Piece GetPiece()
    {
        return movePiece;
    }

    public int GetMoveX()
    {
        return moveX;
    }

    public int GetMoveY()
    {
        return moveY;
    }

    public int GetStartX()
    {
        return startX;
    }

    public int GetStartY()
    {
        return startY;
    }
}
