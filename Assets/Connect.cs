using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Connect : MonoBehaviour
{

    public Button b1;
    public Button b2;
    public Button b3;

    void Awake()
    {
        b1.onClick.AddListener(()=>{
            NetworkManager.Singleton.StartHost();
        });
        b2.onClick.AddListener(()=>{
            NetworkManager.Singleton.StartClient();
        });
        b3.onClick.AddListener(()=>{
            NetworkManager.Singleton.StartServer();
        });
    }
}
