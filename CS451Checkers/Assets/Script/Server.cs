using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server : MonoBehaviour
{
    public int Port = 1234;

    private List<ServerClient> _clients;
    private List<ServerClient> _disconnectList;

    private TcpListener _server;
    private bool _serverStarted;

    // Called when creating server
    public void Init(string ip)
    {
        DontDestroyOnLoad(gameObject);
        _clients = new List<ServerClient>();
        _disconnectList = new List<ServerClient>();

        try
        {
            IPAddress ipAddress;
            if (IPAddress.TryParse(ip, out ipAddress))
            {
                Debug.Log("!");
                _server = new TcpListener(ipAddress, Port);
            }
            else
            {
                Debug.Log("@");
                // Failsafe: Accept any IP Address as long as the port matches
                _server = new TcpListener(IPAddress.Any, Port);
            }


            _server.Start();

            _serverStarted = true;
            StartListening();
        }
        catch (Exception e)
        {
            Debug.Log("Socket error: " + e.Message);
        }
    }

    private void Update()
    {
        if (!_serverStarted)
            return;

        foreach (ServerClient c in _clients)
        {
            // Is the client still connected?
            if (!IsConnected(c.Tcp))
            {
                c.Tcp.Close();
                _disconnectList.Add(c);
              //  continue;
            }
            else
            {
                NetworkStream s = c.Tcp.GetStream();
                if (s.DataAvailable)
                {
                    StreamReader reader = new StreamReader(s, true);
                    string data = reader.ReadLine();

                    if (data != null)
                        OnIncomingData(c, data);
                }
            }
        }

        for (int i = _disconnectList.Count - 1; i >= 0; i--)
        {
            // Tell our player somebody has disconnected
            _clients.Remove(_disconnectList[i]);
            _disconnectList.RemoveAt(i);
        }
    }

    private void StartListening()
    {
        _server.BeginAcceptTcpClient(AcceptTcpClient, _server);
    }

    private void AcceptTcpClient(IAsyncResult ar)
    {
        TcpListener listener = (TcpListener)ar.AsyncState;

        string allUsers = "";
        foreach (ServerClient sclient in _clients)
        {
            allUsers += sclient.ClientName + '|';
        }

        ServerClient sc = new ServerClient(listener.EndAcceptTcpClient(ar));
        _clients.Add(sc);

        if (_clients.Count < 2)
            StartListening();

        Broadcast("S:Connection|" + allUsers, _clients[_clients.Count - 1]);
    }

    private bool IsConnected(TcpClient c)
    {
        try
        {
            if (c != null && c.Client != null && c.Client.Connected)
            {
                if (c.Client.Poll(0, SelectMode.SelectRead))
                    return (c.Client.Receive(new byte[1], SocketFlags.Peek) != 0);

                return true;
            }
            else
                return false;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }
    }

    // Server send
    private void Broadcast(string data, List<ServerClient> cl)
    {
        foreach (ServerClient sc in cl)
        {
            try
            {
                StreamWriter writer = new StreamWriter(sc.Tcp.GetStream());
                writer.WriteLine(data);
                writer.Flush();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
    }

    private void Broadcast(string data, ServerClient c)
    {
        List<ServerClient> sc = new List<ServerClient> { c };
        Broadcast(data, sc);
    }

    // Server read
    private void OnIncomingData(ServerClient c, string data)
    {
        Debug.Log(data);
        string[] aData = data.Split('|');
        switch (aData[0])
        {
            case "C:Player":
                c.ClientName = aData[1];
                c.IsHost = (aData[2] == "(Host)");
                Broadcast("SCNN|" + c.ClientName + " " + c.IsHost, _clients);
                break;
            case "C:Move":
                // aData[1] contains the Player Name of the player who made the move
                // Broadcast the message only to the player who didn't make the move, so that the same move isn't
                // repeated by the initial player
                Broadcast("S:Move|" + aData[2] + "|" + aData[3] + "|" + aData[4] + "|Client Receieved", _clients.Find(cl => cl.ClientName != aData[1]));
                break;
        }
    }
}


// Definition of client
public class ServerClient
{
    public string ClientName;
    public TcpClient Tcp;
    public bool IsHost;

    public ServerClient(TcpClient tcp)
    {
        this.Tcp = tcp;
    }
}