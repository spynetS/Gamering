using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class S_MainUiHandler : MonoBehaviour
{
    public List<GameObject> panels = new List<GameObject>();

    [SerializeField] private int _activePanelIndex = 0;
    public int activePanelIndex {
        get{ return _activePanelIndex; }
        set{
            _activePanelIndex = value;
            toggleOffPanels();
            panels[value].SetActive(true);
        }
    }

    void toggleOffPanels()
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
    }

    public string _HostInput;
    public string HostInput {
        get{ return _HostInput; }
        set{ _HostInput = value; _NetworkManager.SERVER_HOST = $"ws://{value}";}
    }

    public string _ClientNameInput;
    public string ClientNameInput {
        get{ return _ClientNameInput; }
        set{ _ClientNameInput = value; _NetworkManager.CLIENT_NAME = value;}
    }

    public P_NetworkManager _NetworkManager;

    [Header("HUD")]
    public GameObject ChatTextObject;
    public GameObject ChatBoxPanel;
    public GameObject ChatContentBox;
    public GameObject ChatInputBox;
    private bool ChatBoxInFocus = false;

    void Start()
    {
        P_RequestHandler.SMUIH = this;
        activePanelIndex = 0;
    }

    void Update()
    {
        if(ChatBoxInFocus && Input.GetKeyDown(KeyCode.Return))
        {
            string _ToChat = ChatInputBox.GetComponent<TMP_InputField>().text;
            if(_ToChat != "")
            {
                _NetworkManager.SendChatPublic(_ToChat);
                ChatBoxInFocus = false;
                ChatInputBox.GetComponent<TMP_InputField>().text = "";
                ChatBoxPanel.SetActive(false);
            }
        }

        if(activePanelIndex >= 2)
        {
            if(!ChatBoxInFocus && Input.GetKeyDown(KeyCode.T))
            {
                ChatBoxInFocus = true;
                ChatBoxPanel.SetActive(true);
                ChatInputBox.GetComponent<TMP_InputField>().ActivateInputField();
            }
        }
    }

    void LateUpdate()
    {
        if(_NetworkManager.loadedServer && activePanelIndex < 2)
        {
            activePanelIndex = 2;
        }
    }

    public void AddToChat(string _c)
    {
        GameObject _go = Instantiate(ChatTextObject, ChatContentBox.transform);
        _go.GetComponent<TextMeshProUGUI>().text = _c;
    }

    public void ListenKey_SendMessage()
    {
        ChatBoxInFocus = true;
    }
}
