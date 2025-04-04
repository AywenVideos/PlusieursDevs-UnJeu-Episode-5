using System;
using Managers;
using Managers.TaskManager;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Car {
    public class HoodCar : MonoBehaviour {

        [SerializeField] private Animator animator;
        [SerializeField] private TMP_Text subMessageHud;
        [SerializeField] private Camera camera;
        [SerializeField] private EngineQteReparationCar engineQteReparationCar;
        [SerializeField] private TaskManager taskManager;

        private Inputs _inputs;
        private bool _inZone;
        private bool _isOpen;

        private void Start() {
            _inputs = GameManager.Instance.Inputs;
            _inputs.FPS.Enable();

            OnInteract = _ => {
                if (engineQteReparationCar.IsActive) {
                    return;
                }

                if (!_isOpen) {
                    ;
                    SetOpen();
                    return;
                }

                if (taskManager.Task.Id == TaskIdentifier.EngineReparedTask) {
                    engineQteReparationCar.Launch();
                    return;
                }

                SetClose();
            };
        }

        private void FixedUpdate() {
            if (taskManager.Task.Id is not (TaskIdentifier.OpenHoodCar or TaskIdentifier.EngineReparedTask or TaskIdentifier.CloseCarHoddTask)) {
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

                subMessageHud.gameObject.SetActive(true);

                switch (_isOpen) {
                    case false:
                        subMessageHud.SetText("Appuyez sur E pour ouvrir le capot");
                        break;
                    case true:
                        subMessageHud.SetText(taskManager.Task.Id == TaskIdentifier.EngineReparedTask
                            ? "Appuyez sur E pour commencer à réparer le moteur"
                            : "Appuyez sur E pour fermet le capot");
                        break;
                }

                return;
            }

            if (!subMessageHud.gameObject.activeSelf) {
                return;
            }

            subMessageHud.gameObject.SetActive(false);
            subMessageHud.SetText("");
        }

        private void OnTriggerEnter(Collider other) {
            if (!other.CompareTag("Player")) {
                return;
            }
            if (taskManager.Task.Id is not (TaskIdentifier.OpenHoodCar or TaskIdentifier.EngineReparedTask or TaskIdentifier.CloseCarHoddTask)) {
                return;
            }


            _inputs.FPS.Interact.performed += OnInteract;
        }

        private void OnTriggerExit(Collider other) {
            if (!other.CompareTag("Player")) {
                return;
            }

            _inputs.FPS.Interact.performed -= OnInteract;
        }

        public event Action<InputAction.CallbackContext> OnInteract;

        private void SetOpen() {
            _isOpen = true;
            animator.SetBool("CapotOpen", true);

            if (taskManager.Task.Id == TaskIdentifier.OpenHoodCar) {
                taskManager.CompleteTask();
            }
        }

        private void SetClose() {
            _isOpen = false;
            animator.SetBool("CapotOpen", false);


            if (taskManager.Task.Id == TaskIdentifier.CloseCarHoddTask) {
                taskManager.CompleteTask();
            }
        }
    }
}