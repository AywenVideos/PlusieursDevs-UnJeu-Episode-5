using System;
using Managers;
using UnityEngine;

namespace Player {
    public class FPSController : MonoBehaviour {

        [Header("Movement Settings")] public float walkSpeed = 5.0f;

        public float runSpeed = 8.0f;
        public float jumpForce = 5.0f;
        public float gravity = 20.0f;

        [Header("Look Settings")] public float mouseSensitivity = 2.0f;

        public float gamepadLookSensitivity = 2.0f;
        public float lookUpLimit = 80.0f;
        [SerializeField] public Vector3 offsetCameraCar = new(0, 2, 0);
        [SerializeField] public float rotationCameraCar;

        public CharacterController characterController;
        [SerializeField] public Collider playerCollider;

        [Header("Game Over")] [SerializeField] private float deathZoneY;

        private GameObject car;
        private Inputs inputs;
        private Vector3 moveDirection = Vector3.zero;
        private Camera playerCamera;
        private float rotationX;

        [Header("Driving")]
        public bool IsDriving { get; set; }

        public bool IsDead { get; private set; }

        public bool Freeze { get; set; }

        private void Start() {
            inputs = GameManager.Instance.Inputs;
            inputs.FPS.Enable();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            characterController = GetComponent<CharacterController>();
            playerCamera = GetComponentInChildren<Camera>();
        }

        private void Update() {
            if (Freeze) {
                return;
            }

            //if (IsDead) return;

            if (inputs.FPS.enabled) {
                HandleLook();
            }


            HandlePlayerMovement();

            if (transform.position.y <= deathZoneY) {
                GameOver?.Invoke();
                IsDead = true;
            }
        }

        public event Action GameOver;

        private void HandleLook() {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            var gamepadLook = inputs.FPS.Look.ReadValue<Vector2>() * gamepadLookSensitivity;
            mouseX += gamepadLook.x;
            mouseY += gamepadLook.y;

            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -lookUpLimit, lookUpLimit);

            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, mouseX, 0);
        }

        private void HandlePlayerMovement() {
            bool isRunning = inputs.FPS.Run.IsPressed();
            float currentSpeed = isRunning ? runSpeed : walkSpeed;

            var move = inputs.FPS.Move.ReadValue<Vector2>() * currentSpeed;

            var movement = transform.forward * move.y + transform.right * move.x;

            if (characterController.isGrounded) {
                moveDirection.y = -0.5f; // Small downward force when grounded
                if (inputs.FPS.Jump.WasPerformedThisFrame()) {
                    moveDirection.y = jumpForce;
                }
            } else {
                moveDirection.y -= gravity * Time.deltaTime;
            }

            movement.y = moveDirection.y;
            moveDirection = movement;

            characterController.Move(moveDirection * Time.deltaTime);
        }
    }
}