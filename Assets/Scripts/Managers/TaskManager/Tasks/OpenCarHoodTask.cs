namespace Managers.TaskManager.Tasks {
    public class OpenCarHoodTask : Task {
        public OpenCarHoodTask(TaskIdentifier id, TaskIdentifier nextId) : base(id, nextId) { }

        public override string Name => "Ouvre le capot de la voiture";

        public override void OnComplete() { }
    }
}