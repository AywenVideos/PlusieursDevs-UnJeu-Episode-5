using System.Collections.Generic;
using UnityEngine;

public class ConsumableManager : MonoBehaviour
{
    public static ConsumableManager Instance { get; private set; }

    [SerializeField] private float consumeRadius = 0.25f; // Distance de consommation
    private List<IEntityConsumable> consumables = new List<IEntityConsumable>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEntityConsumable LowestDistanceFrom(IEntityConsumable actor, float maxDistanceNormOne = float.MaxValue)
    {
        float minDistance = float.MaxValue;
        IEntityConsumable result = null;
        Vector3 pos = ((MonoBehaviour)actor).transform.position;

        foreach (var consumable in consumables)
        {
            if (consumable == actor) continue;

            Vector3 v = pos - ((MonoBehaviour)consumable).transform.position;
            float d = Mathf.Abs(v.x) + Mathf.Abs(v.y) + Mathf.Abs(v.z);

            if (d < minDistance && d < maxDistanceNormOne)
            {
                minDistance = d;
                result = consumable;
            }
        }

        return result;
    }


    public void RegisterConsumable(IEntityConsumable consumable)
    {
        if (!consumables.Contains(consumable))
        {
            consumables.Add(consumable);
        }
    }

    public void UnregisterConsumable(IEntityConsumable consumable)
    {
        consumables.Remove(consumable);
    }

    public void CheckForConsumption(EntitySubject entity)
    {
        IEntityConsumable target = entity.foodTarget;

        if (target != null && target is MonoBehaviour targetMono)
        {
            // Vérifier si l'objet n'a pas été détruit
            if (targetMono != null && targetMono.transform != null)
            {
                if (Mathf.Abs(targetMono.transform.position.x) - Mathf.Abs(entity.transform.position.x) <= consumeRadius)
                {
                    entity.foodTarget.Neutralize();
                    entity.GetFood();
                }
            }
        }
    }
}
