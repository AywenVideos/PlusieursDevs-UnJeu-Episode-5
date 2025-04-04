namespace Managers.TaskManager.Tasks {
    public class EngineReparedTask : Task {

        public EngineReparedTask(TaskIdentifier id, TaskIdentifier nextId) : base(id, nextId) { }

        public override string Name => "Repare le moteur de la voiture";

        public override void OnComplete() { }
    }
}