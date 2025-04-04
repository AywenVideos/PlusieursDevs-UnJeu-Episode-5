using Managers;
using Managers.TaskManager;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Car {
    public class EngineQteReparationCar : MonoBehaviour {
        [SerializeField] private Slider progressBar;
        [SerializeField] private float decreaseRate = 5f;
        [SerializeField] private float increaseAmount = 10f;
        [SerializeField] private float successThreshold = 100f;
        [SerializeField] private FPSController fpsController;
        [SerializeField] private TaskManager taskManager;

        private Inputs _inputs;
        private float _progress;

        public bool IsActive { get; private set; }

        private void Start() {
            _inputs = GameManager.Instance.Inputs;
            _inputs.FPS.Enable();
            progressBar.value = 0f;
        }

        private void Update() {
            if (!IsActive) {
                return;
            }

            _progress -= decreaseRate * Time.deltaTime;
            _progress = Mathf.Clamp(_progress, 0f, successThreshold);

            if (_inputs.FPS.Interact.triggered) {
                _progress += increaseAmount;
                _progress = Mathf.Clamp(_progress, 0f, successThreshold);
            }

            progressBar.value = _progress / successThreshold;

            if (_progress >= successThreshold) {
                Complete();
            }
        }

        public void Launch() {
            IsActive = true;
            _progress = 0;

            fpsController.Freeze = true;
            progressBar.gameObject.SetActive(true);
        }

        private void Complete() {
            if (taskManager.Task.Id == TaskIdentifier.EngineReparedTask) {
                taskManager.CompleteTask();
            }

            IsActive = false;

            fpsController.Freeze = false;
            progressBar.gameObject.SetActive(false);
        }
    }
}