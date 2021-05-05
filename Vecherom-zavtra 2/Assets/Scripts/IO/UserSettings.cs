using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserSettings
{
    // User
    public int userID;

    // Sound
    public float masterVolume;
    public float musicVolume;
    public float effectsVolume;

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
                    "\"masterVolume\":" + masterVolume + "\"," +
                    "\"musicVolume\":" + musicVolume + "\"," +
                    "\"effectsVolume\":" + effectsVolume +
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
