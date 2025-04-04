using System;
using System.Collections;
using Managers;
using Managers.TaskManager;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Car {
    public class DoorController : MonoBehaviour {
        [SerializeField] public Camera camera;
        [SerializeField] private TMP_Text subMessageHud;
        [SerializeField] private CarController carController;
        [SerializeField] private TaskManager taskManager;
        [SerializeField] private TalkManager talkManager;

        private bool _asRider;
        private bool _delay = true;
        private bool _inDelay;
        private Inputs _inputs;
        private bool _lookDoor;

        private void Start() {
            _inputs = GameManager.Instance.Inputs;
            _inputs.FPS.Enable();
        }

        private void FixedUpdate() {
            if (talkManager.isInDialog) {
                return;
            }

            if (taskManager.Task.Id is TaskIdentifier.OpenHoodCar or TaskIdentifier.CloseCarHoddTask or TaskIdentifier.EngineReparedTask or TaskIdentifier.OpenGarageDoorTask) {
                subMessageHud.gameObject.SetActive(false);
                _lookDoor = false;
                return;
            }

            if (_lookDoor && _inputs.FPS.Interact.IsPressed()) {
                if (!carController.CanEnter) {
                    return;
                }
                carController.SetPlayerAsRider();
                return;
            }

            var cameraTransform = camera.transform;
            var ray = new Ray(cameraTransform.position, cameraTransform.forward);

            if (!Physics.Raycast(ray, out var hit, 2f)) {
                return;
            }

            if (hit.collider.gameObject == gameObject) {
                if (subMessageHud.gameObject.activeSelf) {
                    return;
                }

                _lookDoor = true;
                subMessageHud.gameObject.SetActive(true);
                subMessageHud.SetText("Appuyez sur E pour conduire");
                return;
            }

            if (!subMessageHud.gameObject.activeSelf) {
                return;
            }

            _lookDoor = false;
            subMessageHud.gameObject.SetActive(false);
            subMessageHud.SetText("");
        }

        public event Action<InputAction.CallbackContext> OnInteract;

        private IEnumerator Delay() {
            yield return new WaitForSeconds(2f);
            _delay = false;
        }
    }
}