using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Action = Dialogue_Scripts.Action;

public class DialogueManager : Singleton<DialogueManager>
{
    // Dialogue Script Path
    [Tooltip("Path of txt file containing the script to play relative from Resources folder")]
    public string dialogueTextPath;
    
    // Scriptable Object
    [Tooltip("Scriptable Object (DialogueConstants.asset) containing default canvas settings")]
    public DialogueConstants dialogueConstants;
    
    // Text file for variables
    public TextAsset dialogueVariableText;
    
    // used when checking if raycast to world space interactables is activated or not
    [HideInInspector] public bool isDialoguePlaying;

    // Dialogues
    private List<Dialogue> _currDialogueList; // list storing all Dialogue objects
    private int _currDialogueIndex = -1; // index to increase dialogue count
    
    // to stop typing sound
    private AudioClip _silenceSfx;
    
    // Variables from DialogueConstants
    private float _cps; // character per second
    private bool _enableFade; //idk yet
    private AudioClip _typingSfx;
    private AudioClip _dialogueContinueSfx;
    private float _sfxVolume;
    private float _bgmVolume;
    private float _voiceVolume;
    private float _interfaceVolume;
    
    // Managers
    public AudioManager audioManager;
    public CanvasManager canvasManager;
    public VariableManager variableManager;
    public CameraManager cameraManager;
    
    // control variables
    [HideInInspector]
    public bool canClick = true;
    
    
    private void Start()
    {
        LoadDialogueConstants(); // Load variables from SO
        variableManager.LoadDialogueVariables(dialogueVariableText);
        LoadDialogueList(dialogueTextPath);
        _silenceSfx = Resources.Load<AudioClip>("DialogueSystem/silence");
    }

    private void Update()
    {
        // TODO - DISABLE RETURN AND SPACE
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && canClick)
        {
            if (_currDialogueList != null)
            {
                // // if typing coroutine not done, make it done
                // if (!canvasManager.typingDone)
                // {
                //     StopCoroutine(canvasManager.typingCoroutine);
                //     canvasManager.SetDialogueText(_currDialogueList[_currDialogueIndex].GetText());
                // }
                if (!canvasManager.typingDone && canvasManager.typingCoroutine != null)
                {
                    StopCoroutine(canvasManager.typingCoroutine);
                    canvasManager.SetDialogueText(_currDialogueList[_currDialogueIndex].GetText());
                }
                else
                {
                    NextDialogue();
                }
                
            }
            
        }

        // TODO: backtrack
    }

    public void LoadDialogueList(string path)
    {
        EndDialogue();
        List<Dialogue> unAssignedDialogueList = DialogueRecords.LoadDialogueList(path);
        _currDialogueList = variableManager.AssignVariablesToDialogues(unAssignedDialogueList);
        isDialoguePlaying = true;
        NextDialogue();
    }

    private void PlayDialogue()
    {
        if (canvasManager.typingCoroutine != null)
        {
            StopCoroutine(canvasManager.typingCoroutine);
        }
    
        canvasManager.textBoxText.text = "";
        string speakerName = _currDialogueList[_currDialogueIndex].GetName();
        string speakerText = _currDialogueList[_currDialogueIndex].GetText();
        
        if (speakerName == "")
        {
            canvasManager.SetAlphaImage(canvasManager.speakerBox, 0f);
            canvasManager.speakerBoxText.text = "";
        }
        else
        {
            canvasManager.SetAlphaImage(canvasManager.speakerBox, 1f);
            canvasManager.speakerBoxText.text = speakerName;
        }
        

        // Start the coroutine and store a reference to it
        canvasManager.typingCoroutine = StartCoroutine(canvasManager.TypeText(speakerText, _cps, audioManager.interfaceAudio, _typingSfx));
    
        
    }

    
    
    // Increase the next index and play next dialogue
    private void NextDialogue()
    {
        if (_currDialogueIndex >= _currDialogueList.Count - 1)
        {
            isDialoguePlaying = false;
            EndDialogue();
        }
        else
        {
            if (canClick)
            {
                audioManager.interfaceAudio.PlayOneShot(_dialogueContinueSfx);
                _currDialogueIndex++;
            
                // check if it is an Action
                if(_currDialogueList[_currDialogueIndex].GetAction() != null)
                {
                    print("ACTION " + _currDialogueList[_currDialogueIndex].GetAction().GetActionName() + " PLAYED!");
                    // TODO- if(action cannot be clicked to continue...):
                
                    PlayAction(_currDialogueList[_currDialogueIndex].GetAction());
                    NextDialogue();
                }
                else
                {
                    PlayDialogue();
                }
            }
            
        }
    }

    private void EndDialogue()
    {
        // reset index 
        audioManager.interfaceAudio.Stop();
        _currDialogueIndex = -1;
        // _currDialogueIndex = 0;
        _currDialogueList = null;

        // hide all boxes
        // canvasManager.HideCanvas();
        
        // clear all text
        canvasManager.speakerBoxText.text = "";
        canvasManager.textBoxText.text = "";
    }

    private void PlayAction(Action action)
    {
        string actionName = action.GetActionName();
        string[] parameterList = action.GetParameterList();
        switch (actionName)
        {
            case "switchCamera":
                if(parameterList.Length == 1) 
                    cameraManager.SwitchCamera(parameterList[0]);
                else if (parameterList.Length == 2)
                    cameraManager.SwitchCamera(parameterList[0], HelperFunctions.ParseToFloat(parameterList[1]));
                else 
                    throw new Exception("@case switchCamera: Too many arguments for switchCamera");
                break;
            
            case "showChar":
            case "show":
                canvasManager.ShowChar(parameterList);
                break;
            
            case "hideChar":
            case "hide":
                canvasManager.HideChar(parameterList);
                break;
            
            case "showBg":
                break;
            
            case "hideBg":
                break;
            
            case "playSfx":
                audioManager.PlaySfx(parameterList, _sfxVolume);
                break;
            
            case "stopSfx":
                audioManager.StopSfx(parameterList, _sfxVolume);
                break;
            
            case "playBgm":
                audioManager.PlayBgm(parameterList, _bgmVolume);
                break;
            
            case "stopBgm":
                audioManager.StopBgm(parameterList, _bgmVolume);
                break;
            
            case "playVoice":
                audioManager.PlayVoice(parameterList, _voiceVolume);
                break;
            
            case "stopVoice":
                audioManager.PlayVoice(parameterList, _voiceVolume);
                break;
            
            case "playInterface":
                audioManager.PlayInterface(parameterList, _interfaceVolume);
                break;
            
            case "stopInterface":
                audioManager.StopInterface(parameterList, _interfaceVolume);
                break;
            
            case "showChoices":
                break;
            
            case "hideChoices":
                break;
            
            case "showCanvas":
                canvasManager.ShowCanvas();
                break;
            
            case "hideCanvas":
                canvasManager.HideCanvas();
                break;
            
            case "wait":
                if (parameterList.Length == 1)
                    StartCoroutine((WaitCoroutine(HelperFunctions.ParseToFloat(parameterList[0]))));
                else
                    throw new Exception("@wait: Too many arguments. Expected 1, got " + parameterList.Length);
                break;
            
            case "fadeIn":
                canvasManager.FadeIn();
                break;
            
            case "fadeOut":
                canvasManager.FadeOut();
                break;
            
            case "playScript":
                if (parameterList.Length == 1)
                {
                    canClick = true;
                    LoadDialogueList(parameterList[0]);
                }
                else throw new Exception("@playScript: Too many arguments. Expected 1, got " + parameterList.Length);
                break;
            
            case "loadScene":
                if (parameterList.Length == 1) LoadScene(parameterList[0]);
                else throw new Exception("@loadScene: Too many arguments. Expected 1, got " + parameterList.Length);
                break;
            
            case "setString":
                if(parameterList.Length == 2) variableManager.SetVariableValue(parameterList[0], "string", parameterList[1]);
                else throw new Exception("@setString: Incorrect number of arguments. Expected 2, got " + parameterList.Length + ". Usage: setString [variable name] [new value]");
                break;
            
            case "setBool":
                if(parameterList.Length == 2) variableManager.SetVariableValue(parameterList[0], "bool", parameterList[1]);
                else throw new Exception("@setBool: Incorrect number of arguments. Expected 2, got " + parameterList.Length + ". Usage: setBool [variable name] [new value]");
                break;
            
            case "setInt":
                if(parameterList.Length == 2) variableManager.SetVariableValue(parameterList[0], "int", parameterList[1]);
                else throw new Exception("@setInt: Incorrect number of arguments. Expected 2, got " + parameterList.Length + ". Usage: setInt [variable name] [new value]");
                break;
            
            case "setFloat":
                if(parameterList.Length == 2) variableManager.SetVariableValue(parameterList[0], "float", parameterList[1]);
                else throw new Exception("@setFloat: Incorrect number of arguments. Expected 2, got " + parameterList.Length + ". Usage: setFloat [variable name] [new value]");
                break;
            
            case "increment":
                if(parameterList.Length == 1) variableManager.IncrementVariable(parameterList[0]);
                else throw new Exception("@increment: Incorrect number of arguments. Expected 1, got " + parameterList.Length + ". Usage: increment [variable name (integer)]");
                break;
                
            case "decrement":
                if(parameterList.Length == 1) variableManager.DecrementVariable(parameterList[0]);
                else throw new Exception("@decrement: Incorrect number of arguments. Expected 1, got " + parameterList.Length + ". Usage: decrement [variable name (integer)]");
                break;
                
            case "compare":
                if (parameterList.Length == 4)
                {
                    // if comparison is true, playScript
                    if (variableManager.CompareVariable(parameterList[0], parameterList[1], parameterList[2]))
                    {
                        LoadDialogueList(parameterList[3]);
                    }
                }
                else throw new Exception("@compare: Incorrect number of arguments. Expected 4, got " + parameterList.Length + ". Usage: compare [variable compared to] [operand] [compared value] [script path played if true]");
                break;
            
            case "choice":
                (string[], string[]) choiceParserResult = HelperFunctions.ParseChoices(parameterList);
                canvasManager.AddChoiceBox(choiceParserResult.Item1, choiceParserResult.Item2);
                break;
            
            default:
                Debug.LogError("@CanvasManager.cs, PlayAction(): No such method " + actionName + ". If you created a custom function, don't forget to add a case here, or update the ActionConstants asset.");
                break;
        }
    }
    
    // HELPER FUNCTIONS
    
    private void LoadDialogueConstants()
    {
        // assign dialogue constants to local variables
        _cps = dialogueConstants.textSpeed;
        _enableFade = dialogueConstants.enableFade;
        _typingSfx = dialogueConstants.typingSfx;
        _dialogueContinueSfx = dialogueConstants.dialogueContinueSfx;
        _sfxVolume = Mathf.Clamp01(dialogueConstants.sfxVolume);
        _bgmVolume = Mathf.Clamp01(dialogueConstants.bgmVolume);
        _voiceVolume = Mathf.Clamp01(dialogueConstants.voiceVolume);
        _interfaceVolume = Mathf.Clamp01(dialogueConstants.interfaceVolume);
        
        // set volumes to audio sources
        audioManager.sfxAudio.volume = _sfxVolume;
        audioManager.bgmAudio.volume = _bgmVolume;
        audioManager.voiceAudio.volume = _voiceVolume;
        audioManager.interfaceAudio.volume = _interfaceVolume;
    }

    
    // TODO - canCTC
    private IEnumerator WaitCoroutine(float waitTime, bool canClickToContinue = false)
    {
        canClick = false;
        audioManager.interfaceAudio.PlayOneShot(_silenceSfx);
        yield return new WaitForSeconds(waitTime);
        canClick = true;
        NextDialogue();
    }

    private void LoadScene(string sceneName)
    {
        try
        {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        }
        catch
        {
            throw new Exception("@LoadScene: Scene " + sceneName + " does not exist!");
        }
    }
    
    
    // GAMEPLAY FUNCTIONS
    public void GameOver(string ending)
    {
        canvasManager.HideCanvas();
        canvasManager.HideChoices();
        audioManager.bgmAudio.Stop();
        EndDialogue();
        
        cameraManager.SwitchCamera("Black", 3f);
        StartCoroutine(WaitCoroutine(3f));
        switch (ending)
        {
            case "fart":
                break;
            
            case "vomit":
                break;
            
        }
    }

}