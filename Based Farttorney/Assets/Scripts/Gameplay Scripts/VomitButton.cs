using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay_Scripts
{
    public class VomitButton : MonoBehaviour
    {

        public Button moveButton;
        public float moveSpeed = 50f;
        public AudioClip vomitSfx;

        private Vector2 _randomPosition;

        private Coroutine _waitCoroutine;

        void Start()
        {
            _randomPosition = new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height));
            
            if (moveButton == null)
            {
                moveButton = GetComponent<Button>();
            }
        }

        private void Update()
        {
            MoveButtonRandomly();
        }

        public void VomitButtonPressed()
        {
            this.gameObject.GetComponent<AudioSource>().PlayOneShot(vomitSfx);
            if (_waitCoroutine == null)
            {
                _waitCoroutine = StartCoroutine(WaitForAudioCompletion(vomitSfx));
            }
        }

        private void MoveButtonRandomly()
        {
            // Get the RectTransform component of the button
            RectTransform rectTransform = moveButton.GetComponent<RectTransform>();

            // if (Random.Range(0, 1) < 0.000000000000001)
            // {
            //     _randomPosition = new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height));
            //     print(_randomPosition);
            // }
            
            if (Vector2.Distance(rectTransform.position, _randomPosition) < 1f)
            {
                _randomPosition = new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height));
            }
            
            // Move the button towards the random position
            rectTransform.position =
                Vector2.MoveTowards(rectTransform.position, _randomPosition, moveSpeed * Time.deltaTime);

            // Optionally, you can check if the button is close to the target position and set a new random position
            
        }

        private IEnumerator WaitForAudioCompletion(AudioClip audioClip)
        {
            yield return new WaitForSeconds(audioClip.length);
            _waitCoroutine = null;
            DialogueManager.instance.GameOver("vomit");
        }
    }
}