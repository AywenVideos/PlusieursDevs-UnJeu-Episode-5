using Managers;
using UnityEngine;

public class PosChecker : MonoBehaviour
{
    public int Position;
    void Start()
    {
        if (GameManager.Position != Position) Destroy(gameObject);
    }

}
