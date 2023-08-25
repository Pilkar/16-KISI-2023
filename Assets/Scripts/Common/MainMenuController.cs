using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class MainMenuController : MonoBehaviour
{
    public void Game1()
	{
		SceneManager.LoadScene("BulletHell");
	}
	public void Game2()
	{
		SceneManager.LoadScene("IcyTower");
	}
	public void BackToMainMenu()
	{
		SceneManager.LoadScene("Main_menu");
	}
	public void Quit()
	{
		Application.Quit();
	}
}
