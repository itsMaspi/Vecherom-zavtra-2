using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class PauseManager : MonoBehaviour
{
    public static PauseState pauseState;

    public static PauseManager instance;

    public GameObject panel;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
        pauseState = PauseState.NotPaused;
    }

    public void Toggle()
	{
        // This script will persist through scenes, 
        // so it may loose the reference to the pause panel.
        // If the scene has a PauseMenu it will find it, if it doesn't
        // it won't do anything OnPause.
        if (panel?.activeSelf == null)
		{
            panel = GameObject.Find("PauseMenu");
            if (panel == null) return;
		}
        if (pauseState == PauseState.Paused)
		{
            panel.SetActive(false);
            pauseState = PauseState.NotPaused;
            return;
        }
        panel.SetActive(true);
        pauseState = PauseState.Paused;
    }

    public void ExitToMenu()
	{
        NetworkManager.singleton.StopClient();
        NetworkManager.singleton.StopHost();
        //Destroy(NetworkManager.singleton);
        pauseState = PauseState.NotPaused;
        SceneManager.LoadScene(0);
	}
}

public enum PauseState
{
    NotPaused,
    Paused
}
