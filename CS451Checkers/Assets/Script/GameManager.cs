using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net;
using System.Net.Sockets;
using TMPro;

 

public class GameManager : MonoBehaviour {

    public static GameManager Instance { set; get; }

    public GameObject serverPrefab;
    public GameObject clientPrefab;

	void Start () {
        Instance = this;
        DontDestroyOnLoad(gameObject);
	}
	
	public void ConenctButton()
    {
        Debug.Log("Connect");
    }

    /*
     * Called when trying to Host a game
     * Establishes a server and a client for the Hosting player
     */
    public void HostButton()
    {
        try
        {
            string ip = GetLocalIPAddress();
            
            // Create Server on host
            Server s = Instantiate(serverPrefab).GetComponent<Server>();
            s.Init(ip);
            

            // Create Client on host
            Client c = Instantiate(clientPrefab).GetComponent<Client>();

            // Hosting Player is Player 1
            c.clientName = "Player 1";
            c.isHost = true;
            c.ConnectToServer(ip, 1234);
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    /*
     * Gets local IP Address and sets Host IP label to match
     * Returns IPv4 string representation 
     */
    private string GetLocalIPAddress()
    {
        // Default to local host IP
        string returnIp = "127.0.0.1";
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                returnIp = ip.ToString();
            }
        }

        // Update label
        GameObject.Find("EnterIP (2)").GetComponent<TextMeshProUGUI>().text = returnIp;

        return returnIp;
    }

    /*
     * Called when trying to Join a game
     * Defaults to local host IP address, but uses whatever IP is input
     */
    public void ConnectToServerButton()
    {
        string hostAddress = GameObject.Find("IPInput").GetComponent<InputField>().text;
        
        if (string.IsNullOrEmpty(hostAddress))
            hostAddress = "127.0.0.1";

        try
        {
            Client c = Instantiate(clientPrefab).GetComponent<Client>();

            // Joining Player is Player 2
            c.clientName = "Player 2";
            if(!c.ConnectToServer(hostAddress, 1234))
                GameObject.Find("ErrorMessage").GetComponent<TextMeshProUGUI>().text = "Error: Check the IP Address and try again";
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    /*
     * Called when going back from the Host menu
     * Upon entering the Host menu, Server and Client prefabs are created so
     * we need to destroy them to avoid having multiple server/clients every time
     * the Host menu is loaded
     */
    public void BackButton()
    {
        Server s = FindObjectOfType<Server>();
        Client c = FindObjectOfType<Client>();

        if (s != null)
            Destroy(s.gameObject);

        if (c != null)
            Destroy(c.gameObject);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("boardTest");
    }
}
