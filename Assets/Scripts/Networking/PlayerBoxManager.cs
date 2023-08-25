using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Collections;
using System.Collections;

public class PlayerBoxManager : NetworkBehaviour
{
    GameObject PlayerBox1;
    GameObject PlayerBox2;
    GameObject ScoreUI;
    
    NetworkVariable<FixedString32Bytes> Name = new NetworkVariable<FixedString32Bytes>();

    void Start()
    {
        if (IsLocalPlayer)
        {
            //Debug.Log(GameObject.Find("Player1Box"));
            PlayerBox1 = GameObject.Find("Player1Box");
            //Debug.Log(GameObject.Find("Player2Box"));
            PlayerBox2 = GameObject.Find("Player2Box");
            ScoreUI = GameObject.Find("ScoreUI");
        }
        
        if (IsHost && IsOwner)
        {
            PlayerBox2.SetActive(false);
        }

        if (IsLocalPlayer)
        {
            ChangeNameServerRpc(GameObject.Find("NetworkManager").GetComponent<TransferInput>().GetName());
        }
    }

    [ServerRpc]
    void ChangeNameServerRpc(string name)
    {
        Name.Value = name;
        updateUIClientRPC();
    }

    [ClientRpc]
    void updateUIClientRPC()
    {
        StartCoroutine(WaitAndUpdate());

        IEnumerator WaitAndUpdate()
        {
            bool endLoop = true;
            while (endLoop)
            {
                GameObject[] players = GameObject.FindGameObjectsWithTag("PlayerSpawner");
                int i = 0;
                foreach (var player in players)
                {
                    if (i == 0)
                    {
                        PlayerBox1.SetActive(true);
                        ScoreUI.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TMP_Text>().text = player.GetComponent<PlayerBoxManager>().Name.Value.ToString();
                        PlayerBox1.GetComponentInChildren<TMPro.TMP_Text>().text = player.GetComponent<PlayerBoxManager>().Name.Value.ToString();
                    }
                    else if (i == 1)
                    {
                        if (player.GetComponent<PlayerBoxManager>().Name.Value.ToString() != "")
                        {
                            PlayerBox2.SetActive(true);
                            ScoreUI.transform.GetChild(1).GetChild(0).GetComponent<TMPro.TMP_Text>().text = player.GetComponent<PlayerBoxManager>().Name.Value.ToString();
                            PlayerBox2.GetComponentInChildren<TMPro.TMP_Text>().text = player.GetComponent<PlayerBoxManager>().Name.Value.ToString();
                            endLoop = false;
                        }
                    }
                    i++;
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public void StopCoroutines()
    {
        if(IsLocalPlayer)
            StopAllCoroutines();
    }
    
}
