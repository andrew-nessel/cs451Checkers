using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;

public class Client : MonoBehaviour
{
    public string clientName;
    public bool isHost;

    // Keep track if the socket is ready
    private bool socketReady;

    // Objects for connection and data 
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    // Keep track of players connected
    // Server will broadcast a response to each client connected when a new
    // player connects
    private List<GameClient> players = new List<GameClient>();

    private void Start()
    {
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
        if (socketReady)
            return false;

        try
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);

            socketReady = true;
        }
        catch (Exception e)
        {
            Debug.Log("Socket error " + e.Message);
        }

        return socketReady;
    }

    private void Update()
    {
        if (socketReady)
        {
            if (stream.DataAvailable)
            {
                string data = reader.ReadLine();
                if (data != null)
                    OnIncomingData(data);
            }
        }
    }


    // Send messages to the server
    public void Send(string data)
    {
        if (!socketReady)
            return;

        writer.WriteLine(data);
        writer.Flush();
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
                    UserConnected(aData[i], false);
                }

                // Send to the server a message that this player joined 
                Send("C:Player|" + clientName + "|" + ((isHost) ? "(Host)" : "(Guest)"));
                break;
            case "SCNN":
                UserConnected(aData[1], false);
                break;
            case "S:Move":
                checkerBoard.Instance.UpdateAfterOpponentMove(int.Parse(aData[1]), int.Parse(aData[2]), int.Parse(aData[3]));
                break;
        }

        Debug.Log(data);
    }

    private void UserConnected(string pname, bool host)
    {
        GameClient c = new GameClient()
        {
            name = pname
        };

        players.Add(c);

        if (players.Count == 2)
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
        if (!socketReady)
            return;

        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;
    }
}

public class GameClient
{
    public string name;
    public bool isHost;
}
