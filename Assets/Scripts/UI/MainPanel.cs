using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UI {
    public class MainMenu : MonoBehaviour {

        [SerializeField] private GameObject helpPanel;
        [SerializeField] private GameObject lorePanel;
        private GameManager gameManager;

        private void Start() {
            gameManager = GameManager.Instance;

            if (gameManager)
            {
                gameManager.Inputs.Settings.Pause.performed += StartGame;
            }

            Time.timeScale = 1f;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            AudioListener.pause = false;
        }

        private void OnDestroy() {
            if(gameManager) gameManager.Inputs.Settings.Pause.performed -= StartGame;
        }

        private void StartGame(InputAction.CallbackContext _) {
            StartGame();
        }

        public void StartGame() {
            SceneManager.LoadScene("Game");
        }

        public void QuitGame() {
            Application.Quit();
        }

        public void ToogleHelp() {
            helpPanel.SetActive(!helpPanel.activeSelf);
        }

        public void LorePanel()
        {
            lorePanel.SetActive(!lorePanel.activeSelf);
        }
    }
}