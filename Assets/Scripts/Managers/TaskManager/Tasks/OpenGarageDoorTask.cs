namespace Managers.TaskManager.Tasks {
    public class OpenGarageDoorTask : Task {

        public OpenGarageDoorTask(TaskIdentifier id, TaskIdentifier nextId) : base(id, nextId) { }

        public override string Name => "Ouvre la porte du garage";

        public override void OnComplete() {

            string[] lines;
            
            if (GameManager.Position == 0)
            {
                lines = new[]
                {
                    "En route pour le travail je suppose"
                };  
            }
            else
            {
                lines = new[]
                {
                    "Qu'est-ce que je suis censé faire pour avoir cette nouvelle simulation ?",
                    "Je l'aurais vraiment en conduisant à nouveau ?"
                };
            }

            GameManager.Instance.talkManager.dialogues = lines;
            GameManager.Instance.talkManager.StartDialogue();
        }
    }
}