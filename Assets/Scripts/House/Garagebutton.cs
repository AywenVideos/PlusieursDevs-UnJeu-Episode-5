using System;
using Managers;
using Managers.TaskManager;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Car {
    public class GarageButton : MonoBehaviour {

        [SerializeField] private Animator animator;
        [SerializeField] private TMP_Text subMessageHud;
        [SerializeField] private Camera camera;
        [SerializeField] private EngineQteReparationCar engineQteReparationCar;
        [SerializeField] private TaskManager taskManager;

        [SerializeField] private GameObject _redLight;
        [SerializeField] private GameObject _greenLight;

        [SerializeField] private Animator _garageDoorAnimator;
        private Inputs _inputs;
        private bool _isOn;

        public string _interactText;
        public TaskIdentifier task;
        private bool _onInteract;

        private void Start() {
            _inputs = GameManager.Instance.Inputs;
            _inputs.FPS.Enable();

            OnInteract = _ => {
                SetOn();
            };
        }

        private void FixedUpdate() {
            if (task == TaskIdentifier.OpenGarageDoorTask &&  taskManager.Task.Id is not TaskIdentifier.OpenGarageDoorTask) {
                return;
            }

            var cameraTransform = camera.transform;
            var ray = new Ray(cameraTransform.position, cameraTransform.forward);

            if (!Physics.Raycast(ray, out var hit, 0.5f)) {
                return;
            }

            if (hit.collider.gameObject == gameObject) {
                if (subMessageHud.gameObject.activeSelf) {
                    return;
                }

                if (_isOn) {
                    return;
                }

                subMessageHud.gameObject.SetActive(true);
                subMessageHud.SetText(_interactText);

                if (_onInteract) {
                    return;
                }

                _onInteract = true;
                _inputs.FPS.Interact.performed += OnInteract;
                return;
            }

            if (!subMessageHud.gameObject.activeSelf) {
                return;
            }

            subMessageHud.gameObject.SetActive(false);
            subMessageHud.SetText("");
        }

        public event Action<InputAction.CallbackContext> OnInteract;

        private void SetOn() {

            _greenLight.SetActive(true);
            _redLight.SetActive(false);

            _garageDoorAnimator.SetTrigger("Open");

            _isOn = true;
            animator?.SetBool("IsOn", true);
            _inputs.FPS.Interact.performed -= OnInteract;
            _onInteract = false;

            if (taskManager.Task.Id == task) {
                taskManager.CompleteTask();
            }
        }
    }
}