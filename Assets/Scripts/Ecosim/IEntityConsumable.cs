public interface IEntityConsumable
{
    int GetFoodValue();

    /// <summary>
    /// quand l'entité a été dévorée
    /// </summary>
    void Consume();

    /// <summary>
    /// quand l'entité est en train de se faire dévorer
    /// </summary>
    void Neutralize();
}
