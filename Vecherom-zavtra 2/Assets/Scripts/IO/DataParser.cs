using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Salaros.Configuration;
using System.Threading.Tasks;

public static class DataParser
{
    public static async Task SaveUserSettings(UserSettings settings)
	{
		//BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + "/user.cfg";
		string path2 = Application.persistentDataPath + "/usr.vz";
		
		if (!File.Exists(path))
			File.Create(path);
		var configFile = new ConfigParser(path);
		
		configFile.SetValue("Sound", "volume", settings.volume);
		configFile.SetValue("Screen", "resolution", settings.resolution);
		configFile.SetValue("Screen", "fullscreen", settings.isFullscreen);

		configFile.Save(path);

		int UserID;
		using (BinaryReader r = new BinaryReader(File.Open(path2, FileMode.Open)))
		{
			UserID = r.ReadInt32();
		}
		await Repository.SetUserConfig(new UserCfgRequest(UserID, settings.ToJson()));
	}

	public static UserSettings LoadUserSettings()
	{
		string path = Application.persistentDataPath + "/user.cfg";
		var configFile = new ConfigParser(path);

		UserSettings userSettings = new UserSettings();

		userSettings.volume = (float) configFile.GetValue("Sound", "volume", 0f);
		userSettings.resolution = configFile.GetValue("Screen", "resolution", "1280 x 720 @ 60Hz");
		userSettings.isFullscreen = configFile.GetValue("Screen", "fullscreen", true);

		return userSettings;
	}
}
