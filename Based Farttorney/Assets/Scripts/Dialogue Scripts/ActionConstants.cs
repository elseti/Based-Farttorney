using UnityEngine;

// base class of ActionConstants scriptable objects.
[CreateAssetMenu(fileName = "ActionConstants", menuName = "ScriptableObjects/ActionConstants", order = 2)]
public class ActionConstants : ScriptableObject
{
    [Tooltip("List of available actions. To create a custom action, add the action name here and add a case statement in PlayAction() + corresponding function in CanvasManager.")]
    public string[] actionList =
    {
        "switchCamera",
        "showChar",
        "hideChar",
        "showBg",
        "hideBg",
        "playSfx",
        "stopSfx",
        "playBgm",
        "stopBgm",
        "playVoice",
        "stopVoice",
        "showChoices",
        "hideChoices",
        "showCanvas",
        "hideCanvas",
        "wait",
        "fadein",
        "fadeout",
        "show",
        "hide",
        "playScript",
        "loadScene",
        "setString",
        "setBool",
        "setInt",
        "setFloat",
        "increment",
        "decrement",
        "compare",
        "choice"
    };
}