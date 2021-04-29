using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string SceneName;

    public void Play()
	{
        SceneManager.LoadScene(SceneName);
	}

    public void QuitGame()
	{
		Debug.LogWarning("APP CLOSED!");
		Application.Quit();
	}
}
