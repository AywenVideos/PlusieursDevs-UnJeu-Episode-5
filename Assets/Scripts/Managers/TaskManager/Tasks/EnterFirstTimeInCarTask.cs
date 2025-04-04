namespace Managers.TaskManager.Tasks {
    public class EnterFirstTimeInCarTask : Task {
        public EnterFirstTimeInCarTask(TaskIdentifier id, TaskIdentifier nextId) : base(id, nextId) { }

        public override string Name => "Entre dans ta voiture";

        public override void OnComplete() {
            GameManager.Instance.talkManager.dialogues = new[] {
                "Raaah… encore un problème avec ce moteur"
            };
            GameManager.Instance.talkManager.StartDialogue();
        }
    }
}