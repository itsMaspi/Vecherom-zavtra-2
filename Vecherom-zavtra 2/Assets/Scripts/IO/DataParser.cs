using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Salaros.Configuration;
using System.Threading.Tasks;
using Newtonsoft.Json;

public static class DataParser
{
    public static async Task SaveUserSettings(UserCfg settings)
	{
		//BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + "/user.cfg";
		string path2 = Application.persistentDataPath + "/usr.vz";
		
		if (!File.Exists(path))
			File.Create(path);
		var configFile = new ConfigParser(path);
		
		configFile.SetValue("Sound", "masterVolume", settings.Sound.masterVolume);
		configFile.SetValue("Sound", "musicVolume", settings.Sound.musicVolume);
		configFile.SetValue("Sound", "effectsVolume", settings.Sound.effectsVolume);
		configFile.SetValue("Screen", "resolution", settings.Screen.resolution);
		configFile.SetValue("Screen", "fullscreen", settings.Screen.fullscreen);

		configFile.Save(path);

		int UserID;
		using (BinaryReader r = new BinaryReader(File.Open(path2, FileMode.Open)))
		{
			UserID = r.ReadInt32();
		}
		await Repository.SetUserConfig(new UserCfgRequest(UserID, JsonConvert.SerializeObject(settings)));
	}

	public static async Task<UserCfg> LoadUserSettings()
	{
		string path = Application.persistentDataPath + "/user.cfg";
		string path2 = Application.persistentDataPath + "/usr.vz";

		UserCfg userSettings = new UserCfg();

		// If there isn't a user.cfg but the user is logged in, import the config from the database
		if (!File.Exists(path) && File.Exists(path2))
		{
			int UserID;
			using (BinaryReader r = new BinaryReader(File.Open(path2, FileMode.Open)))
				UserID = r.ReadInt32();
			string json = await Repository.GetUserConfig(new UserCfgRequest(UserID, ""));
			userSettings = JsonConvert.DeserializeObject<UserCfg>(json);
			return userSettings;
		}
		var configFile = new ConfigParser(path);

		userSettings.Sound.masterVolume = (float) configFile.GetValue("Sound", "masterVolume", 1f);
		userSettings.Sound.musicVolume = (float)configFile.GetValue("Sound", "musicVolume", .3f);
		userSettings.Sound.effectsVolume = (float)configFile.GetValue("Sound", "effectsVolume", 1f);
		userSettings.Screen.resolution = configFile.GetValue("Screen", "resolution", "1280 x 720 @ 60Hz");
		userSettings.Screen.fullscreen = configFile.GetValue("Screen", "fullscreen", false);

		return userSettings;
	}
}
