
namespace Base 
{
    /// <summary>
    /// A list of all goods that can be bought or sold on the market.
    /// Has the problem of changing prefab values if enum list is
    /// added to in the middle of the list. May be necessary to change
    /// approach to using ScriptableObjects instead.
    /// </summary>
    public enum GoodType
    {
        Tool_Start,
        Tool_Hoe,
        Tool_Bucket,
        Tool_End,
        Seed_Start,
        Seed_Avocado,
        Seed_Banana,
        Seed_Tomato,
        Seed_Onion,
        Seed_Corn,
        Seed_End,
        Animal_Start,
        Animal_Pig,
        Animal_Goat,
        Animal_Chicken,
        Animal_End,
        Fruit_Start,
        Fruit_Corn,
        Fruit_Tomato,
        Fruit_Onion,
        Fruit_Pepper,
        Fruit_End,
    }
}
