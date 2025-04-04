using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sign : MonoBehaviour
{
    public TalkManager talkManager;

    public bool touched = false;

    private void OnTriggerEnter(Collider other)
    {
        if (touched) return;

        if (other.CompareTag("Player"))
        {
            touched = true;

            Invoke("Next", 1f);

            talkManager.dialogues = new string[] { "Trouvé!" }; 
            talkManager.StartDialogue();
        }
    }

    private void Next()
    {
        SceneManager.LoadScene("Coms");
    }
}
