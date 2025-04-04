using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [System.Serializable]
    public struct Dialogue
    {
        public Author author;
        public string text;
        public float delay;
    }

    public enum Author { Player, NPC, System }

    [Header("UI Elements")]
    public TMP_Text dialogueText;
    public Color playerColor = Color.cyan;
    public Color npcColor = Color.green;
    public Color systemColor = Color.yellow;
    public float textSpeed = 0.05f;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip typingSound;

    private Coroutine typingCoroutine;

    public static DialogueManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public Coroutine ShowDialogues(Dialogue[] dialogues)
    {
        StopAllCoroutines();
        return StartCoroutine(DisplayDialogues(dialogues));
    }

    private IEnumerator DisplayDialogues(Dialogue[] dialogues)
    {
        foreach (Dialogue dialogue in dialogues)
        {
            SetDialogueUI(dialogue);
            yield return StartCoroutine(TypeText(dialogue.text));
            yield return new WaitForSeconds(dialogue.delay);
        }
        dialogueText.text = "";
    }

    private void SetDialogueUI(Dialogue dialogue)
    {
        switch (dialogue.author)
        {
            case Author.Player:
                dialogueText.color = playerColor;
                break;
            case Author.NPC:
                dialogueText.color = npcColor;
                break;
            case Author.System:
                dialogueText.color = systemColor;
                break;
        }
    }

    private IEnumerator TypeText(string text)
    {
        dialogueText.text = "";
        audioSource.clip = typingSound;
        audioSource.loop = true;
        audioSource.Play();

        foreach (char letter in text)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        audioSource.Stop();
    }
}
