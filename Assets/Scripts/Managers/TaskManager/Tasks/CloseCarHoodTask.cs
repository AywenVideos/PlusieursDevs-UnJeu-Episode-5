namespace Managers.TaskManager.Tasks {
    public class CloseCarHoodTask : Task {

        public CloseCarHoodTask(TaskIdentifier id, TaskIdentifier nextId) : base(id, nextId) { }

        public override string Name => "Ferme le capot de la voiture";

        public override void OnComplete() { }
    }
}