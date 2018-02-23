using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class checkerBoard : MonoBehaviour {

    // Use this for initialization
    public piece[,] pieces = new piece[8, 8];
    public GameObject whitePiece;
    public GameObject blackPiece;
    private Vector3 piecePos;
    private piece selectedPiece;

    private Vector2 mouseOver;
    private Vector2 startDrag;
    private Vector2 endDrag;


    void Start () {
        generateBoard();
	}
	
	// Update is called once per frame
	void Update () {
        UpdateMouseOver();
        Debug.Log(mouseOver);
        

        //if my turn
        {
            int x = (int)mouseOver.x;
            int y = (int)mouseOver.y;

            if(Input.GetMouseButtonDown(0))
            {
                SelectPiece(x, y);
            }

        }

    }

    private void UpdateMouseOver()
    {
        if (!Camera.main)
        {
            Debug.Log("Unable to find main camera");
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 110.0f, LayerMask.GetMask("Board")))
        {
            mouseOver.x = (int)(hit.point.x/10);
            mouseOver.y = (int)(hit.point.z/10);
        }
        else
        {
            mouseOver.x = -1;
            mouseOver.y = -1;
        }
    }

    private void SelectPiece(int x, int y)
    {
        if (x < 0 || x >= 8 || y < 0 || y >= 8)
            return;

        piece p = pieces[x, y];
        if (p != null)
        {
            selectedPiece = p;
            startDrag = mouseOver;
            Debug.Log(selectedPiece.name);
        }
       
        
    }

    private void generateBoard()
    {
        // generate white pieces
        for (int y = 0; y < 3; y++)
        {
            for(int x = 0; x < 8; x+=2)
            {
                if (y % 2 == 0)
                {
                    piecePos = new Vector3(x * 10 + 5, 0.2f, y * 10 + 5);
                    generatePiece(x, y);
                }
                else
                {
                    piecePos = new Vector3(x * 10 + 15, 0.2f, y * 10 + 5);
                    generatePiece(x, y);
                }
            }
        }

        //generate black pieces
        for (int y = 7; y > 4; y--)
        {
            for (int x = 0; x < 8; x += 2)
            {
                if (y % 2 == 0)
                {
                    piecePos = new Vector3(x * 10 + 5, 0.2f, y * 10 + 5);
                    generatePiece(x, y);
                }
                else
                {
                    piecePos = new Vector3(x * 10 + 15, 0.2f, y * 10 + 5);
                    generatePiece(x, y);
                }
            }
        }
    }

    private void generatePiece(int x,int y)
    {
        bool isPieceWhite = (y > 3) ? false : true;
        GameObject go = Instantiate((isPieceWhite) ? whitePiece : blackPiece, piecePos, transform.rotation);
        piece p = go.GetComponent<piece>();
        pieces[x, y] = p;
    }

   
}
