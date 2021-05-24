using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	[HideInInspector] public Animator animator;

	public float transitionTime = 1f;

    void Start()
	{
		animator = transform.GetChild(0).GetComponent<Animator>();
	}

	public void LoadNextLevel()
	{
		StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
	}
	public void LoadLevel(string sceneName)
	{
		StartCoroutine(LoadScene(sceneName));
	}

	IEnumerator LoadLevel(int levelIndex)
	{
		animator.SetTrigger("Start");

		yield return new WaitForSeconds(transitionTime);

		SceneManager.LoadScene(levelIndex);
	}

	IEnumerator LoadScene(string sceneName)
	{
		animator.SetTrigger("Start");

		yield return new WaitForSeconds(transitionTime);

		SceneManager.LoadScene(sceneName);
	}
}
