using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

    protected int ID;
    protected bool white;
    protected bool king;
    protected int x;
    protected int y;

    public Piece(int ID, int y, int x, bool isWhite)
    {
        this.ID = ID;
        this.white = isWhite;
        this.king = false;
        this.x = x;
        this.y = y;
    }

    public int getX()
    {
        return x;
    }

    public void updateX(int x)
    {
        this.x = x;
    }

    public int getY()
    {
        return y;
    }

    public void updateY(int y)
    {
        this.y = y;
    }

    public int getID()
    {
        return ID;
    }

    public bool isWhite()
    {
        return white;
    }

    public bool isKing()
    {
        return king;
    }

    public void kingMe()
    {
        king = true;
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
