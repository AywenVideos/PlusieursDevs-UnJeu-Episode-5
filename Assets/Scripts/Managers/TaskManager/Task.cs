namespace Managers.TaskManager {
    public abstract class Task {

        public Task(TaskIdentifier id, TaskIdentifier nextId) {
            Id = id;
            NextId = nextId;
        }

        public TaskIdentifier Id { get; private set; }

        public TaskIdentifier NextId { get; private set; }

        public abstract string Name { get; }

        public abstract void OnComplete();
    }
}