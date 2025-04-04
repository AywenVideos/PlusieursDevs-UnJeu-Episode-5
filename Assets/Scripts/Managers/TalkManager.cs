using System.Collections;
using Player;
using TMPro;
using UnityEngine;

namespace Managers {
    public class TalkManager : MonoBehaviour {
        [SerializeField] private TMP_Text subMessageHud;
        [SerializeField] private FPSController fpsController;
        [SerializeField] private TMP_Text subMessageInput;

        public float typingSpeed = 0.025f;
        public string[] dialogues = { };
        public bool isTyping;
        public bool isInDialog;
        private int _currentDialogueIndex;
        public AudioSource audioSource;
        public AudioClip TalkSFX;

        private Coroutine _typingCoroutine;

        public void Update() {
            if (!Input.GetKeyDown(KeyCode.Space)) {
                return;
            }

            if (isTyping) {
                StopCoroutine(_typingCoroutine);
                subMessageHud.text = dialogues[_currentDialogueIndex];
                isTyping = false;
            } else {
                _currentDialogueIndex++;
                if (_currentDialogueIndex < dialogues.Length) {
                    StartDialogue();
                } else {
                    EndDialogue();
                }
            }
        }

        public void StartDialogue() {
            subMessageHud.gameObject.SetActive(true);
            subMessageInput.gameObject.SetActive(true);
            fpsController.enabled = false;
            isInDialog = true;
            _typingCoroutine = StartCoroutine(TypeSentence(dialogues[_currentDialogueIndex]));
        }

        public IEnumerator TypeSentence(string sentence) {
            isTyping = true;
            subMessageHud.text = "";
            foreach (char letter in sentence) {
                subMessageHud.text += letter;
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.PlayOneShot(TalkSFX);
                yield return new WaitForSeconds(typingSpeed);
                subMessageInput.gameObject.SetActive(false);
            }
            isTyping = false;
            subMessageInput.gameObject.SetActive(true);
        }

        public void EndDialogue() {
            _currentDialogueIndex = 0;
            isInDialog = false;
            subMessageHud.text = "";
            fpsController.enabled = true;
            subMessageInput.gameObject.SetActive(false);
        }
    }
}