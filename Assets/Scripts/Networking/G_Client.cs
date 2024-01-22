using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class G_Client : MonoBehaviour
{
    public Vector3 _spawnPos;

    [SerializeField] public string clientName;
    [SerializeField] public int clientID;
    [SerializeField] public GameObject clientGameObject;

    [Header("None playable clients")]
    [SerializeField] private TextMeshProUGUI nameTagText;

    [SerializeField] public Transform MAIN_client;

    void Start()
    {
        if(nameTagText != null){
            nameTagText.text = clientName;
            MAIN_client = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
    }

    void LateUpdate()
    {
        if(nameTagText != null)
        {
            nameTagText.transform.rotation = Quaternion.LookRotation(transform.position - MAIN_client.transform.position);
        }
    }
}
