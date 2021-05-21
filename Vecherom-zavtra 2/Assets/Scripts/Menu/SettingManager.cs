using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
	public AudioMixer audioMixer;

	public Slider volumeSlider;



	void Awake()
	{
		
	}

	void Start()
	{

	}

	//Screen
	public void SetQuality(int qualityIdx)
	{
		QualitySettings.SetQualityLevel(qualityIdx);
	}

	// Sound
	public void SetMasterVolume(float volume)
	{
		audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
	}
	public void SetMusicVolume(float volume)
	{
		audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
	}
	public void SetEffectsVolume(float volume)
	{
		audioMixer.SetFloat("EffectsVolume", Mathf.Log10(volume) * 20);
	}

	

	

	
}
