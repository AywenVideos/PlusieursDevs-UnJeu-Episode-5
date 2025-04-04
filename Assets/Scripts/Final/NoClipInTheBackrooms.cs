using UnityEngine;
using Managers;
using Managers.TaskManager;

public class NoClipInTheBackrooms : MonoBehaviour
{
    public GameObject[] _toDisable;
    [SerializeField] private TaskManager taskManager;

    private void Start()
    {
        if(GameManager.Position == 1)
        {
            Initialize();
        }
    }

    private void Update()
    {
        if(taskManager.Task.Id is TaskIdentifier.RunTask && GameManager.Position != 1)
        {
            taskManager.CompleteTask();
        }
    }

    private void Initialize()
    {
        foreach (var item in _toDisable)
        {
            item.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (taskManager.Task.Id is not TaskIdentifier.GoToTheStoreTask)
        {
            return;
        }

        taskManager.CompleteTask();
    }
}
