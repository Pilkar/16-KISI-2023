using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerStats : NetworkBehaviour
{
    [SerializeField] GameObject pointsIndicator;
    [SerializeField] GameObject healthIndicator;
    [SerializeField] GameObject pointsIndicator2;
    [SerializeField] GameObject healthIndicator2;

    [SerializeField] GameObject EndScreen;
    [SerializeField] GameObject ScorePlayer1;
    [SerializeField] GameObject ScorePlayer2;
    [SerializeField] GameObject ScorePlayerSingle;

    public void EndGame(bool isMulti, int points)
    {
        EndScreen.SetActive(true);
        if (isMulti)
        {
            EndGameServerRpc();
        }
        else
        {
            ScorePlayerSingle.SetActive(true);
            ScorePlayer1.SetActive(false);
            ScorePlayer2.SetActive(false);
            ScorePlayerSingle.GetComponentInChildren<TMPro.TMP_Text>().text = points.ToString();
            GameObject.Find("PreSceneAll").SetActive(false);
        }
    }

    public void UpdateUIStat(int health,int points)
    {
        if (IsHost)
        {

            UpdateUIServerRpc(true, health, points);
        }
        else if (IsClient)
        {

            UpdateUIServerRpc(false, health, points);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void EndGameServerRpc()
    {
        EndGameClientRpc();
    }

    [ClientRpc]
    void EndGameClientRpc()
    {
        EndScreen.SetActive(true);
        ScorePlayerSingle.SetActive(false);
        ScorePlayer1.SetActive(true);
        ScorePlayer2.SetActive(true);
        GameObject.Find("PreSceneAll").SetActive(false);
    }
    [ServerRpc(RequireOwnership =false)]
    void UpdateUIServerRpc(bool isPlayerOne, int health, int points)
    {
        UpdateUIClientRpc(isPlayerOne, health, points);
    }

    [ClientRpc]
    void UpdateUIClientRpc(bool isPlayerOne, int health, int points)
    {
        if (isPlayerOne)
        {
            pointsIndicator.GetComponent<TMPro.TMP_Text>().text = points.ToString();
            healthIndicator.GetComponent<TMPro.TMP_Text>().text = health.ToString();
            ScorePlayer1.GetComponentInChildren<TMPro.TMP_Text>().text = points.ToString();
        }
        else
        {
            pointsIndicator2.GetComponent<TMPro.TMP_Text>().text = points.ToString();
            healthIndicator2.GetComponent<TMPro.TMP_Text>().text = health.ToString();
            ScorePlayer2.GetComponentInChildren<TMPro.TMP_Text>().text = points.ToString();
        }
    }
    public void BackToMainMenu()
    {
        NetworkManager.Singleton.DisconnectClient(OwnerClientId);
        SceneManager.LoadScene("Main_menu");
    }
}
