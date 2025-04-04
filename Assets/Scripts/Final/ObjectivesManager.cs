using System.Collections;
using UnityEngine;
using TMPro;

public class ObjectivesManager : MonoBehaviour
{
    public TextMeshProUGUI objectiveDescription;
    public TextMeshProUGUI dialogueText;
    public float objectiveTypingSpeed = 0.05f;
    public float dialogueTypingSpeed = 0.02f;
    public AudioSource audioSource;
    public AudioClip typingSound;
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    private static ObjectivesManager instance;
    public static ObjectivesManager Instance => instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        objectiveDescription.text = "";
        objectiveDescription.gameObject.SetActive(false);
        dialogueText.text = "";
        dialogueText.gameObject.SetActive(false);
    }

    public void ShowObj(string description)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            audioSource.Stop();
        }

        objectiveDescription.gameObject.SetActive(true);
        typingCoroutine = StartCoroutine(TypeText(description, objectiveDescription, objectiveTypingSpeed));
    }

    public void ShowDialogues(string dialogue)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            audioSource.Stop();
        }

        dialogueText.gameObject.SetActive(true);
        typingCoroutine = StartCoroutine(TypeText(dialogue, dialogueText, dialogueTypingSpeed));
    }

    IEnumerator TypeText(string text, TextMeshProUGUI textComponent, float speed)
    {
        isTyping = true;
        textComponent.text = "";
        if (typingSound != null && audioSource != null)
        {
            audioSource.clip = typingSound;
            audioSource.loop = true;
            audioSource.Play();
        }

        foreach (char letter in text)
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(speed);
        }

        isTyping = false;
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    private void SkipTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        isTyping = false;
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    public void HideObj()
    {
        SkipTyping();
        objectiveDescription.text = "";
        objectiveDescription.gameObject.SetActive(false);
    }

    public void HideDialogues()
    {
        SkipTyping();
        dialogueText.text = "";
        dialogueText.gameObject.SetActive(false);
    }
}
