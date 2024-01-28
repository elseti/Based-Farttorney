using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Build.Content;

/*
 * CanvasManager manages all dialogue/sprite/BG-related UI.
 */
public class CanvasManager : MonoBehaviour
{
    // Sprite or images - TODO: make them all an array/list so modular
    public SpriteRenderer barkeepPos;
    public SpriteRenderer ladyPos;
    public SpriteRenderer oldManPos;
    public SpriteRenderer jockPos;
    
    
    // Canvas group to fade in/out
    public CanvasGroup canvasGroup;
    public CanvasGroup blackCanvas;
    
    // Text boxes with text inside 
    public Image textBox;
    public Image speakerBox;
    
    // Dialogue Text
    public TextMeshProUGUI textBoxText;
    public TextMeshProUGUI speakerBoxText;
    
    // Choice boxes
    public Transform choiceLocation;
    public GameObject choiceBox;
    
    // Coroutine for typing dialogue text
    public Coroutine typingCoroutine;
    
    // if typing coroutine is done or not
    [HideInInspector] public bool typingDone;

    private void Start()
    {
        try
        {
            textBoxText = textBox.GetComponentInChildren<TextMeshProUGUI>();
            speakerBoxText = speakerBox.GetComponentInChildren<TextMeshProUGUI>();
        }
        catch
        {
            throw new Exception("@CanvasManager: Text components not found!");
        }
    }
    
    // Shows character image on one of the image positions
    public void ShowChar(string[] parameterList)
    {
        
        if (parameterList == null || parameterList.Length < 2)
        {
            Debug.Log("@CanvasManager.cs, ShowChar(): Invalid parameterList");
            return;
        }

        Sprite sprite = ResourceLoader.LoadSprite(parameterList[0]);

        switch (parameterList[1])
        {
            case "barkeep":
                SetAlphaSprite(barkeepPos, 1f);
                barkeepPos.sprite = sprite;
                break;

            case "lady":
                SetAlphaSprite(ladyPos, 1f);
                ladyPos.sprite = sprite;
                break;

            case "oldMan":
                SetAlphaSprite(oldManPos, 1f);
                oldManPos.sprite = sprite;
                break;
            
            case "jock":
                SetAlphaSprite(jockPos, 1f);
                jockPos.sprite = sprite;
                break;

            default:
                Debug.LogError("Unknown position: " + parameterList[1]);
                break;
        }
    }
    
    // HideChar by position. Usage: hideChar left
    public void HideChar(string[] parameterList)
    {
        if (parameterList == null)
        {
            Debug.Log("@CanvasManager.cs. HideChar(): Null parameterList");
            return;
        }
        // For now, hide character by position (left, center, right)

        switch (parameterList[0])
        {
            case "barkeep":
                SetAlphaSprite(barkeepPos, 0f);
                break;

            case "lady":
                SetAlphaSprite(ladyPos, 0f);
                break;

            case "oldMan":
                SetAlphaSprite(oldManPos, 0f);
                break;
            
            case "jock":
                SetAlphaSprite(jockPos, 0f);
                break;
            
            case "all":
                SetAlphaSprite(barkeepPos, 0f);
                SetAlphaSprite(ladyPos, 0f);
                SetAlphaSprite(oldManPos, 0f);
                SetAlphaSprite(jockPos, 0f);
                break;
            
            default:
                Debug.LogError("Unknown position: " + parameterList[0]);
                break;
        }
    }

    public void HideCanvas()
    {
        canvasGroup.gameObject.SetActive(false);
    }

    public void ShowCanvas()
    {
        canvasGroup.gameObject.SetActive(true);
    }

    public void ShowChoices()
    {
        choiceLocation.gameObject.SetActive(true);
    }

    public void HideChoices()
    {
        choiceLocation.gameObject.SetActive(false);
    }
    
    // Fade in, fade out
    public void FadeOut(){
        StartCoroutine(IncreaseAlphaImageCoroutine(blackCanvas, 0.05f));
    }
    
    public void FadeIn(){
        StartCoroutine(DecreaseAlphaImageCoroutine(blackCanvas, 0.005f));   
    }
    
    public void SetDialogueText(string fullText)
    {
        textBoxText.text = fullText;
        typingDone = true;
    }

    public void AddChoiceBox(string[] choiceTextList, string[] choiceScriptList)
    {
        ShowChoices();
        float buttonGap = 120f;
        speakerBox.gameObject.SetActive(false);
        textBox.gameObject.SetActive(false);
        DialogueManager.instance.canClick = false;
        for (int x = 0; x < choiceTextList.Length; x++)
        {
            GameObject choice = Instantiate(choiceBox, choiceLocation);
            choice.SetActive(true);
            choice.transform.localPosition = new Vector3(0, x * -buttonGap, 0);
            choice.GetComponentInChildren<TextMeshProUGUI>().text = choiceTextList[x];
            var x1 = x;
            choice.GetComponent<Button>().onClick.AddListener(() => PlayScriptAfterChoice(choiceScriptList[x1]));
        }
    }


    public void PlayScriptAfterChoice(string script)
    {
        // print("played script " + script);
        
        DialogueManager.instance.canClick = true;
        
        speakerBox.gameObject.SetActive(true);
        textBox.gameObject.SetActive(true);
        
        DialogueManager.instance.LoadDialogueList(script);
        HideChoices();
    }
    
    // HELPER FUNCTIONS
    public void SetAlphaImage(Image image, float alphaValue)
    {
        Color currentColor = image.color;
        currentColor.a = alphaValue;
        image.color = currentColor;
    }
    
    public void SetAlphaSprite(SpriteRenderer spriteRenderer, float alpha)
    {
        Color currentColor = spriteRenderer.material.color;
        currentColor.a = Mathf.Clamp01(alpha);
        spriteRenderer.material.color = currentColor;
    }
    
    private IEnumerator IncreaseAlphaImageCoroutine(CanvasGroup canvas, float multiplier){
        for (float alpha = 1f; alpha >= -0.05f; alpha -= 0.05f)
        {
            canvas.alpha = alpha;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
    
    private IEnumerator DecreaseAlphaImageCoroutine(CanvasGroup canvas, float multiplier){
        yield return new WaitForSecondsRealtime(2);

        for (float alpha = 1f; alpha >= -0.05f; alpha -= 0.05f)
        {
            canvas.alpha = alpha;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        
        canvas.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }

   
    public IEnumerator TypeText(string fullText, float cps, AudioSource interfaceAudio, AudioClip typingSfx)
    {
        typingDone = false;
        foreach (char c in fullText)
        {
            // TODO- make function such that if dialogue is not done but button is pressed typing sound will stop...
            
            if (typingSfx != null)
            {
                interfaceAudio.PlayOneShot(typingSfx);
            }
            textBoxText.text += c;
            yield return new WaitForSeconds(1f / cps);
        }
        typingDone = true;
        
        // Reset the coroutine reference when the typing is done
        typingCoroutine = null;
    }
    
    private IEnumerator WaitCoroutine(float seconds){
        yield return new WaitForSecondsRealtime(seconds);
    }
}