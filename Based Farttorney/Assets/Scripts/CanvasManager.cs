using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

/*
 * CanvasManager manages all dialogue/sprite/BG-related UI.
 */
public class CanvasManager : MonoBehaviour
{
    // Sprite or images - TODO: make them all an array/list so modular
    public GameObject spritePos1;
    public GameObject spritePos2;
    public GameObject spritePos3;
    // public GameObject spritePos4;
    public GameObject sidePos;
    
    // Canvas group to fade in/out
    public CanvasGroup canvasGroup;
    public CanvasGroup blackCanvas;
    
    // Text boxes with text inside 
    public Image textBox;
    public Image speakerBox;
    
    // Dialogue Text
    public TextMeshProUGUI textBoxText;
    public TextMeshProUGUI speakerBoxText;
    
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
            case "left":
                Image pos1 = spritePos1.GetComponent<Image>();
                SetAlpha(pos1, 1f);
                pos1.sprite = sprite;
                break;

            case "center":
                Image pos2 = spritePos2.GetComponent<Image>();
                SetAlpha(pos2, 1f);
                pos2.sprite = sprite;
                break;

            case "right":
                Image pos3 = spritePos3.GetComponent<Image>();
                SetAlpha(pos3, 1f);
                pos3.sprite = sprite;
                break;
            
            case "side":
                Image pos = sidePos.GetComponent<Image>();
                SetAlpha(pos, 1f);
                pos.sprite = sprite;
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
            case "left":
                SetAlpha(spritePos1.GetComponent<Image>(), 0f);
                break;

            case "center":
                SetAlpha(spritePos2.GetComponent<Image>(), 0f);
                break;

            case "right":
                SetAlpha(spritePos3.GetComponent<Image>(), 0f);
                break;
            
            case "side":
                SetAlpha(sidePos.GetComponent<Image>(), 0f);
                break;
            
            case "all":
                SetAlpha(spritePos1.GetComponent<Image>(), 0f);
                SetAlpha(spritePos2.GetComponent<Image>(), 0f);
                SetAlpha(spritePos3.GetComponent<Image>(), 0f);
                SetAlpha(sidePos.GetComponent<Image>(), 0f);
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
    
    // Fade in, fade out
    public void FadeOut(){
        StartCoroutine(IncreaseAlphaImageCoroutine(blackCanvas, 0.05f));
    }
    
    public void FadeIn(){
        StartCoroutine(DecreaseAlphaImageCoroutine(blackCanvas, 0.005f));   
    }
    
    
    // HELPER FUNCTIONS
    private void SetAlpha(Image image, float alphaValue)
    {
        Color currentColor = image.color;
        currentColor.a = alphaValue;
        image.color = currentColor;
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

    public void SetDialogueText(string fullText)
    {
        textBoxText.text = fullText;
        typingDone = true;
    }
    
    public IEnumerator TypeText(string fullText, float cps, AudioSource interfaceAudio, AudioClip typingSfx)
    {
        typingDone = false;
        print("typing is false");
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
        print("typing is true");
        
        // Reset the coroutine reference when the typing is done
        typingCoroutine = null;
    }
    
    private IEnumerator WaitCoroutine(float seconds){
        yield return new WaitForSecondsRealtime(seconds);
    }
}