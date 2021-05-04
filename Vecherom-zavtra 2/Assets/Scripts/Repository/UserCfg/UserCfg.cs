using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class UserCfg
{
    public SoundCfg Sound;
    public ScreenCfg Screen;
}

[System.Serializable]
public class ScreenCfg
{
    public string resolution;
    public bool fullscreen;
}

[System.Serializable]
public class SoundCfg
{
    public float volume;
}