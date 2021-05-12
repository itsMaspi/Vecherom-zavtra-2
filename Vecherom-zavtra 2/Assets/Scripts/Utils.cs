using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Utils
{
	public static Resolution ResolutionFromString(string s)
	{
		int width = int.Parse(s.Split('x')[0].Trim());
		int height = int.Parse(s.Split('x')[1].Split('@')[0].Trim());
		int refresh = int.Parse(s.Split('@')[1].Split('H')[0].Trim());
		return new Resolution() { width = width, height = height, refreshRate = refresh };
	}

	public static void DeleteUserInfo()
	{
		string path = Application.persistentDataPath + "/usr.vz";
		if (File.Exists(path)) File.Delete(path);
	}

	public static void DeleteLoginUserInfo()
	{
		string path = Application.persistentDataPath + "/loginInfo.vz";
		if (File.Exists(path)) File.Delete(path);
	}
}
