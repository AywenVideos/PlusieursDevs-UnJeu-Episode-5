using UnityEngine;

public class Flower : MonoBehaviour, IEntityConsumable
{
    [SerializeField] private int foodValue = 1;

    private void Start()
    {
        ConsumableManager.Instance.RegisterConsumable(this);
    }

    public int GetFoodValue()
    {
        return foodValue;
    }

    public void Consume()
    {
        // La fleur disparaît lorsqu’elle est consommée
        if(gameObject != null) Destroy(gameObject);
    }

    public void Neutralize()
    {
        
    }
}
