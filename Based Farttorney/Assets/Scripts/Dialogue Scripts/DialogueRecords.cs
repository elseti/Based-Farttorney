using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public static class DialogueRecords
{
    // TODO- make more modular; just use name...
    private static List<Dialogue> _dialogueList;

    public static List<Dialogue> LoadDialogueList(string path)
    {
        _dialogueList = TxtReader.ReadTextFile(path);
        return _dialogueList;
        // throw new Exception("@DialogueRecords.cs: File not found!");
    }
}