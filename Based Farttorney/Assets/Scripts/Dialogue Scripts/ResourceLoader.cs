using System;
using UnityEngine;

public static class ResourceLoader
{
    public static Sprite LoadSprite(string path)
    {
        // TODO- make starting customisable?
        Sprite sprite = Resources.Load<Sprite>("Sprite/" + path);
        if (sprite == null)
        {
            throw new Exception("@ResourceLoader.cs: Sprite in path Sprite/" + path + " not found!");
        }

        return sprite;
    }
    
    public static Sprite LoadBG(string path)
    {
        // TODO- make starting customisable?
        Sprite sprite = Resources.Load<Sprite>("BG/" + path);
        if (sprite == null)
        {
            throw new Exception("@ResourceLoader.cs: Sprite in path Image/" + path + " not found!");
        }

        return sprite;
    }


    public static AudioClip LoadSfx(string path)
    {
        AudioClip audioClip = Resources.Load<AudioClip>("SFX/" + path);
        if (audioClip == null)
        {
            throw new Exception("@ResourceLoader.cs: AudioClip in path SFX/" + path + " not found!");
        }
        return audioClip;
    }
    
    public static AudioClip LoadBgm(string path)
    {
        AudioClip audioClip = Resources.Load<AudioClip>("BGM/" + path);
        if (audioClip == null)
        {
            throw new Exception("@ResourceLoader.cs: AudioClip in path BGM/" + path + " not found!");
        }
        return audioClip;
    }
    
    public static AudioClip LoadVoice(string path)
    {
        AudioClip audioClip = Resources.Load<AudioClip>("Voice/" + path);
        if (audioClip == null)
        {
            throw new Exception("@ResourceLoader.cs: AudioClip in path Voice/" + path + " not found!");
        }
        return audioClip;
    }
}