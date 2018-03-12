using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class checkerBoard : MonoBehaviour {

    public static checkerBoard Instance { get; set; }

    // Use this for initialization
    public GameObject whitePiece;
    public GameObject blackPiece;
    public GameObject whiteKing;
    public GameObject blackKing;
    private int selectedPiece;
    public GameObject[] pieces = new GameObject[24];
    private GameLogic gameLogic;
    private int currentTurn;
    private int playerNumber;

    private Vector2 mouseOver;
    private Vector2 startDrag;
    private Vector2 endDrag;

    private Client client;

    public Text turnText;

    void Start () {

        gameLogic = GetComponent<GameLogic>();
        generateBoard();
        Instance = this;
        client = FindObjectOfType<Client>();

        currentTurn = 1;
        turnText.text = "Player 1 Turn" + "\n" + "(White)";

        if (client.isHost)
            playerNumber = 1;
        else
            playerNumber = 2;
        
        //playerNumber = 1;
    }

    // Update is called once per frame
    void Update () {

        UpdateMouseOver();
                       
        if(playerNumber == currentTurn)
        {
            int x = (int)mouseOver.x;
            int y = (int)mouseOver.y;

            if(Input.GetMouseButtonDown(0))
            {
                SelectPiece(x, y);
            }

            if(Input.GetMouseButtonUp(0))
            {
                TryMove((int)startDrag.x, (int)startDrag.y, x, y);
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
        
        Piece pi = gameLogic.selectPiece(x, y);
        //Debug.Log(gameLogic.getBoard()[0][0].getID());

        if (pi != null)
        {
            selectedPiece = pi.getID();
            startDrag = mouseOver;
            Debug.Log(selectedPiece);
        }
        else
        {
            selectedPiece = -1;
        }
       
        
    }

    private void TryMove(int x1, int y1, int x2, int y2)
    {
        if(selectedPiece == -1)
        {
            return;
        }

        // Multiplayer Support
        startDrag = new Vector2(x1, y1);
        endDrag = new Vector2(x2, y2);
        Debug.Log(endDrag.x+" "+endDrag.y);
        Piece spiece = gameLogic.getPiece(selectedPiece);

        if((!spiece.isWhite() && playerNumber==1) || (spiece.isWhite() && playerNumber == 2))
        {
            return;
        }

        Move move = new Move(spiece, x2, y2);
        if (gameLogic.validMove(move))
        {
            gameLogic.makeMove(move);
            
            string msg = "C:Move|" + client.clientName + "|";
            msg += spiece.getID() + "|";
            msg += endDrag.x.ToString() + "|";
            msg += endDrag.y.ToString();
            msg += "|Server Receieved";

            client.Send(msg);
        }
    }

    public void changeTurn()
    {
        if (currentTurn == 1)
        {
            currentTurn = 2;
            turnText.text = "Player 2 Turn" + "\n" + "(Black)";
        }
        else
        {
            currentTurn = 1;
            turnText.text = "Player 1 Turn" + "\n" + "(White)";
        }
    }

    public void declareWinner(int winningPlayer)
    {
        currentTurn = -1;
        if (winningPlayer == 1)
        {
            turnText.text = "Player 1 Wins" + "\n" + "(White)";
        }
        else
        {
            turnText.text = "Player 2 Wins" + "\n" + "(Black)";
        }
    }



    /* 
    * Essentially the same as TryMove, but called to synchronize the board.
    * Moves a piece without sending a message to the server, since this function
    * is called when receiving a message from the server.
    */
    public void UpdateAfterOpponentMove(int pieceId, int x, int y)
    {
        Piece spiece = gameLogic.getPiece(pieceId);
        Move move = new Move(spiece, x, y);
        if (gameLogic.validMove(move))
        {
            gameLogic.makeMove(move);
        }
    }

    public void updateBoard(Piece[] newPieces)
    {
        foreach (Piece newpiece in newPieces)
        {
            GameObject go = pieces[newpiece.getID()];
            if (!newpiece.isCaptured())
            {
                int newX = newpiece.getX();
                int newY = newpiece.getY();
                MovePiece(go, newX, newY);
            }
            else
            {
                MovePiece(go, 12, 12);
            }
            if ((newpiece.isKing() == true) && ((go.name != "whiteKing(Clone)") && (go.name != "blackKing(Clone)")))
            {
                if (newpiece.isWhite() == true)
                {
                    MovePiece(go, 12, 12);
                    int newX = newpiece.getX();
                    int newY = newpiece.getY();
                    go = Instantiate(whiteKing, (new Vector3(newX * 10 + 5, 0.2f, newY * 10 + 5)), transform.rotation);
                    pieces[newpiece.getID()] = go;
                    Debug.Log(go.GetType());

                    //MovePiece(go, newX, newY);

                }
                if (newpiece.isWhite() == false)
                {
                    MovePiece(go, 12, 12);
                    int newX = newpiece.getX();
                    int newY = newpiece.getY();
                    go = Instantiate(blackKing, (new Vector3(newX * 10 + 5, 0.2f, newY * 10 + 5)), transform.rotation);
                    pieces[newpiece.getID()] = go;
                }
            }
        }
    }
    

    private void generateBoard()
    {

        Vector3 piecePos;
        int z = 0;

        // generate white pieces
        for (int y = 0; y < 3; y++)
        {
            if (y % 2 == 0)
            {
                for (int x = 0; x < 8; x += 2)
                {
                    piecePos = new Vector3(x * 10 + 5, 0.2f, y * 10 + 5);
                    generatePiece(z, piecePos);
                    z++;
                }

            }
            else
            {
                for (int x = 1; x < 8; x += 2)
                {
                    piecePos = new Vector3(x * 10 + 5, 0.2f, y * 10 + 5);
                    generatePiece(z, piecePos);
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
                    piecePos = new Vector3(x * 10 + 5, 0.2f, y * 10 + 5);
                    generatePiece(z, piecePos);
                    z++;
                }

            }
            else
            {
                for (int x = 1; x < 8; x += 2)
                {
                    piecePos = new Vector3(x * 10 + 5, 0.2f, y * 10 + 5);
                    generatePiece(z, piecePos);
                    z++;
                }
            }
        }
    }

    private void generatePiece(int ID, Vector3 piecePos)
    {
        bool isPieceWhite = (ID > 11) ? false : true;
        GameObject go = Instantiate((isPieceWhite) ? whitePiece : blackPiece, piecePos, transform.rotation);
        pieces[ID] = go;
    }

     private void MovePiece(GameObject p, int x, int y)
    {
        p.transform.position = new Vector3(x * 10 + 5, 0.2f, y * 10 + 5);

    }
   
}
