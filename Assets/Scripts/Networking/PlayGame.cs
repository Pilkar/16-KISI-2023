using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayGame : NetworkBehaviour
{
    [SerializeField] GameObject PreSceneAll;
    [SerializeField] GameObject NetUIAll;
    [SerializeField] GameObject PlayerStats1;
    [SerializeField] GameObject PlayerStats2;
    
    public void StartGame()
    {
        PreSceneAll.SetActive(true);
        PlayerStats1.SetActive(true);

        if (NetworkManager.ConnectedClients.Count > 1)
        {
            PlayerStats2.SetActive(true);
        }
        PlayClientRpc(NetworkManager.ConnectedClients.Count);
        NetUIAll.SetActive(false);
    }
    
    [ClientRpc]
    void PlayClientRpc(int ClientsCount)
    {
        PreSceneAll.SetActive(true);
        PlayerStats1.SetActive(true);
        GameObject[] players = GameObject.FindGameObjectsWithTag("PlayerSpawner");
        foreach (var player in players)
        {
            player.GetComponent<PlayerBoxManager>().StopCoroutines();
        }
        if (ClientsCount > 1)
        {
            PlayerStats2.SetActive(true);
        }
        GameObject.Find("ScoreUI").GetComponent<PlayerStats>().UpdateUIStat((int)GameObject.Find("Player").GetComponent<Damageble>().Health, GameObject.Find("Player").GetComponent<player_controller>().Score);
        NetUIAll.SetActive(false);
    }
}
