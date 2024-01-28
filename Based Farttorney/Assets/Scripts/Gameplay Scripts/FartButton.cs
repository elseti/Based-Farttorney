using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay_Scripts
{
    public class FartButton : MonoBehaviour
    {

        public AudioClip fartSfx;
        public float minAppearDuration = 1f;
        public float maxAppearDuration = 5f;
        public float minHideDuration = 1f;
        public float maxHideDuration = 10f;

        private Coroutine _appearCoroutine;
        
        private void Update()
        {
            if (Random.Range(0, 1) < 0.25 && _appearCoroutine == null)
            {
                _appearCoroutine = StartCoroutine(AppearCoroutine(Random.Range(minAppearDuration, maxAppearDuration), Random.Range(minHideDuration, maxHideDuration)));
            }
        }
        
        public void FartButtonPressed()
        {
            this.gameObject.GetComponent<AudioSource>().PlayOneShot(fartSfx);
            StartCoroutine(WaitForAudioCompletion(fartSfx));
            DialogueManager.instance.ShowEndingCard("bg_MenuLose");
        }

        private void ShowFartButton()
        {
            print("show fart button");
            this.transform.localPosition = GetRandomPosition(960f, 540f);
            this.gameObject.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            this.gameObject.GetComponent<Button>().enabled = true;
            this.gameObject.GetComponent<Image>().enabled = true;
        }
        private void HideFartButton()
        {
            print("hide fart button");
            this.gameObject.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            this.gameObject.GetComponent<Button>().enabled = false;
            this.gameObject.GetComponent<Image>().enabled = false;
        }
        
        private IEnumerator AppearCoroutine(float appearTime, float hideTime){
            ShowFartButton();
            yield return new WaitForSecondsRealtime(appearTime);
            
            HideFartButton();
            yield return new WaitForSecondsRealtime(hideTime);
            
            _appearCoroutine = null;
        }
        
        // get a random position within canvas size
        private Vector3 GetRandomPosition(float width, float height)
        {
            return new Vector3(Random.Range(-width, width), Random.Range(-height, height), 0);
        }
        
        // TODO - resize button randomly

        public IEnumerator WaitForAudioCompletion(AudioClip audioClip)
        {
            yield return new WaitForSeconds(audioClip.length);
            DialogueManager.instance.GameOver("fart");
        }
        
    }
}