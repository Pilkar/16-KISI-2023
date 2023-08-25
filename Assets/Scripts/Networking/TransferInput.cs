using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class TransferInput : MonoBehaviour
{
    //statics for transfering variables between scenes
    static string ipconfig="";
    static bool isHost = true;
    static bool isMulti = false;
    static string PlayerName = "";
    [SerializeField] GameObject PreSceneAll;

    private void Start()
    {
        if (this.gameObject.name == "NetworkManager" && isMulti)
        {
            if (isHost)
            {
                GameObject.Find("PlayButton").SetActive(true);
                if (NetworkManager.Singleton.StartHost())
                {
                    Debug.Log("Host started...");
                }
                else
                {
                    Debug.Log("Host could not be started...");
                }
            }
            else
            {
                GameObject.Find("PlayButton").SetActive(false);
                if (this.gameObject.GetComponent<Unity.Netcode.Transports.UNET.UNetTransport>())
                {
                    this.gameObject.GetComponent<Unity.Netcode.Transports.UNET.UNetTransport>().ConnectAddress = ipconfig;
                }
                if (NetworkManager.Singleton.StartClient())
                {
                    Debug.Log("Client started...");
                }
                else
                {
                    Debug.Log("Client could not be started...");
                }
            }
        }
        else if(this.gameObject.name == "NetworkManager")
        {
            //turn on singleplayer
            Debug.Log(transform.Find("NetUIAll"));
            
            GameObject.Find("NetUIAll").gameObject.SetActive(false);
            PreSceneAll.SetActive(true);
        }
    }

    //functions for button eventHandler
    public void WriteIP()
    {
        if (this.gameObject.GetComponent<TMPro.TMP_InputField>())
        {
            if (this.gameObject.GetComponent<TMPro.TMP_InputField>().text=="")
                ipconfig ="127.0.0.1";
            else
                ipconfig = this.gameObject.GetComponent<TMPro.TMP_InputField>().text;
        }
    }

    public void WriteName()
    {
        if (GameObject.Find("InputName").GetComponent<TMPro.TMP_InputField>())
        {
            if (GameObject.Find("InputName").GetComponent<TMPro.TMP_InputField>().text == "")
                PlayerName = "Player";
            else
                PlayerName = GameObject.Find("InputName").GetComponent<TMPro.TMP_InputField>().text;
        }
    }

    public string GetName()
    {
        return PlayerName;
    }

    public void WriteHost(bool value)
    {
        isHost = value;
    }
    public void WriteMulti(bool value)
    {
        isMulti = value;
    }
    public bool ReadMulti()
    {
        return isMulti;
    }
}
