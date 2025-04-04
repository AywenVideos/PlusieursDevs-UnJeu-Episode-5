using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers {
    public class GameManager : MonoBehaviour {

        [SerializeField] public TalkManager talkManager;
        //*******************************************//
        // The Game Manager is present on all scenes //
        //     It must not be a child GameObject     //
        //*******************************************//

        public GameObject _pausePanel;
        int _cState;
        public bool _testing = false;

        public static GameManager Instance { get; private set; }
        public Inputs Inputs { get; private set; }

        public TaskManager.TaskManager TaskManager { get; private set; }

        public int _positionForTest = 0;
        public static int Position = 0;

        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
                return;
            }


            if (_testing) Position = _positionForTest;
            DontDestroyOnLoad(this);

            if (Inputs != null) Inputs.Disable();
            Inputs = new Inputs();
            Inputs.Settings.Enable();

            _pausePanel.SetActive(false);
            TaskManager = gameObject.AddComponent<TaskManager.TaskManager>();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.P))
            {
                LoadEcosimScene();
            }

            if(Inputs.Settings.Pause.triggered)
            {
                PauseGame(_cState == 1 ? 2 : 1);
            }
        }

        public void PauseGame(int state)
        {
            _cState = state;

            if(state == 1)
            {
                //Pause
                Time.timeScale = 0;
                _pausePanel.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
            } 
            else
            {
                //Resume
                Time.timeScale = 1;
                _pausePanel.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        public void MainMenu()
        {
            Position = 0;
            SceneManager.LoadSceneAsync("MainMenuScene");
        }

        void LoadEcosimScene()
        {
            SceneManager.LoadSceneAsync("Ecosim"); 
        }

        public void QuitGame() {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
        }

        private void OnDisable()
        {
            Inputs.Disable();
        }
    }
}