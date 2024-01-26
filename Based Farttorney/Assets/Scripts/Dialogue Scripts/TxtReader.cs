using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using Action = Dialogue_Scripts.Action;

// returns a DialogueLines (? - list) object ; separate between action and dialogue (make a class which has 2 of them...)
public static class TxtReader
{
    // Pass path of txt file relative to Resources
    public static List<Dialogue> ReadTextFile(string path)
    {

        List<Dialogue> dialogueList = new List<Dialogue>();
        TextAsset txt = (TextAsset)Resources.Load(path);
        if (txt == null) throw new Exception("Text not found in path " + path + "!");
        
        string content = txt.text;
        string[] lines = content.Split("\n\n");
        bool isComment = false;
        
        foreach (string line in lines)
        {
            isComment = false;
            
            string[] split = line.Split("\n");
            
            // check if it is comment. If it is, ignore.
            foreach (string splitLine in split)
            {
                if (splitLine.Length > 1)
                {
                    if (splitLine.Substring(0, 2) == "//")
                    {
                        isComment = true;
                    }
                }
            }

            if (!isComment)
            {
                if (split.Length > 2)
                {
                    throw new Exception("@" + path + ": Invalid format. More than 2 lines detected.");
                }

                // if have 2 or more lines, then it is a dialogue
                // TODO- condition if speaker is blank (remove speaker textbox)
                if (split.Length > 1)
                {
                    // check if SpeakerName is "null" (no speaker / narrative)
                    SpeakerName speakerName = new SpeakerName(split[0]);
                    if (split[0] == "null")
                    {
                        speakerName.text = "";
                    }
                    
                    SpeakerText speakerText = new SpeakerText(split[1]);
                    
                    dialogueList.Add(new Dialogue(speakerName, speakerText));
                }
            
                // if have only 1 line, it is an action
                else
                {
                    // try and check if action has parameters and instantiate accordingly.
                    string[] parameterList = split[0].Split(" "); // TODO- error check- space can be multiple?
                    if (parameterList.Length > 1)
                    {
                        // Action action = new Action(parameterList[0], (string[]) parameterList.Skip(1));
                        Action action = new Action(parameterList[0],
                            HelperFunctions.Slice(parameterList, 1, parameterList.Length - 1));
                        dialogueList.Add(new Dialogue(action));
                    }
                    else
                    {
                        Action action = new Action(parameterList[0]);
                        dialogueList.Add(new Dialogue(action));
                    }
                }   
            
            }
        }
        // TODO later: check if line only contains 1 line or 2 lines. if 1 line--> action, if 2 --> dialogue; if 3 --> error

        // PrintList(dialogueList);
        return dialogueList;
    }

    public static (List<DialogueVariable<string>>, List<DialogueVariable<bool>>, List<DialogueVariable<int>>, List<DialogueVariable<float>>)  ReadVariableText(TextAsset textAsset)
    {
        // For now doesn't check duplicates - the lowest one will replace 
        List<DialogueVariable<string>> stringList = new List<DialogueVariable<string>>();
        List<DialogueVariable<bool>> boolList = new List<DialogueVariable<bool>>();
        List<DialogueVariable<int>> intList = new List<DialogueVariable<int>>();
        List<DialogueVariable<float>> floatList = new List<DialogueVariable<float>>();

        string content = textAsset.text;
        string[] lines = content.Split("\n");
        
        foreach(string line in lines)
        {
            string[] split = line.Split(" ");
            if (split.Length != 3)
            {
                throw new Exception(
                    "@TxtReader.cs, ReadVariableText: DialogueVariable has wrong number of parameters (should be 3 but instead received " +
                    split.Length + " parameters.");
            }
            
            // error check- make sure there are no key duplicates
            string key = split[1];
            bool keyDuplicated = CheckKeyStringDuplicate(key, stringList) || CheckKeyBoolDuplicate(key, boolList) ||
                                 CheckKeyFloatDuplicate(key, floatList) || CheckKeyIntDuplicate(key, intList);
            if (keyDuplicated)
            {
                throw new Exception("@TxtReader.cs ReadVariableText: " + key + " key Duplicated!");
            }
            
            switch (split[0])
            {
                case "string":
                    stringList.Add(new DialogueVariable<string>(key, split[2]));
                    break;
                
                case "bool":
                    bool value;
                    if (split[2] == "true") value = true;
                    else if (split[2] == "false") value = false;
                    else throw new Exception("@TxtReader.cs, ReadVariableText: No such boolean variable " + split[2]);
                    boolList.Add(new DialogueVariable<bool>(key, value));
                    break;
                
                case"int":
                    intList.Add(new DialogueVariable<int>(key, HelperFunctions.ParseToInt(split[2])));
                    break;
                
                case "float":
                    floatList.Add(new DialogueVariable<float>(key, HelperFunctions.ParseToFloat(split[2])));
                    break;
                
                default:
                    throw new Exception("@TxtReader.cs, ReadVariableText: type " + split[0] + " does not exist!");
            }
        }
        // PrintKeyValue(stringList, boolList, intList, floatList);
        return (stringList, boolList, intList, floatList);
    }
    
    
    // Error-testing methods
    private static bool CheckKeyStringDuplicate(string key, List<DialogueVariable<string>> dialogueVariables)
    {
        foreach (DialogueVariable<string> dv in dialogueVariables)
        {
            if (string.Equals(dv.GetKey(), key, StringComparison.CurrentCultureIgnoreCase))
            {
                // there is a duplicate
                return true; 
            }
        }
        return false;
    }
    
    private static bool CheckKeyBoolDuplicate(string key, List<DialogueVariable<bool>> dialogueVariables)
    {
        foreach (DialogueVariable<bool> dv in dialogueVariables)
        {
            if (string.Equals(dv.GetKey(), key, StringComparison.CurrentCultureIgnoreCase))
            {
                // there is a duplicate
                return true; 
            }
        }
        return false;
    }
    
    private static bool CheckKeyIntDuplicate(string key, List<DialogueVariable<int>> dialogueVariables)
    {
        foreach (DialogueVariable<int> dv in dialogueVariables)
        {
            if (string.Equals(dv.GetKey(), key, StringComparison.CurrentCultureIgnoreCase))
            {
                // there is a duplicate
                return true; 
            }
        }
        return false;
    }
    
    private static bool CheckKeyFloatDuplicate(string key, List<DialogueVariable<float>> dialogueVariables)
    {
        foreach (DialogueVariable<float> dv in dialogueVariables)
        {
            if (string.Equals(dv.GetKey(), key, StringComparison.CurrentCultureIgnoreCase))
            {
                // there is a duplicate
                return true; 
            }
        }
        return false;
    }
    
    // for testing purposes
    private static void PrintArray(string[] strings)
    {
        foreach (string s in strings)
        {
            Debug.Log("" + s);
        }
    }

    private static void PrintList(List<Dialogue> list)
    {
        foreach (Dialogue l in list)
        {
            if (l.GetAction() != null)
            {
                Debug.Log("ACTION: " + l.GetAction().GetActionName());
            }
            else
            {
                Debug.Log("" + l.GetName() + ": " + l.GetText());
            }
        }
    }

    // print all DialogueVariable list
    private static void PrintKeyValue(List<DialogueVariable<string>> stringList, List<DialogueVariable<bool>> boolList, List<DialogueVariable<int>> intList, List<DialogueVariable<float>> floatList)
    {
        foreach (DialogueVariable<string> dv in stringList)
        {
            Debug.Log("Key: " + dv.GetKey() + " | Value: " + dv.GetValue());
        }
        
        foreach (DialogueVariable<bool> dv in boolList)
        {
            Debug.Log("Key: " + dv.GetKey() + " | Value: " + dv.GetValue());
        }
        
        foreach (DialogueVariable<int> dv in intList)
        {
            Debug.Log("Key: " + dv.GetKey() + " | Value: " + dv.GetValue());
        }
        
        foreach (DialogueVariable<float> dv in floatList)
        {
            Debug.Log("Key: " + dv.GetKey() + " | Value: " + dv.GetValue());
        }
    }
}