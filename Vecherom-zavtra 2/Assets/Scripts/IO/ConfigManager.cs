using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ConfigManager : MonoBehaviour
{
	public AudioMixer audioMixer;
	public Slider masterVolumeSlider;
	public Slider musicVolumeSlider;
	public Slider effectsVolumeSlider;

	// Start is called before the first frame update
	void Start()
    {
		
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	void OnApplicationQuit()
	{
		SaveUserSettingsToDB();
		// Borrar fitxer d'usuari
		string path = Application.persistentDataPath + "/usr.vz";
		if (File.Exists(path)) File.Delete(path);
	}

	public async void SaveUserSettingsToDB()
	{
		UserCfg userSettings = new UserCfg();
		// userSettings.userID = Player.instance.userID ????
		userSettings.Screen.resolution = Screen.currentResolution.ToString();
		userSettings.Screen.fullscreen = Screen.fullScreen;
		userSettings.Sound.masterVolume = masterVolumeSlider.value;
		userSettings.Sound.musicVolume = musicVolumeSlider.value;
		userSettings.Sound.effectsVolume = effectsVolumeSlider.value;

		await DataParser.SaveUserSettings(userSettings);
	}

	public async Task LoadUserSettingsFromDB()
	{
		// Load user settings
		UserCfg userSettings = await DataParser.LoadUserSettings();
		if (userSettings != null)
		{
			// Set the resolution
			Resolution res = Utils.ResolutionFromString(userSettings.Screen.resolution);
			Screen.SetResolution(res.width, res.height, userSettings.Screen.fullscreen, res.refreshRate);

			// Set the volume
			masterVolumeSlider.value = userSettings.Sound.masterVolume;
			audioMixer.SetFloat("MasterVolume", Mathf.Log10(userSettings.Sound.masterVolume) * 20);
			musicVolumeSlider.value = userSettings.Sound.musicVolume;
			audioMixer.SetFloat("MusicVolume", Mathf.Log10(userSettings.Sound.musicVolume) * 20);
			effectsVolumeSlider.value = userSettings.Sound.effectsVolume;
			audioMixer.SetFloat("EffectsVolume", Mathf.Log10(userSettings.Sound.effectsVolume) * 20);
		}
	}
}
