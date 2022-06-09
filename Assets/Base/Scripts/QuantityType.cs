
namespace Base
{
    /// <summary>
    /// Some quantities are discrete (ie seeds)
    /// while others represent mass in kilograms (ie fruits).
    /// It may be better to change quantity to all integers
    /// and have the mass type be grams.
    /// </summary>
    public enum QuantityType
    {
        Integer, Mass
    }
}
