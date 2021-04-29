using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
	public AudioMixer audioMixer;

	public TMPro.TMP_Dropdown resolutionDropdown;
	public Slider volumeSlider;
	public Toggle fullscreenToggle;

	Resolution[] resolutions;

	void Awake()
	{
		
	}

	void Start()
	{
		fullscreenToggle.isOn = Screen.fullScreen;

		resolutions = Screen.resolutions;

		resolutionDropdown.ClearOptions();
		List<string> options = new List<string>();

		int currentResolutionIdx = 0;
		for (int i = 0; i < resolutions.Length; i++)
		{
			if ((resolutions[i].width < 1280 && resolutions[i].height < 720) ||
				(resolutions[i].refreshRate != 30 && resolutions[i].refreshRate != 60 && resolutions[i].refreshRate != 90 && resolutions[i].refreshRate != 144))
			{
				continue;
			}
			string option = $"{resolutions[i].width} x {resolutions[i].height} @ {resolutions[i].refreshRate}Hz)";
			options.Add(option);

			if (resolutions[i].Equals(Screen.currentResolution))
			{
				currentResolutionIdx = i;
			}
		}
		resolutionDropdown.AddOptions(options);
		resolutionDropdown.value = currentResolutionIdx;
		resolutionDropdown.RefreshShownValue();
	}

	public void SetVolume(float volume)
	{
		audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
	}

	public void SetFullscreen(bool isFullscreen)
	{
		Screen.fullScreen = isFullscreen;
	}

	public void SetResolution(int resolutionIdx)
	{
		//Resolution resolution = resolutions[resolutionIdx];
		Resolution resolution = Utils.ResolutionFromString(resolutionDropdown.options[resolutionDropdown.value].text);
		//Debug.LogError(resolution.ToString());
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
	}

	
}
