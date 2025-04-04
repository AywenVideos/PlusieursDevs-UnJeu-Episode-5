public interface IEntityConsumable
{
    int GetFoodValue();

    /// <summary>
    /// quand l'entit� a �t� d�vor�e
    /// </summary>
    void Consume();

    /// <summary>
    /// quand l'entit� est en train de se faire d�vorer
    /// </summary>
    void Neutralize();
}
