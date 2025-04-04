using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class NPCOnHeroTouched : MonoBehaviour
{
    public npcMove _npcMove;
    public move2d npcMove;
    public List<string> dialogueLines;
    private bool isInteracting = false;
    public AudioSource audioSource;
    public AudioClip glitchclip;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isInteracting && _npcMove.stop)
        {
            SceneManager.LoadSceneAsync("Coms");
        }
    }
}