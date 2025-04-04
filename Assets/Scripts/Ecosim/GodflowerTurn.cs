using UnityEngine;
using DG.Tweening;

public class GodflowerTurn : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float turnSpeed = 5f;

    Tween rotate;

    void Start()
    {
        transform.DORotate(new Vector3(0f, Random.Range(0, 360), 0f), 0f);

        if (target != null)
        {
            rotate = target.DOLocalRotate(new Vector3(0, 360, 0), turnSpeed, RotateMode.FastBeyond360)
                  .SetEase(Ease.Linear)
                  .SetLoops(-1, LoopType.Restart);
        }
    }

    private void OnDestroy()
    {
        rotate.Kill();
    }
}

