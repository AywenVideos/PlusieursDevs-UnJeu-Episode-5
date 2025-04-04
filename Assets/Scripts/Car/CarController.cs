using System.Collections;
using Managers;
using Managers.TaskManager;
using Player;
using TMPro;
using UnityEngine;

namespace Car {
    public class CarController : MonoBehaviour {
        [SerializeField] private float speed = 800f;
        [SerializeField] private float turnSpeed = 50f;

        [SerializeField] private float baseFov = 60f;
        [SerializeField] private float speedFov = 2f;
        [SerializeField] private float fovLerp = 2f;

        [SerializeField] public Camera camera;
        [SerializeField] public Camera carCamera;
        [SerializeField] private TMP_Text subMessageHud;
        [SerializeField] private FPSController player;

        [SerializeField] private Transform exitPosition;

        [SerializeField] private TaskManager taskManager;

        [SerializeField] private GameObject frontLight;
        [SerializeField] private GameObject backLight;
        private bool _asRider;

        private bool _canExit;

        private Inputs _inputs;
        private bool _isBroken;
        private Vector3 _lastPosition;
        private Rigidbody _rigidbody;
        private float _speed;

        public bool CanEnter { get; private set; } = true;

        public AudioSource sfxSource;
        public AudioSource enginenoiseSource;
        public AudioClip enterCar;
        public AudioClip exitCar;

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start() {
            _inputs = GameManager.Instance.Inputs;
            _inputs.Car.Enable();
        }

        private void FixedUpdate() {
            if (!_asRider) {
                return;
            }

            if (_inputs.Car.Exit.IsPressed() && _canExit && !GameManager.Instance.talkManager.isInDialog) {
                UnsetPlayerAsRider();
            }

            if (taskManager.Task.Id is TaskIdentifier.OpenHoodCar or TaskIdentifier.CloseCarHoddTask or TaskIdentifier.EngineReparedTask) {
                return;
            }

            Accelerate(_inputs.Car.Accelerate.ReadValue<float>());
            Steer(_inputs.Car.Steer.ReadValue<float>());

            float distance = Vector3.Distance(transform.position, _lastPosition);
            _speed = distance / Time.fixedDeltaTime;
            _lastPosition = transform.position;

            ChangeFov();

            backLight.SetActive(_speed < 0);
        }

        private IEnumerator ExitTime() {
            yield return new WaitForSeconds(1f);
            _canExit = true;
        }

        private IEnumerator EnterTime() {
            yield return new WaitForSeconds(1f);
            CanEnter = true;

        }

        private void Accelerate(float amount) {
            var force = speed * Mathf.Clamp(amount, -1f, 1f) * Time.deltaTime * transform.forward;
            _rigidbody.AddForce(force, ForceMode.Acceleration);
        }

        private float currentRotation = 0f;

        private void Steer(float amount)
        {
            float speedFactor = Mathf.Clamp01((speed - 0.01f) * 1);
            float targetRotation = amount * turnSpeed * speedFactor;

            currentRotation = Mathf.Lerp(currentRotation, targetRotation, Time.fixedDeltaTime * 5f);

            currentRotation *= Mathf.Sign(Vector3.Dot(_rigidbody.linearVelocity, transform.forward));

            var deltaRotation = Quaternion.Euler(0f, currentRotation * Time.fixedDeltaTime, 0f);
            _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
        }


        private void ChangeFov() {
            float fov = baseFov + _rigidbody.linearVelocity.magnitude * speedFov;
            carCamera.fieldOfView = Mathf.Lerp(carCamera.fieldOfView, fov, Time.fixedDeltaTime * fovLerp);
        }

        public void SetPlayerAsRider() {
            sfxSource.PlayOneShot(enterCar);
            
            if (taskManager.Task.Id == TaskIdentifier.EnterFirstTimeInCar) {
                taskManager.CompleteTask();
                _isBroken = true;
            }

            enginenoiseSource.Play();
            
            _asRider = true;
            CanEnter = false;

            player.gameObject.SetActive(false);

            carCamera.gameObject.SetActive(true);
            camera.gameObject.SetActive(false);

            StartCoroutine(ExitTime());

            if (!_isBroken) {
                frontLight.SetActive(true);
            }
        }

        private void UnsetPlayerAsRider() {
            sfxSource.PlayOneShot(exitCar);
            enginenoiseSource.Stop();

            _asRider = false;
            _canExit = false;

            StartCoroutine(EnterTime());

            player.gameObject.SetActive(true);
            player.transform.position = exitPosition.position;

            carCamera.gameObject.SetActive(false);
            camera.gameObject.SetActive(true);
        }
    }
}