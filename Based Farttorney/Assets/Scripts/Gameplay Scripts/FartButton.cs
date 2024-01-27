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

        private Coroutine appearCoroutine;
        
        private void Update()
        {
            if (Random.Range(0, 1) < 0.2 && appearCoroutine == null)
            {
                appearCoroutine = StartCoroutine(AppearCoroutine(Random.Range(minAppearDuration, maxAppearDuration)));
                ShowFartButton();
            }
        }
        
        public void FartButtonPressed()
        {
            this.gameObject.GetComponent<AudioSource>().PlayOneShot(fartSfx);
        }

        private void ShowFartButton()
        {
            this.transform.localPosition = GetRandomPosition(960f, 540f);
            this.gameObject.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            this.gameObject.GetComponent<Button>().enabled = true;
            this.gameObject.GetComponent<Image>().enabled = true;
        }
        private void HideFartButton()
        {
            // this.transform.localPosition = GetRandomPosition(1920f, 1080f);
            this.gameObject.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            this.gameObject.GetComponent<Button>().enabled = false;
            this.gameObject.GetComponent<Image>().enabled = false;
        }
        
        private IEnumerator AppearCoroutine(float seconds){
            print("appear coroutine");
            ShowFartButton();
            yield return new WaitForSecondsRealtime(seconds);
            appearCoroutine = null;
        }
        
        // get a random position within canvas size
        private Vector3 GetRandomPosition(float width, float height)
        {
            return new Vector3(Random.Range(-width, width), Random.Range(-height, height), 0);
        }

        
        
    }
}