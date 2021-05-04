using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ConfigManager : MonoBehaviour
{
	public AudioMixer audioMixer;
	public Slider volumeSlider;

	// Start is called before the first frame update
	void Start()
    {
		// Load user settings
		UserSettings userSettings = DataParser.LoadUserSettings();
		if (userSettings != null)
		{
			// Set the resolution
			Resolution res = Utils.ResolutionFromString(userSettings.resolution);
			Screen.SetResolution(res.width, res.height, userSettings.isFullscreen, res.refreshRate);

			// Set the volume
			volumeSlider.value = userSettings.volume;
			audioMixer.SetFloat("Volume", Mathf.Log10(userSettings.volume) * 20);
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	void OnApplicationQuit()
	{
		//SaveUserSettings();
		// Borrar
		string path = Application.persistentDataPath + "/usr.vz";
		if (File.Exists(path)) File.Delete(path);
	}

	public void SaveUserSettings()
	{
		UserSettings userSettings = new UserSettings();
		// userSettings.userID = Player.instance.userID ????
		userSettings.resolution = Screen.currentResolution.ToString();
		userSettings.isFullscreen = Screen.fullScreen;
		userSettings.volume = volumeSlider.value;
		_ = DataParser.SaveUserSettings(userSettings);
	}
}
