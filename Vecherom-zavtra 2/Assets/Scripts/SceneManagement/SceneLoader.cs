using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Animator animator;

	public float transitionTime = 1f;

    void Start()
	{
		animator = transform.GetChild(0).GetComponent<Animator>();
	}

	public void LoadNextLevel()
	{
		StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 2));
	}

	IEnumerator LoadLevel(int levelIndex)
	{
		animator.SetTrigger("Start");

		yield return new WaitForSeconds(transitionTime);

		SceneManager.LoadScene(levelIndex);
	}
}
