using System;
using System.Collections;
using System.Collections.Generic;

using WebSocketSharp;
using Newtonsoft.Json;
using UnityEngine.UI;
using UnityEngine;
using Unity.Mathematics;

public class P_NetworkManager : MonoBehaviour
{
    [Header("Game parameters")]
    [SerializeField] private GameObject clientPlayer;
    [SerializeField] private GameObject otherClientPlayer;
    public List<G_Client> clients = new List<G_Client>();
    public bool loadedServer = false;
    
    [Header("Network parameters")]
    private WebSocket WS;
    public string SERVER_HOST { get; set; }
    public string CLIENT_NAME { get; set; }

    private List<Dictionary<string, string>> IncomingBuffer = new List<Dictionary<string, string>>();
    private float tickRate = 0.024f;
    private float timePassed = 0f;

    public void ConnectToHost()
    {
        print("Connecting to host: " + SERVER_HOST);
        WS = new WebSocket(SERVER_HOST);

        WS.OnOpen += OnJoin_ServerConfiguration;
        WS.OnMessage += OnIncoming;
        WS.OnError += (sender, e) => { Debug.LogError(e.Exception); };
        WS.OnClose += (sender, e) => { print("Connection closed"); };
        WS.ConnectAsync();

        P_RequestHandler.WS = WS;
        P_RequestHandler.NP = this;
    }

    private void OnJoin_ServerConfiguration(object sender, EventArgs e)
    {
        print("Connection established");
        P_RequestHandler.cRequestAUTH(CLIENT_NAME);
    }

    private void OnIncoming(object sender, MessageEventArgs e)
    {
        IncomingBuffer.Add(P_RequestHandler.parseIncoming(e.Data));
    }

    public void SpawnClientPlayer(Vector3 _pos, int _id)
    {
        GameObject newClientObj = Instantiate(clientPlayer, _pos, quaternion.identity);
        G_Client newClient = newClientObj.GetComponent<G_Client>();
        newClient._spawnPos = _pos;
        newClient.clientName = CLIENT_NAME;
        newClient.clientID = _id;

        clients.Add(newClient);
    }

    public void SpawnClientOther(Vector3 _pos, int _id, string _name)
    {
        GameObject newClientObj = Instantiate(otherClientPlayer, _pos, quaternion.identity);
        G_Client newClient = newClientObj.GetComponent<G_Client>();
        newClient._spawnPos = _pos;
        newClient.clientName = _name;
        newClient.clientID = _id;

        clients.Add(newClient);
    }

    public void UpdateClientOtherPosRot(Vector3 _pos, Vector3 _rot, int _id)
    {
        //print("UPDATING POSITION");
        int ind = P_RequestHandler.clientIndexByID(_id);
        if(ind != -1)
        {
            clients[ind].transform.position = _pos;
            clients[ind].transform.rotation = Quaternion.Euler(_rot);
        }
    }

    public void SendChatPublic(string _c)
    {
        P_RequestHandler.cRequestSENDCHAT(_c);
    }

    void Update()
    {
        if(WS != null && WS.IsAlive)
        {
            if(IncomingBuffer.Count > 0)
            {
                P_RequestHandler.ReqRoute(IncomingBuffer[0]);
                IncomingBuffer.RemoveAt(0);
            }
            timePassed += Time.deltaTime;
        }
    }

    void tick()
    {
        print(clients.Count);
        if(clients.Count > 0)
        {
            P_RequestHandler.cRequestMOVING(clients[0].transform.position, clients[0].transform.rotation.eulerAngles);
        }
    }

    void LateUpdate()
    {
        if(timePassed > tickRate)
        {
            tick();
            timePassed = 0;
        }
    }

    public void printERROR(string _a)
    {
        print(_a);
    }

    void OnDestroy()
    {
        if(WS != null)
        {
            WS.CloseAsync();
        }
    }

}
