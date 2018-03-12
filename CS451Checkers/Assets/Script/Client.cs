using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;


public class Client : MonoBehaviour
{
    public string ClientName;
    public bool IsHost;

    // Keep track if the socket is ready
    private bool _socketReady;

    // Objects for connection and data 
    private TcpClient _socket;
    private NetworkStream _stream;
    private StreamWriter _writer;
    private StreamReader _reader;

    // Keep track of players connected
    // Server will broadcast a response to each client connected when a new
    // player connects
    private List<GameClient> _players;

    private void Start()
    {
        _players = new List<GameClient>();
        DontDestroyOnLoad(gameObject);
    }

    /*
     * Try to connect to the server
     * Sets networking objects
     * Returns true if new connection is established, false otherwise
     */
    public bool ConnectToServer(string host, int port)
    {
        // Don't do anything if already connected
        if (_socketReady)
            return false;

        try
        {
            _socket = new TcpClient(host, port);
            _stream = _socket.GetStream();
            _writer = new StreamWriter(_stream);
            _reader = new StreamReader(_stream);

            _socketReady = true;
        }
        catch (Exception e)
        {
            Debug.Log("Socket error " + e.Message);
        }

        return _socketReady;
    }

    private void Update()
    {
        if (_socketReady)
        {
            if (_stream.DataAvailable)
            {
                string data = _reader.ReadLine();
                if (data != null)
                    OnIncomingData(data);
            }
        }
    }


    // Send messages to the server
    public void Send(string data)
    {
        if (!_socketReady)
            return;

        _writer.WriteLine(data);
        _writer.Flush();
    }

    // Read messages from the server
    private void OnIncomingData(string data)
    {
        Debug.Log(data);
        string[] aData = data.Split('|');
        switch (aData[0])
        {
            // Player connected; add to local client players list
            case "S:Connection":
                for (int i = 1; i < aData.Length - 1; i++)
                {
                    UserConnected(aData[i]);
                }

                // Send to the server a message that this player joined 
                Send("C:Player|" + ClientName + "|" + ((IsHost) ? "(Host)" : "(Guest)"));
                break;
            case "SCNN":
                UserConnected(aData[1]);
                break;
            case "S:Move":
                checkerBoard.Instance.UpdateAfterOpponentMove(int.Parse(aData[1]), int.Parse(aData[2]), int.Parse(aData[3]));
                break;
        }

        Debug.Log(data);
    }

    private void UserConnected(string pname)
    {
        GameClient c = new GameClient()
        {
            Name = pname
        };

        _players.Add(c);

        if (_players.Count == 2)
            GameManager.Instance.StartGame();
    }

    private void OnApplicationQuit()
    {
        CloseSocket();
    }

    private void OnDisable()
    {
        CloseSocket();
    }

    private void CloseSocket()
    {
        if (!_socketReady)
            return;

        _writer.Close();
        _reader.Close();
        _socket.Close();
        _socketReady = false;
    }
}

public class GameClient
{
    public string Name;
    public bool IsHost;
}
