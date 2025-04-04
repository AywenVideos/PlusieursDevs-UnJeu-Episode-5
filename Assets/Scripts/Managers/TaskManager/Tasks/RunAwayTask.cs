namespace Managers.TaskManager.Tasks
{
    public class RunAwayTask : Task
    {
        public RunAwayTask(TaskIdentifier id, TaskIdentifier nextId) : base(id, nextId) { }

        public override string Name => "Faille Système Détectée ! COUREZ";

        public override void OnComplete()
        {

        }
    }
}
