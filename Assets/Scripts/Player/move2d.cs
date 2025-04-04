using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class move2d : MonoBehaviour
{
    public float movSpeed;
    private float speedX, speedY;
    public Rigidbody2D rb;
    public Animator anim;
    public AudioSource stepSfx;

    public bool freeze;

    void Update()
    {
        if (!freeze)
        {
            speedX = Input.GetAxisRaw("Horizontal");
            speedY = Input.GetAxisRaw("Vertical");

            Vector2 movement = new Vector2(speedX, speedY);

            if (movement.magnitude > 1)
            {
                movement.Normalize();
            }

            rb.linearVelocity = movement * movSpeed;

            if (speedX == 1 && speedY == 0)
            {
                anim.SetInteger("Direction", 3);
            }

            if (speedX == -1 && speedY == 0)
            {
                anim.SetInteger("Direction", 4);
            }

            if (speedX == 0 && speedY == 1)
            {
                anim.SetInteger("Direction", 2);
            }

            if (speedX == 0 && speedY == -1)
            {
                anim.SetInteger("Direction", 1);
            }

            if (speedX == 0 && speedY == 0)
            {
                anim.SetBool("IsWalking", false);
            }
            else
            {
                anim.SetBool("IsWalking", true);
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    public void stepSound()
    {
        stepSfx.Play();
    }
}
