using UnityEngine;
using UnityEngine.UI;

public class checkerBoard : MonoBehaviour {

    public static checkerBoard Instance { get; set; }

    // Use this for initialization
    public GameObject WhitePiece;
    public GameObject BlackPiece;
    public GameObject WhiteKing;
    public GameObject BlackKing;
    private int _selectedPiece;
    public GameObject[] Pieces = new GameObject[24];
    private GameLogic _gameLogic;
    private int _currentTurn;
    private int _playerNumber;

    private Vector2 _mouseOver;
    private Vector2 _startDrag;
    private Vector2 _endDrag;

    private Client _client;

    public Text TurnText;

    void Start () {
        _currentTurn = 1;
        _gameLogic = GetComponent<GameLogic>();
        GenerateBoard();
        Instance = this;
        _client = FindObjectOfType<Client>();


        _playerNumber = (_client.IsHost) ? 1 : 2;
        TurnText.text = "Player 1 Turn" + "\n" + "(White)";
        
        //_playerNumber = 1;
    }

    // Update is called once per frame
    void Update () {

        UpdateMouseOver();
                       
        if(_playerNumber == _currentTurn)
        {
            int x = (int)_mouseOver.x;
            int y = (int)_mouseOver.y;

            if(Input.GetMouseButtonDown(0))
            {
                SelectPiece(x, y);
            }

            if(Input.GetMouseButtonUp(0))
            {
                TryMove((int)_startDrag.x, (int)_startDrag.y, x, y);
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
            _mouseOver.x = (int)(hit.point.x/10);
            _mouseOver.y = (int)(hit.point.z/10);
        }
        else
        {
            _mouseOver.x = -1;
            _mouseOver.y = -1;
        }
    }

    private void SelectPiece(int x, int y)
    {
        
        Piece pi = _gameLogic.SelectPiece(x, y);
        //Debug.Log(_gameLogic.GetBoard()[0][0].GetId());

        if (pi != null)
        {
            _selectedPiece = pi.GetId();
            _startDrag = _mouseOver;
            Debug.Log(_selectedPiece);
        }
        else
        {
            _selectedPiece = -1;
        }
       
        
    }

    private void TryMove(int x1, int y1, int x2, int y2)
    {
        if(_selectedPiece == -1)
        {
            return;
        }

        // Multiplayer Support
        _startDrag = new Vector2(x1, y1);
        _endDrag = new Vector2(x2, y2);
        Debug.Log(_endDrag.x+" "+_endDrag.y);
        Piece spiece = _gameLogic.GetPiece(_selectedPiece);

        if((!spiece.IsWhite() && _playerNumber==1) || (spiece.IsWhite() && _playerNumber == 2))
        {
            return;
        }

        Move move = new Move(spiece, x2, y2);
        if (_gameLogic.ValidMove(move))
        {
            _gameLogic.MakeMove(move);
            
            string msg = "C:Move|" + _client.ClientName + "|";
            msg += spiece.GetId() + "|";
            msg += _endDrag.x + "|";
            msg += _endDrag.y;
            msg += "|Server Receieved";

            _client.Send(msg);
        }
    }

    public void ChangeTurn()
    {
        if (_currentTurn == 1)
        {
            _currentTurn = 2;
            TurnText.text = "Player 2 Turn" + "\n" + "(Black)";
        }
        else
        {
            _currentTurn = 1;
            TurnText.text = "Player 1 Turn" + "\n" + "(White)";
        }
    }

    public void declareWinner(int winningPlayer)
    {
        _currentTurn = -1;
        if (winningPlayer == 1)
        {
            TurnText.text = "Player 1 Wins" + "\n" + "(White)";
        }
        else
        {
            TurnText.text = "Player 2 Wins" + "\n" + "(Black)";
        }
    }



    /* 
    * Essentially the same as TryMove, but called to synchronize the board.
    * Moves a piece without sending a message to the server, since this function
    * is called when receiving a message from the server.
    */
    public void UpdateAfterOpponentMove(int pieceId, int x, int y)
    {
        Piece spiece = _gameLogic.GetPiece(pieceId);
        Move move = new Move(spiece, x, y);
        if (_gameLogic.ValidMove(move))
        {
            _gameLogic.MakeMove(move);
        }
    }

    public void UpdateBoard(Piece[] newPieces)
    {
        foreach (Piece newpiece in newPieces)
        {
            GameObject go = Pieces[newpiece.GetId()];
            if (!newpiece.IsCaptured())
            {
                int newX = newpiece.GetX();
                int newY = newpiece.GetY();
                MovePiece(go, newX, newY);
            }
            else
            {
                MovePiece(go, 12, 12);
            }
            if ((newpiece.IsKing()) && ((go.name != "whiteKing(Clone)") && (go.name != "blackKing(Clone)")))
            {
                if (newpiece.IsWhite())
                {
                    MovePiece(go, 12, 12);
                    int newX = newpiece.GetX();
                    int newY = newpiece.GetY();
                    go = Instantiate(WhiteKing, (new Vector3(newX * 10 + 5, 0.2f, newY * 10 + 5)), transform.rotation);
                    Pieces[newpiece.GetId()] = go;
                    Debug.Log(go.GetType());

                    //MovePiece(go, newX, newY);

                }
                if (newpiece.IsWhite() == false)
                {
                    MovePiece(go, 12, 12);
                    int newX = newpiece.GetX();
                    int newY = newpiece.GetY();
                    go = Instantiate(BlackKing, (new Vector3(newX * 10 + 5, 0.2f, newY * 10 + 5)), transform.rotation);
                    Pieces[newpiece.GetId()] = go;
                }
            }
        }
    }
    

    private void GenerateBoard()
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
                    GeneratePiece(z, piecePos);
                    z++;
                }

            }
            else
            {
                for (int x = 1; x < 8; x += 2)
                {
                    piecePos = new Vector3(x * 10 + 5, 0.2f, y * 10 + 5);
                    GeneratePiece(z, piecePos);
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
                    GeneratePiece(z, piecePos);
                    z++;
                }

            }
            else
            {
                for (int x = 1; x < 8; x += 2)
                {
                    piecePos = new Vector3(x * 10 + 5, 0.2f, y * 10 + 5);
                    GeneratePiece(z, piecePos);
                    z++;
                }
            }
        }
    }

    private void GeneratePiece(int id, Vector3 piecePos)
    {
        bool isPieceWhite = !(id > 11);
        GameObject go = Instantiate((isPieceWhite) ? WhitePiece : BlackPiece, piecePos, transform.rotation);
        Pieces[id] = go;
    }

     private void MovePiece(GameObject p, int x, int y)
    {
        p.transform.position = new Vector3(x * 10 + 5, 0.2f, y * 10 + 5);

    }
   
}
