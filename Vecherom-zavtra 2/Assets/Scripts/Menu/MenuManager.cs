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
		menuPanel.SetActive(true);
		optionsPanel.SetActive(false);
		loginPanel.SetActive(false);
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
