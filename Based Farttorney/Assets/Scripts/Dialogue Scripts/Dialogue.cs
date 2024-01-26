using Dialogue_Scripts;
using UnityEngine;
using UnityEngine.Device;

public class Dialogue
{
    private SpeakerName _name;
    private SpeakerText _text;
    private Action _action;
    
    public Dialogue(SpeakerName speakerName, SpeakerText speakerText)
    {
        _name = speakerName;
        _text = speakerText;
    }

    public Dialogue(Action action)
    {
        _action = action;
    }

    public void SetName(string name)
    {
        // TODO- strip trailing spaces
        _name.text = name;
    }
    
    public string GetName()
    {
        if (_name != null)
        {
            return _name.text;
        }
        Debug.Log("@Dialogue.cs: Name is null");
        return "";
    }

    public void SetText(string text)
    {
        _text.text = text;
    }
    
    public string GetText()
    {
        if (_text != null)
        {
            return _text.text;
        }
        Debug.Log("@Dialogue.cs: Text is null");
        return "";
    }

    public Action GetAction()
    {
        return _action;
    }
}