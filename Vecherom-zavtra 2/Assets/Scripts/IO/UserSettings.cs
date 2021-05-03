using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserSettings
{
    // User
    public int userID;

    // Sound
    public float volume;

    // Screen
    public string resolution;
    public bool isFullscreen;

    public UserSettings()
	{

	}

    public string ToJson()
	{
        string json = 
            "{" +
                "\"Sound\":" +
                "{" +
                    "\"volume\":" + volume +
                "}," +
                "\"Screen\":" +
                "{" +
                    "\"resolution\":\"" + resolution + "\"," +
                    "\"isFullscreen\":\"" + isFullscreen + "\"" +
                "}" +
            "}";

        return json;
	}
}
