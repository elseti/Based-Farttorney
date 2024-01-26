using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class VariableManager : MonoBehaviour
{
    private TextAsset _dialogueVariableText; //make public later?
    
    private List<DialogueVariable<string>> _stringList;
    private List<DialogueVariable<bool>> _boolList;
    private List<DialogueVariable<int>> _intList;
    private List<DialogueVariable<float>> _floatList;

    public void LoadDialogueVariables(TextAsset dialogueVariableText)
    {
        var result = TxtReader.ReadVariableText(dialogueVariableText);
        _stringList = result.Item1;
        _boolList = result.Item2;
        _intList = result.Item3;
        _floatList = result.Item4;
    }

    public List<Dialogue> AssignVariablesToDialogues(List<Dialogue> dialogueList)
    {
        foreach (Dialogue dialogue in dialogueList)
        {
            if (dialogue.GetAction() == null)
            {
                string speakerName = dialogue.GetName();
                string speakerText = dialogue.GetText();
                
                // check if name is a variable
                if (speakerName.Length > 2)
                {
                    if (speakerName[0] == '[' && speakerName[^1] == ']')
                    {
                        string keyToFind = HelperFunctions.GetSubstring(speakerName, 1, speakerName.Length - 2);
                        string valueFound = GetValue(keyToFind);
                        dialogue.SetName(valueFound);
                    }
                }
                
                // check if text contains a variable from the list
                List<string> keyList = GetAllKeys();
                foreach (string key in keyList)
                {
                    if (speakerText.Contains("[" + key + "]"))
                    {
                        string valueFound = GetValue(key);
                        speakerText = speakerText.Replace("[" + key + "]", valueFound);
                        dialogue.SetText(speakerText);
                    }
                }
            }
        }
        
        return dialogueList;
    }

    public bool CompareVariable(string key, string operation, string value)
    {
        var comparand = GetValue(key);
        if (comparand != "")
        {
            switch (operation)
            {
                case "=":
                case "==":
                    return comparand == value;
                
                case "!=":
                    return comparand != value;
                
                case ">":
                    break;
                
                default:
                    throw new Exception("@CompareVariable: Invalid operation " + operation);
                    
            }
        }
        return false;
    }
    

    // Increment and Decrement - Integer only
    public void IncrementVariable(string key)
    {
        foreach (DialogueVariable<int> dv in _intList)
        {
            if(string.Equals(dv.GetKey(), key, StringComparison.CurrentCultureIgnoreCase))
            {
                int newValue = dv.GetValue() + 1;
                dv.SetValue(newValue);
            }
        }
    }
    
    public void DecrementVariable(string key)
    {
        foreach (DialogueVariable<int> dv in _intList)
        {
            if(string.Equals(dv.GetKey(), key, StringComparison.CurrentCultureIgnoreCase))
            {
                int newValue = dv.GetValue() - 1;
                dv.SetValue(newValue);
            }
        }
    }

    public void SetVariableValue(string key, string type, string value)
    {
        // string valueFound = GetValue()
        if (type == "string")
        {
            foreach (DialogueVariable<string> dv in _stringList)
            {
                if(string.Equals(dv.GetKey(), key, StringComparison.CurrentCultureIgnoreCase))
                {
                    dv.SetValue(value);
                }
            }
        }

        if (type == "bool")
        {
            foreach (DialogueVariable<bool> dv in _boolList)
            {
                if(string.Equals(dv.GetKey(), key, StringComparison.CurrentCultureIgnoreCase))
                {
                    if (value == "true") dv.SetValue(true);
                    else dv.SetValue(false);
                }
            }
        }

        if (type == "int")
        {
            foreach (DialogueVariable<int> dv in _intList)
            {
                if(string.Equals(dv.GetKey(), key, StringComparison.CurrentCultureIgnoreCase))
                {
                    dv.SetValue(HelperFunctions.ParseToInt(value));
                }
            }
        }

        if (type == "float")
        {
            foreach (DialogueVariable<float> dv in _floatList)
            {
                if(string.Equals(dv.GetKey(), key, StringComparison.CurrentCultureIgnoreCase))
                {
                    dv.SetValue(HelperFunctions.ParseToFloat(value));
                }
            }
        }

        else
        {
            print("@VariableManager: Variable " + key + " not found!");
        }
    }

    public string GetValue(string key)
    {
        foreach (DialogueVariable<string> dv in _stringList)
        {
            if(string.Equals(dv.GetKey(), key, StringComparison.CurrentCultureIgnoreCase))
            {
                return dv.GetValue();
            }
        }
        foreach (DialogueVariable<bool> dv in _boolList)
        {
            if(string.Equals(dv.GetKey(), key, StringComparison.CurrentCultureIgnoreCase))
            {
                return "" + dv.GetValue();
            }
        }
        foreach (DialogueVariable<int> dv in _intList)
        {
            if(string.Equals(dv.GetKey(), key, StringComparison.CurrentCultureIgnoreCase))
            {
                return "" + dv.GetValue();
            }
        }
        foreach (DialogueVariable<float> dv in _floatList)
        {
            if(string.Equals(dv.GetKey(), key, StringComparison.CurrentCultureIgnoreCase))
            {
                return "" + dv.GetValue();
            }
        }
        
        print("@VariableManager: Variable " + key + " not found!");
        return "";
    }

    public List<string> GetAllKeys()
    {
        List<string> keyList = new List<string>();
        
        foreach (DialogueVariable<string> dv in _stringList)
        {
            keyList.Add(dv.GetKey());
        }
        foreach (DialogueVariable<bool> dv in _boolList)
        {
            keyList.Add(dv.GetKey());
        }
        foreach (DialogueVariable<int> dv in _intList)
        {
            keyList.Add(dv.GetKey());
        }
        foreach (DialogueVariable<float> dv in _floatList)
        {
            keyList.Add(dv.GetKey());
        }

        return keyList;
    }
    
}