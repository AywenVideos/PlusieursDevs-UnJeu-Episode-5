using UnityEngine;

public class FootSubject : MonoBehaviour
{
    public EntitySubject subject;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Walkable"))
        {
            //subject.FootIn(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Walkable"))
        {
            //subject.FootOut(this);
        }
    }
}
