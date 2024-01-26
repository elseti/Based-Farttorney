using UnityEngine;

// base class of DialogueConstants scriptable objects.
[CreateAssetMenu(fileName = "DialogueConstants", menuName = "ScriptableObjects/DialogueConstants", order = 1)]
public class DialogueConstants : ScriptableObject
{
    [Tooltip("Speed of dialogue text (0 for instant text appearance).")]
    public float textSpeed;

    [Tooltip("If true, text will fade before appearing on screen.")]
    public bool enableFade;

    [Tooltip("Sound when dialogue is typed out. Leave empty if soundless.")]
    public AudioClip typingSfx;

    [Tooltip("Sound to make when dialogue is clicked/proceeds. Leave empty if soundless.")]
    public AudioClip dialogueContinueSfx;

    [Tooltip("Default SFX volume")] 
    public float sfxVolume = 1.0f;
    
    [Tooltip("Default BGM volume")] 
    public float bgmVolume = 1.0f;
    
    [Tooltip("Default Voice volume")] 
    public float voiceVolume = 1.0f;
    
    [Tooltip("Default Interface volume")] 
    public float interfaceVolume = 1.0f;
}