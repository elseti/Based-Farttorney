using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public string _gameScene;
    public float _fadeDuration = 2.0f, _fadeDelay = 60.0f;
    public GameObject _msgLose, _msgTryAgain, _btnTryAgain;

    // change scenes to start and end game
    public void PlayGame()
    {
        SceneManager.LoadScene(_gameScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // fade out joke message, fade in try again button
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (_msgLose != null && _msgTryAgain != null && _btnTryAgain != null)
            StartCoroutine(FadeMessages());
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_msgLose != null && _msgTryAgain != null && _btnTryAgain != null)
            StartCoroutine(FadeMessages());
    }

    IEnumerator FadeMessages()
    {
        Color m1Color = _msgLose.GetComponent<Image>().color, m2Color = _msgTryAgain.GetComponent<Image>().color, bColor = _btnTryAgain.GetComponent<Image>().color;
        float elapsedTime = 0f;

        // Set All Objects Transparent
        _msgLose.GetComponent<Image>().color = new Color(m1Color.r, m1Color.g, m1Color.b, 0.0f);
        _msgTryAgain.GetComponent<Image>().color = new Color(m2Color.r, m2Color.g, m2Color.b, 0.0f);
        _btnTryAgain.GetComponent<Image>().color = new Color(bColor.r, bColor.g, bColor.b, 0.0f);


        // fade in lose message
        while (elapsedTime < _fadeDuration)
        {
            float alpha = Mathf.Lerp(0, 1, elapsedTime / _fadeDuration);
            _msgLose.GetComponent<Image>().color = new Color(m1Color.r, m1Color.g, m1Color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null; //waits for a frame
        }
        elapsedTime = 0f;


        // fade out lose message
        yield return new WaitForSeconds(_fadeDelay);
        while (elapsedTime < _fadeDuration)
        {
            float alpha = Mathf.Lerp(1, 0, elapsedTime / _fadeDuration);
            _msgLose.GetComponent<Image>().color = new Color(m1Color.r, m1Color.g, m1Color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null; //waits for a frame
        }
        elapsedTime = 0f;


        // fade in try again message and try again button
        yield return new WaitForSeconds(_fadeDelay);
        while (elapsedTime < _fadeDuration)
        {
            float alpha = Mathf.Lerp(0, 1, elapsedTime / _fadeDuration);
            _msgTryAgain.GetComponent<Image>().color = new Color(m2Color.r, m2Color.g, m2Color.b, alpha);
            _btnTryAgain.GetComponent<Image>().color = new Color(bColor.r, bColor.g, bColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null; //waits for a frame
        }
        elapsedTime = 0f;

        // make try again button active
    }
}
