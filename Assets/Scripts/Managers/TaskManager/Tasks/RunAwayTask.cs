namespace Managers.TaskManager.Tasks
{
    public class RunAwayTask : Task
    {
        public RunAwayTask(TaskIdentifier id, TaskIdentifier nextId) : base(id, nextId) { }

        public override string Name => "Faille Syst�me D�tect�e ! COUREZ";

        public override void OnComplete()
        {

        }
    }
}
