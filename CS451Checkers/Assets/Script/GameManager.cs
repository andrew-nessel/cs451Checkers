using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void HostButton()
    {
        try
        {
            // Create Server on host
            Server s = Instantiate(serverPrefab).GetComponent<Server>();
            s.Init();

            // Create Client on host
            Client c = Instantiate(clientPrefab).GetComponent<Client>();
            c.ConnectToServer("127.0.0.1", 1234);
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }

        Debug.Log("Host");
    }

    public void ConnectToServerButton()
    {
        string hostAddress = GameObject.Find("IPInput").GetComponent<InputField>().text;
        
        if (string.IsNullOrEmpty(hostAddress))
            hostAddress = "127.0.0.1";

        try
        {

            Client c = Instantiate(clientPrefab).GetComponent<Client>();
            c.ConnectToServer(hostAddress, 1234);
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void BackButton()
    {

    }
	
}
