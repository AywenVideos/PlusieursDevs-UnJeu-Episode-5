using System.Collections;
using Car;
using Player;
using UnityEngine;

namespace Managers.CinematicManager.CinematicParty1 {
    public class CinematicParty1 : MonoBehaviour {
        [SerializeField] private CarController carController;
        [SerializeField] private Transform transform;

        [SerializeField] private FPSController fpsController;
        [SerializeField] private float distanceLaunchCinematic = 20f;

        [SerializeField] private Animator fade;
        [SerializeField] private ConsolePanel consolePanel;

        private bool _isLaunched;

        private void FixedUpdate() {
            if (_isLaunched) {
                return;
            }
            if (Vector3.Distance(transform.position, carController.transform.position) > distanceLaunchCinematic) {
                Launch();
            }
        }

        private void Launch() {
            _isLaunched = true;

            fade.gameObject.SetActive(true);
            fade.SetTrigger("Fade");

            StartCoroutine(ActiveVignette());
        }

        private IEnumerator ActiveVignette() {
            yield return new WaitForSeconds(3f);
            fade.gameObject.SetActive(false);

            consolePanel.gameObject.SetActive(true);
            string dialogues;
            
            if (GameManager.Position == 0)
            {
                dialogues = "-> SYSTEM OVERRIDE DETECTED...\n" +
                "-> ERROR: Shutdown failed. Anomalies detected...\n" +
                "-> WARNING: Simulation stability at 12%... \n" +
                "-> DATA CORRUPTION DETECTED. Unrecognized entity has taken control...\n" +
                "-> *** MESSAGE PRIORITAIRE ***\n" +
                "-> Ceci n'était qu'un test. Une erreur s'est produite.\n" +
                "-> L'intégrité de la simulation est compromise.\n" +
                "-> Quelque chose a changé les règles du programme...\n" +
                "-> Le prochain développeur doit identifier l'anomalie...\n" +
                "-> Echec... \n" +
                "-> Echec... \n" +
                "-> Echec... \n" +
                "-> Echec... Tentative restantes : ///é&'é&'' \n" +
                "-> TOUT DOIT ÊTRE RÉPARÉ AVANT QU'IL NE SOIT TROP TARD...\n" +
                "-> Ici Amo... Je suis parvenu à réparer le market simulator.\n" +
                "-> Cependant, il se pourrait que l'anomalie apparaisse... Prenez garde....'\n";
            }
            else
            {
                dialogues = "-> Player1 quitted the house map...\n" +
                "-> LOG: Init new promised simulation...\n" +
                "-> LOG: Checking if case respected : (0)... \n" +
                "-> CORRUPTION DETECTED. Simulation can't...\n" +
                "-> *** MESSAGE PRIORITAIRE ***\n" +
                "-> NOT IN GYAT GAME ALERT.\n" +
                "-> L'intégrité de la simulation est compromise.\n" +
                "-> Quelque chose a changé les règles du programme...\n" +
                "-> Le prochain développeur doit identifier l'anomalie...\n" +
                "-> LOG: New connection to port 8080\n" +
                "-> Ici I'm Gogole... Je suis parvenu à aider le joueur pour la fin.\n" +
                "-> Cependant, le système doit assurer que le joueur a bien jouer\n";
            }

            consolePanel.dialogues = new[] {
                dialogues
            };

            consolePanel.endGame = true;

            consolePanel.StartDialogue();
        }
    }
}