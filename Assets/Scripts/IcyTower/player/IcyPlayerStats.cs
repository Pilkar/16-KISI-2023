using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcyPlayerStats : MonoBehaviour
{
    [SerializeField] GameObject EndScreen;
    [SerializeField] GameObject ScorePlayerSingle;

    public void GameOver(int points)
    {
        Debug.Log("Points: "+points);
        EndScreen.SetActive(true);
        ScorePlayerSingle.SetActive(true);
        ScorePlayerSingle.GetComponentInChildren<TMPro.TMP_Text>().text = points.ToString();
        GameObject.Find("PreSceneAll").SetActive(false);
    }
}
