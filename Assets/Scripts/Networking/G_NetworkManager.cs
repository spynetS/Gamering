using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using WebSocketSharp;
using UnityEngine;
using Newtonsoft.Json;
using System.Globalization;
using UnityEngine.Timeline;

public class G_NetworkManager : MonoBehaviour
{
    [SerializeField] private string SERVER_IP;
    [SerializeField] private int    SERVER_PORT;
    [SerializeField] private string CLIENT_NAME;
    private WebSocket _WS;

    [SerializeField] private GameObject clientPrefab;
    [SerializeField] private GameObject connectedPrefab;
    [SerializeField] private List<G_Client> connectedClients = new List<G_Client>();
    [SerializeField] private List<Dictionary<string, string>> commandStack = new List<Dictionary<string, string>>();

    void Start()
    {
        ConnectToServer();
        _WS.Send("Joining");
    }

    void WS_Open(object sender, EventArgs e)
    {
        print("Connection established");
        /* string authCMD = "{\"cmd\": \"AUTH\", \"name\": \"" + CLIENT_NAME +"\"}";
        _WS.Send(authCMD); */
    }

    void ConnectToServer()
    {
        string _urlStr = $"ws://" + SERVER_IP + ":" + SERVER_PORT;
        _WS = new WebSocket(_urlStr);

        _WS.OnOpen += WS_Open;
        _WS.OnClose += (sender ,e ) => { print("CLOSING CONNECTION"); };
        _WS.OnMessage += IncomingServerMsg;
        _WS.OnError += (sender, e) => { print(e.Message); };
        _WS.ConnectAsync();
    }

    [SerializeField] private int MY_ID = -1;

    void IncomingServerMsg(object sender, MessageEventArgs e)
    {
        try
        {
            //Debug.Log("Message received (" + ((WebSocket)sender).Url + "): " + e.Data);
        }catch(Exception ex)
        {
            Debug.LogError("ERROR: " + ex.Message);
        }

        try{
            if(e.Data != null)
            {
                Dictionary<string, string> msg = JsonConvert.DeserializeObject<Dictionary<string,string>>(e.Data);
                commandStack.Add(msg);
            }
        }
        catch(Exception ex)
        {
            Debug.LogError("ERROR: " + ex.Message);
        }
    }

    void executeCommand(Dictionary<string, string> msg)
    {
        switch(msg["cmd"])
        {
            case "join":
                MY_ID = Convert.ToInt32(msg["ID"]);
                string[] _spawn = msg["spawn"].Split(",");
                Vector3 _v_spawn = new Vector3(Convert.ToInt32(_spawn[0]), Convert.ToInt32(_spawn[1]), Convert.ToInt32(_spawn[2]));
                
                GameObject g = Instantiate(clientPrefab, _v_spawn, Quaternion.identity);
                G_Client _g = g.GetComponent<G_Client>();
                _g._spawnPos = _v_spawn;
                _g.clientName = CLIENT_NAME;
                _g.clientID = MY_ID;

                connectedClients.Add(_g);
                break;
            case "new":
                int _id = Convert.ToInt32(msg["ID"]);
                string[] _Nspawn = msg["spawn"].Split(",");
                Vector3 _v_Nspawn = new Vector3(Convert.ToInt32(_Nspawn[0]), Convert.ToInt32(_Nspawn[1]), Convert.ToInt32(_Nspawn[2]));
                
                GameObject ng = Instantiate(connectedPrefab, _v_Nspawn, Quaternion.identity);
                G_Client _ng = ng.GetComponent<G_Client>();
                _ng._spawnPos = _v_Nspawn;
                _ng.clientName = CLIENT_NAME;
                _ng.clientID = _id;

                connectedClients.Add(_ng);
                break;
            case "move":
                int _nid = Convert.ToInt32(msg["ID"]);
                print(msg["pos"]);
                string[] _pos = msg["pos"].Split(",");
                Vector3 _v_pos = new Vector3(
                    (float)Convert.ToDecimal(_pos[0], CultureInfo.InvariantCulture.NumberFormat),
                    (float)Convert.ToDecimal(_pos[1], CultureInfo.InvariantCulture.NumberFormat),
                    (float)Convert.ToDecimal(_pos[2], CultureInfo.InvariantCulture.NumberFormat)
                );

                foreach (var _client in connectedClients)
                {
                    print("MOVING THIS DUDE");
                    if(_client.clientID == _nid)
                    {
                        _client.gameObject.transform.position = _v_pos;
                    }
                }
                break;
        }
    }

    float tickRate = 0.016f;
    float timePassed = 0f;
    void Update()
    {
        if(commandStack.Count > 0)
        {
            var msg = commandStack[0];
            executeCommand(msg);
            commandStack.RemoveAt(0);
        }

        timePassed += Time.deltaTime;
    }

    void LateUpdate()
    {
        if(timePassed > tickRate)
        {
            tick();
            timePassed = 0;
        }
    }

    void tick()
    {
        if(connectedClients.Count > 0)
        {
            string my_pos_str = connectedClients[0].gameObject.transform.position.ToString();
            string posMsg = "{\"cmd\": \"move\", \"ID\": \"" + MY_ID.ToString() + "\", \"pos\": \"" + my_pos_str + "\"}";
            //print(posMsg);
            _WS.Send(posMsg);
        }
    }

    void OnDestroy()
    {
        if(_WS != null)
        {
            _WS.CloseAsync();
        }
    }

}
