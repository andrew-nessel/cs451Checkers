public class Piece {

    protected int Id;
    protected bool White;
    protected bool King;
    protected int X;
    protected int Y;
    protected bool Captured;

    public Piece(int id, int y, int x, bool isWhite)
    {
        this.Id = id;
        this.White = isWhite;
        this.King = false;
        this.Captured = false;
        this.X = x;
        this.Y = y;
    }

    public int GetX()
    {
        return X;
    }

    public void UpdateX(int x)
    {
        this.X = x;
    }

    public int GetY()
    {
        return Y;
    }

    public void UpdateY(int y)
    {
        this.Y = y;
    }

    public int GetId()
    {
        return Id;
    }

    public bool IsWhite()
    {
        return White;
    }

    public bool IsKing()
    {
        return King;
    }

    public void KingMe()
    {
        King = true;
    }

    public bool IsCaptured()
    {
        return Captured;
    }

    public void Capture()
    {
        Captured = true;
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
