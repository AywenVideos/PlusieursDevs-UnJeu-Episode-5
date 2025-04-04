using System.Collections.Generic;
using System.Linq;
using Managers.TaskManager.Tasks;
using UnityEngine;

namespace Managers.TaskManager {
    public class TaskManager : MonoBehaviour {
        private readonly Dictionary<TaskIdentifier, Task> _tasks = new();

        public Task Task { get; set; }

        public AudioClip[] taskCompletionSFX;
        public AudioSource audioSource;


        private void Awake() {
            InitTask();
            Task = _tasks.FirstOrDefault().Value;
        }

        private void FixedUpdate() {
            Debug.Log(Task.Id);
        }

        private void InitTask() {
            AddTask(new GoToTheGarage(TaskIdentifier.GoToTheGarage, TaskIdentifier.EnterFirstTimeInCar));
            AddTask(new EnterFirstTimeInCarTask(TaskIdentifier.EnterFirstTimeInCar, TaskIdentifier.OpenHoodCar));
            AddTask(new OpenCarHoodTask(TaskIdentifier.OpenHoodCar, TaskIdentifier.EngineReparedTask));
            AddTask(new EngineReparedTask(TaskIdentifier.EngineReparedTask, TaskIdentifier.CloseCarHoddTask));
            AddTask(new CloseCarHoodTask(TaskIdentifier.CloseCarHoddTask, TaskIdentifier.OpenGarageDoorTask));
            AddTask(new OpenGarageDoorTask(TaskIdentifier.OpenGarageDoorTask, TaskIdentifier.GoToTheStoreTask));
            AddTask(new GoToTheStoreTask(TaskIdentifier.GoToTheStoreTask, TaskIdentifier.RunTask));
            AddTask(new RunAwayTask(TaskIdentifier.RunTask, TaskIdentifier.EndTask));
        }

        public void CompleteTask() {
            Task.OnComplete();

            int taskIndex = (int)Task.Id;
            if (taskIndex >= 0 && taskIndex < taskCompletionSFX.Length) {
                audioSource.PlayOneShot(taskCompletionSFX[taskIndex]);
            }

            if (_tasks.TryGetValue(Task.NextId, out var nextTask)) {
                Task = nextTask;
            }
        }

        public void AddTask(Task task) {
            _tasks.Add(task.Id, task);
        }
    }
}