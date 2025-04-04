using UnityEngine;

public class pickUp : MonoBehaviour
{
    public GameObject instructions;
    public bool canPickUp;
    public boxNumber stock;
    public AudioSource pickUpSfx;

    void Start()
    {
        instructions.SetActive(false);
        canPickUp = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            instructions.SetActive(true);
            canPickUp = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            instructions.SetActive(false);
            canPickUp = false;
        }
    }

    void Update()
    {
        if(canPickUp == true)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                stock.number += 1;
                pickUpSfx.Play();
                Destroy(gameObject);
            }
        }
    }
}
