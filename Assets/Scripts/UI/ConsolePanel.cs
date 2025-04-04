using System.Collections;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

namespace Managers {
    public class ConsolePanel : MonoBehaviour {
        [SerializeField] private TMP_Text subMessageHud;
        [SerializeField] private FPSController fpsController;
        [SerializeField] private TalkManager talkManager;

        public float typingSpeed = 0.02f;
        public string[] dialogues;
        public bool isTyping;
        public bool isInDialog;
        public AudioClip consoleMusic;
        public AudioClip realityMusic;

        public bool endGame;
        private int _currentDialogueIndex;

        private Coroutine _typingCoroutine;

        public AudioMixer audioMixer;

        private const float MuteVolume = -80f;
        private const float NormalVolume = 0f;

        private void Start() {
            if (GameManager.Position == 0)
            {
                dialogues = new[] {
                "-> Simulation V.1.4 Initializing...\n" +
                "-> Launching simulation... Please wait.\n" +
                "-> ERROR 404: System failure... Something is wrong...\n" +
                "-> Cry protocol activated... But it's not your own...\n" +
                "-> Reflex system online... But are you really in control?\n" +
                "-> Wake up, cowboy... The nightmare begins now...\n" +
                "-> WARNING: Disturbance detected. You are not alone.\n" +
                "-> SYSTEM ERROR: Unknown anomaly in the core.\n" +
                "-> Preparing for immersion... Don't close your eyes.\n" +
                "-> Uncontrollable variables engaged... The boundaries are blurring.\n" +
                "-> All systems normal... Or is it? Proceed with caution...\n" +
                "-> What is the name puchican for ??? \n"
                };
            }
            else
            {
                dialogues = new[] {
                "-> Simulation V.1.3 Initializing...\n" +
                "-> Launching simulation... Please wait.\n" +
                "-> Détection de tentative : [2]\n" +
                "-> Terminal will stop logging since (attempt > 1) returns true \n"
                };
            }


            StartDialogue();

        }

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
            MusicManager.Instance.SetVolume(1.0f);
            MusicManager.Instance.PlayMusic(consoleMusic);
            audioMixer.SetFloat("RealityVolume", MuteVolume);

            subMessageHud.gameObject.SetActive(true);
            fpsController.enabled = false;
            isInDialog = true;
            _typingCoroutine = StartCoroutine(TypeSentence(dialogues[_currentDialogueIndex]));
        }

        private IEnumerator TypeSentence(string sentence) {
            isTyping = true;
            subMessageHud.text = "";
            foreach (char letter in sentence) {
                subMessageHud.text += letter;
                yield return new WaitForSeconds(typingSpeed);
                if (letter == '\n') yield return new WaitForSeconds(0.2f);
            }
            isTyping = false;
            EndDialogue();
            yield return new WaitForSeconds(3f);
        }

        private void EndDialogue() {
            if (endGame)
            {

                if (GameManager.Position == 0)  SceneManager.LoadScene("Store");
                else
                {
                    GameManager.Position = 2;
                    SceneManager.LoadScene("Coms");
                }
                return;
            }

            _currentDialogueIndex = 0;
            isInDialog = false;
            subMessageHud.text = "";
            fpsController.enabled = true;

            MusicManager.Instance.SetVolume(0.1f);
            MusicManager.Instance.PlayMusic(realityMusic);
            audioMixer.SetFloat("RealityVolume", MuteVolume);

            gameObject.SetActive(false);

            if (GameManager.Position == 0)
            {
                talkManager.dialogues = new[] {
                "Haaa !!!",
                "C'était quoi ce rêve... ?",
                "Bon, je bosse de nuit ce soir.",
                "Je ferais mieux d'y aller."
            };
            }
            else
            {
                talkManager.dialogues = new[] { 
                    "Pas le droit à l'echec", 
                    "Je dois trouver autre chose...",
                    "Mon patron va me détester si je ne finis pas la simulation du lundi"
                }; 
            }

            talkManager.StartDialogue();
        }
    }
}