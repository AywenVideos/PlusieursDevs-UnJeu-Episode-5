using System.Collections;
using UnityEngine;

public class GlitchTrigger : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clip;

    public Material glitchMaterial;

    public void TriggerGlitch()
    {
        StartCoroutine(GlitchEffect());
    }

    IEnumerator GlitchEffect()
    {
        audioSource.PlayOneShot(clip);
        glitchMaterial.SetFloat("_Intensity", 1.0f);
        yield return new WaitForSeconds(0.2f);
        glitchMaterial.SetFloat("_Intensity", 0.0f);
    }
}
