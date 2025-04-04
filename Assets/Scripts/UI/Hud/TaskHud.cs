using Managers;
using Managers.TaskManager;
using TMPro;
using UnityEngine;

namespace UI.Hud {
    public class TaskHud : MonoBehaviour {
        [SerializeField] private TaskManager taskManager;
        [SerializeField] private TalkManager talkManager;

        private TMP_Text _text;

        private void Awake() {
            _text = GetComponent<TMP_Text>();
        }

        private void FixedUpdate() {
            if (talkManager.isInDialog) {
                _text.text = "";
            }
            _text.text = taskManager.Task.Name;
        }
    }
}