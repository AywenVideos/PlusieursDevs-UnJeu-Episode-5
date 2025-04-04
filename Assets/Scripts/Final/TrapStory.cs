using Managers;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrapStory : MonoBehaviour
{
    public DialogueManager.Dialogue[] dialoguesONE, dialoguesTWO, dialoguesTHREE;
    public GameObject _exitButton;

    private void Start()
    {
        StartCoroutine(StoryCoroutine());
        _exitButton.SetActive(false);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    IEnumerator StoryCoroutine()
    {
        if (GameManager.Position == 0)
        {
            yield return new WaitForSeconds(3f);
            yield return DialogueManager.instance.ShowDialogues(dialoguesONE);

            GameManager.Position = 1;

            SceneManager.LoadScene("Game");
        }
        else if (GameManager.Position == 1)
        {
            yield return DialogueManager.instance.ShowDialogues(dialoguesTWO); ;

            GameManager.Position = 2;

            SceneManager.LoadScene("Ecosim");
        }
        else if (GameManager.Position == 2)
        {
            yield return DialogueManager.instance.ShowDialogues(dialoguesTHREE); ;
            _exitButton.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
