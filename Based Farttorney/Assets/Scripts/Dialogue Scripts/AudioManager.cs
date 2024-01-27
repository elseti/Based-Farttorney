using System.Collections;
using DefaultNamespace;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Audio Sources
    public AudioSource bgmAudio;
    public AudioSource sfxAudio;
    public AudioSource voiceAudio;
    public AudioSource interfaceAudio;
    
    // Audio coroutines that wait until audio completion
    private Coroutine _sfxCoroutine;
    private Coroutine _bgmCoroutine;
    private Coroutine _voiceCoroutine;
    private Coroutine _interfaceCoroutine;
    
    
    // PlaySfx; usage: playSfx sfxName volume(optional)
    public void PlaySfx(string[] parameterList, float defaultVolume)
    {
        if (_sfxCoroutine != null)
        {
            sfxAudio.volume = defaultVolume;
            StopCoroutine(_sfxCoroutine);
        }
        if (parameterList == null || parameterList.Length > 2 )
        {
            Debug.Log("@CanvasManager.cs. PlaySfx(): Null or Invalid parameterList");
            return;
        }
        if (parameterList.Length > 1)
        {
            float volumeValue = HelperFunctions.ParseToFloat(parameterList[1]);
            volumeValue = Mathf.Clamp01(volumeValue);
            sfxAudio.volume = volumeValue;
        }

        AudioClip sfxClip = ResourceLoader.LoadSfx(parameterList[0]);
        sfxAudio.PlayOneShot(sfxClip);
        _sfxCoroutine = StartCoroutine(WaitForAudioCompletion(sfxAudio, sfxClip, defaultVolume));
    }
    
    // StopSfx; usage: stopSfx [TODO- ADD FADE]
    public void StopSfx(string[] parameterList, float defaultVolume)
    {
        if (_sfxCoroutine != null)
        {
            sfxAudio.volume = defaultVolume;
            StopCoroutine(_sfxCoroutine);
        }
        sfxAudio.Stop();
    }
    
    // PlayBgm; usage: playBgm bgmName volume(optional)
    public void PlayBgm(string[] parameterList, float defaultVolume)
    {
        if (parameterList == null || parameterList.Length > 2 )
        {
            Debug.Log("@AudioManager.cs. PlayBgm(): Null or Invalid parameterList");
            return;
        }
        if (parameterList.Length > 1)
        {
            float volumeValue = HelperFunctions.ParseToFloat(parameterList[1]);
            volumeValue = Mathf.Clamp01(volumeValue);
            bgmAudio.volume = volumeValue;
        }
        else
        {
            bgmAudio.volume = defaultVolume;
        }
        AudioClip bgmClip = ResourceLoader.LoadBgm(parameterList[0]);
        bgmAudio.clip = bgmClip;
        bgmAudio.Play();
    }
    
    // StopBgm; usage: stopBgm [TODO- ADD FADE]
    public void StopBgm(string[] parameterList, float defaultVolume)
    {
        bgmAudio.volume = defaultVolume;
        bgmAudio.Stop();
    }
    
    // PlayVoice
    public void PlayVoice(string[] parameterList, float defaultVolume)
    {
        if (_voiceCoroutine != null)
        {
            voiceAudio.volume = defaultVolume;
            StopCoroutine(_voiceCoroutine);
        }
        if (parameterList == null || parameterList.Length > 2 )
        {
            Debug.Log("@AudioManager.cs. PlayVoice(): Null or Invalid parameterList");
            return;
        }
        if (parameterList.Length > 1)
        {
            float volumeValue = HelperFunctions.ParseToFloat(parameterList[1]);
            volumeValue = Mathf.Clamp01(volumeValue);
            voiceAudio.volume = volumeValue;
        }

        AudioClip voiceClip = ResourceLoader.LoadVoice(parameterList[0]);
        voiceAudio.PlayOneShot(voiceClip);
        _voiceCoroutine = StartCoroutine(WaitForAudioCompletion(voiceAudio, voiceClip, defaultVolume));
    }
    
    // StopVoice; usage: stopVoice [TODO- ADD FADE]
    public void StopVoice(string[] parameterList, float defaultVolume)
    {
        if (_voiceCoroutine != null)
        {
            voiceAudio.volume = defaultVolume;
            StopCoroutine(_voiceCoroutine);
        }
        voiceAudio.Stop();
    }
    
    // PlayInterface
    public void PlayInterface(string[] parameterList, float defaultVolume)
    {
        if (_interfaceCoroutine != null)
        {
            interfaceAudio.volume = defaultVolume;
            StopCoroutine(_interfaceCoroutine);
        }
        if (parameterList == null || parameterList.Length > 2 )
        {
            Debug.Log("@AudioManager.cs. PlayInterface(): Null or Invalid parameterList");
            return;
        }
        if (parameterList.Length > 1)
        {
            float volumeValue = HelperFunctions.ParseToFloat(parameterList[1]);
            volumeValue = Mathf.Clamp01(volumeValue);
            interfaceAudio.volume = volumeValue;
        }

        AudioClip interfaceClip = ResourceLoader.LoadSfx(parameterList[0]);
        interfaceAudio.PlayOneShot(interfaceClip);
        _interfaceCoroutine = StartCoroutine(WaitForAudioCompletion(interfaceAudio, interfaceClip, defaultVolume));
    }
    
    // StopInterface; usage: stopInterface [TODO- ADD FADE]
    public void StopInterface(string[] parameterList, float defaultVolume)
    {
        if (_interfaceCoroutine != null)
        {
            interfaceAudio.volume = defaultVolume;
            StopCoroutine(_interfaceCoroutine);
        }
        interfaceAudio.Stop();
    }
    
    
    // COROUTINES
    public IEnumerator WaitForAudioCompletion(AudioSource audioSource, AudioClip audioClip, float defaultVolume)
    {
        yield return new WaitForSeconds(audioClip.length);
        print("Audio finished playing");
        audioSource.volume = defaultVolume;
    }
}