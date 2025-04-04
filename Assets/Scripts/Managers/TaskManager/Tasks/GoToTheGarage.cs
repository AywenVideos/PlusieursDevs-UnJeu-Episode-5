namespace Managers.TaskManager.Tasks
{
    public class GoToTheGarage : Task
    {
        public GoToTheGarage(TaskIdentifier id, TaskIdentifier nextId) : base(id, nextId) { }

        public override string Name => "Ouvre la porte pour aller au garage";

        public override void OnComplete()
        {
            string[] lines;

            if (GameManager.Position == 0)
            {
                lines = new[]
                {
                    "J'esp�re que je serai pas en retard cette fois",
                    "Enfin, je suis pay� � jouer � ce jeu alors je me plains pas"
                };
            }
            else
            {
                lines = new[]
                {
                    "Pourquoi le syst�me d�conne tout � coup ?!",
                    "C'est qui ce nouvel admin puchican ?",
                    "Je suppose que je dois quand m�me prendre la voiture"
                };
            }

            GameManager.Instance.talkManager.dialogues = lines;
            GameManager.Instance.talkManager.StartDialogue();
        }
    }
}
