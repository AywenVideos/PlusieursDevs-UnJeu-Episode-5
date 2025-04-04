namespace Managers.TaskManager.Tasks {
    public class GoToTheStoreTask : Task {

        public GoToTheStoreTask(TaskIdentifier id, TaskIdentifier nextId) : base(id, nextId) { }

        public override string Name => GameManager.Position == 0 ? "Va au travail" : "VA AU TRAVAIl, VA AU TRAVAIL ???";

        public override void OnComplete() { }
    }
}