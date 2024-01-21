using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class Connect : NetworkBehaviour
{

    public Button b1;
    public Button b2;
    public Button b3;

    public static GameObject Spawn(GameObject gameObject, Vector3 position, Quaternion rotation)
    {
        GameObject spawned = Instantiate(gameObject,position,rotation);
        spawned.GetComponent<NetworkObject>().Spawn();
        return spawned;
        
    }
    public static GameObject Spawn(GameObject gameObject)
    {
        GameObject spawned = Instantiate(gameObject);
        spawned.GetComponent<NetworkObject>().Spawn();
        return spawned;
    }
    
    private void Start()
    {
        //NetworkManager.Singleton.StartHost();
        //Destroy(this.gameObject);


    }

    void Awake()
    {

        b1.onClick.AddListener(()=>{
            NetworkManager.Singleton.StartHost();
            Destroy(this.gameObject);
        });
        b2.onClick.AddListener(()=>{
            NetworkManager.Singleton.StartClient();
            Destroy(this.gameObject);
        });
        //b3.onClick.AddListener(()=>{
        //    NetworkManager.Singleton.StartServer();
        //    Destroy(this.gameObject);
        //});
    }
}
