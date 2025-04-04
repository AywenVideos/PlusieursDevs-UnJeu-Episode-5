using UnityEngine;
using System.Collections;

public class Refill : MonoBehaviour
{
    public GameObject instructions;
    public boxNumber stock;
    public bool canFill;
    public Animator anim;
    public bool filled;
    public AudioSource refillSfx;
    public AudioSource openDoorSfx;
    public npcSpawn Spawner;

    void Start()
    {
        instructions.SetActive(false);
        canFill = false;
        filled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && filled == false)
        {
            instructions.SetActive(true);
            canFill = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && filled == false)
        {
            instructions.SetActive(false);
            canFill = false;
        }
    }

    void Update()
    {
        if(canFill == true && stock.number > 0 && Input.GetKeyDown(KeyCode.E) && filled == false)
        {
            anim.SetBool("Filled", true);
            filled = true;
            instructions.SetActive(false);
            refillSfx.Play();
            stock.number -= 1;
            stock.shelvesNumber += 1;
            if(stock.shelvesNumber == 2)
            {
                StartCoroutine(OpenDelay());
            }
        }
    }

    private IEnumerator OpenDelay()
    {
        yield return new WaitForSeconds(3f);
        openDoorSfx.Play();
        Spawner.SpawnNpc();
    }
}
