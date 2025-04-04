using System.Collections;
using UnityEngine;

public class npcMove : MonoBehaviour
{
    public Transform target;
    public float speed;
    public bool stop = false;
    public AudioSource stepSfx;
    public Animator anim;
    public AudioSource epicMusic;

    void Update()
    {
        if (stop == false)
        {
            anim.SetBool("IsWalking", true);
            var step =  speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
            if(Vector3.Distance(transform.position, target.position) < 0.001f)
            {
                stop = true;
                anim.SetBool("IsWalking", false);
                epicMusic.Play();

                StartCoroutine(EcoutezMoiBienSiCeJeuNeVaPasSeTerminerSurUnEnormeChaos_Coroutine());
            }
        }
    }

    IEnumerator EcoutezMoiBienSiCeJeuNeVaPasSeTerminerSurUnEnormeChaos_Coroutine()
    {
        yield return new WaitForSeconds(1f);

        ObjectivesManager.Instance.ShowObj("Aidez votre client.");
    }

    public void foostepSfx()
    {
        stepSfx.Play();
    }
}
