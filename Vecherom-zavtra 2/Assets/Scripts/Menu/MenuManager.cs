using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string SceneName;

	public GameObject menuPanel;
	public GameObject optionsPanel;
	public GameObject loginPanel;

	void Start()
	{
		string path = Application.persistentDataPath + "/usr.vz";
		if (File.Exists(path))
		{
			// If the user is logged in show the menu
			menuPanel.SetActive(true);
			optionsPanel.SetActive(false);
			loginPanel.SetActive(false);
		}
		else
		{
			// If the user isn't logged in show the login panel
			menuPanel.SetActive(false);
			optionsPanel.SetActive(false);
			loginPanel.SetActive(true);
		}
	}

    public void Play()
	{
        SceneManager.LoadScene(SceneName);
	}

	public void LogOut()
	{
		Utils.DeleteUserInfo();
		SceneManager.LoadScene(0);
	}

    public void QuitGame()
	{
		Debug.LogWarning("APP CLOSED!");
		Application.Quit();
	}
}
