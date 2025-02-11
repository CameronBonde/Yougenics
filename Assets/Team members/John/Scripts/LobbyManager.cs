using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;
using Unity.Collections;
using System;

public class LobbyManager : NetworkBehaviour
{
    public Canvas lobby;
    public TMP_Text clientUI;

    public string clientName;

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartButtons();
        }
        else
        {
            StatusLabels();
        }

        GUILayout.EndArea();
    }

    static void StartButtons()
    {
        if (GUILayout.Button("Host"))
        {
            NetworkManager.Singleton.StartHost();
        }

        if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
        if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
    }

    static void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ?
            "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: " +
            NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientJoin;
        //NetworkManager.Singleton.OnClientDisconnectCallback += OnClientLeave;

        //ClientName.OnValueChanged += UpdateLobbyUI;
    }

    /*
    private void OnClientLeave(ulong obj)
    {
        if(NetworkManager.Singleton.IsServer)
            HandleClientNames();
    }
    */

    public void OnClientJoin(ulong clientID)
    {
        if(IsOwner)
        {
            HandleClientNames();
        }
       
    }

    [ServerRpc]
    void SubmitLobbyRequestServerRpc()
    {
        if (NetworkManager.Singleton.IsServer)
            HandleClientNames();
    }

    void HandleClientNames()
    {
        //ClientName.Value = "";
        clientName = "";

        foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
        {
            //ClientName.Value += client.PlayerObject.GetComponent<ClientInfo>().name;
            clientName += client.PlayerObject.GetComponent<ClientInfo>().name;
        }

        UpdateLobbyClientRPC(clientName);
    }

    [ClientRpc]
    public void UpdateLobbyClientRPC(string name)
    {
        clientUI.text = name;
    }

    public void UpdateLobbyUI(FixedString512Bytes previousValue, FixedString512Bytes newValue)
    {
        clientUI.text = newValue.ToString();
    }
}
