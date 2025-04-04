using System.Collections;
using UnityEngine;

public class CrusherSubject : EntitySubject
{
    public override void Consume()
    {
        StartCoroutine(Consume_Coroutine());
    }

    IEnumerator Consume_Coroutine()
    {
        ForceSetState(State.Scream);
        yield return new WaitForSeconds(1.5f);
        base.Consume();
    }
}
