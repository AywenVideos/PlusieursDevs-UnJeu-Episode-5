using UnityEngine;
using Managers.TaskManager;
using Managers;
using TMPro;
using UnityEngine.InputSystem;
using System;

public class OpenDoorScript : MonoBehaviour
{
    public TaskIdentifier task;
    public bool isTask;
    public Animator _animator;
    public TMP_Text subHudText;
    public string _interactText;
    [SerializeField] private TaskManager taskManager;
    private Inputs _inputs;

    private void Start()
    {
        _inputs = GameManager.Instance.Inputs;
        _inputs.FPS.Enable();

        OnInteract = _ => {
            SetUp();
        };
    }

    public event Action<InputAction.CallbackContext> OnInteract;

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(subHudText.gameObject.activeSelf)
            {
                return;
            }

            subHudText.gameObject.SetActive(true);
            subHudText.SetText(_interactText);
            _inputs.FPS.Interact.performed += OnInteract;
        }
    }

    void SetUp()
    {
        if (isTask)
        {
            if (taskManager.Task.Id == task)
            {
                taskManager.CompleteTask();
            }
            else
            {
                return;
            }
        }

        _animator.SetTrigger("Open");
        _inputs.FPS.Interact.performed -= OnInteract;
    }

    private void OnTriggerExit(Collider other)
    {
        subHudText.gameObject.SetActive(false);
        subHudText.SetText("");
    }
}
