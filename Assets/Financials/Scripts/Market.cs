using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Handles buying and selling of goods on the market.
/// At some point should change item prices.
/// </summary>
public class Market : MonoBehaviour
{
    public Account player_checking; // Player's bank account for transactions.
    public Inventory player_inventory; // Player inventory.
    public List<Inventory> Inventories; // Other inventories, including house.
    public List<MarketWrapper> Reference; // Stores aspects of good types.
    public Dictionary<Base.GoodType, MarketWrapper> Comparator; // Connects type with wrapper.

    public ShelterHandler shelters;
    /*
    public struct SellableAnimal
    {

    }
    */

    /// <summary>
    /// Stores all aspects of a sellable object for ease of reference.
    /// </summary>
    public struct Sellable
    {
        public Inventory inv;
        public int index;
        public MarketWrapper wrap;

        public GameObject Animal;


        public Sellable(Inventory inv, int index, MarketWrapper wrap, GameObject Animal)
        {
            this.inv = inv;
            this.index = index;
            this.wrap = wrap;
            this.Animal = Animal;
        }
    }
    /// <summary>
    /// A list of all the sellable items in all non-player inventories.
    /// </summary>
    public List<Sellable> Sellables;

    /// <summary>
    /// Buys an item from the market.
    /// </summary>
    /// <param name="item">The type of item to buy.</param>
    /// <param name="quantity">The quantity of that item to buy.</param>
    /// <returns>Success of buy. May fail from lack of funds.</returns>
    public bool BuyItem(MarketWrapper item, float quantity)
    {
        // Check if player can afford.
        var cost = Mathf.Round(item.PriceOf() * quantity);
        if (quantity == 0 || cost > player_checking.Balance())
        {
            return false;
        }

        // Check for space in inventories.
        if (FreeSpace() == 0)
        {
            return false;
        }

        // Remove funds and spawn item.
        player_checking.Debit((int)cost);
        var obj = Instantiate(item.item_prefab);

        obj.AddComponent<TypeLabel>();
        TypeLabel tmpLabel = obj.GetComponent<TypeLabel>();
        tmpLabel.Type = item.type;

        Item tmp = new Item();
        tmp.obj = obj;
        tmp.quantity = quantity;

        // Find open inventory slot.
        foreach (Inventory i in Inventories)
        {
            if (i.Add(tmp) >= 0)//i.Add(obj) >= 0)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Sells at particular sellable item.
    /// </summary>
    /// <param name="item">The sellable to sell.</param>
    /// <param name="quantity">The amount to sell.</param>
    /// <returns>Success of sell.</returns>
    public bool SellItem(Sellable item, float quantity)
    {
        if (item.Animal == null && 
            (quantity == 0 || 
            quantity > item.inv[item.index].quantity))
        {
            return false;
        }

        player_checking.Credit((int)(item.wrap.PriceOf() * quantity));

        // Non-animal transaction.
        if (item.Animal == null)
        {
            var obj = item.inv.Remove(item.index);
            Destroy(obj.obj);
        }
        // Shelters handle animals.
        else
        {
            shelters.RemoveAnimal(item.Animal);
        }
        Sellables.Remove(item);

        return true;
    }

    /// <summary>
    /// Check for total amount of free slots in inventories.
    /// </summary>
    /// <returns>The number of open slots.</returns>
    public int FreeSpace()
    {
        int free = 0;
        foreach (Inventory i in Inventories)
        {
            free += i.FreeSpace;
        }
        return free;
    }

    /// <summary>
    /// Searches through inventories for sellable items and populates list of them.
    /// </summary>
    public void PopulateSellables()
    {
        foreach (Inventory inv in Inventories)
        {
            for (int i = 0; i < inv.Size; i++)
            {
                var item = inv[i];
                if (item == null)
                    continue;

                var type = item.obj.GetComponent<TypeLabel>();
                if (type == null)
                    continue;

                MarketWrapper wrap;
                if (!Comparator.TryGetValue(type.Type, out wrap))
                    continue;

                Sellables.Add(new Sellable(inv, i, wrap, null));
            }
        }

        foreach(GameObject AnimalObj in shelters.GetEntirePopulationList())
            Sellables.Add(new Sellable(null,-1, Comparator[AnimalObj.GetComponent<Animal>().species], AnimalObj));

    }

    /// <summary>
    /// Finds the number of a particular type of item to combine quantities.
    /// </summary>
    /// <param name="type">The good type.</param>
    /// <returns>The quantity.</returns>
    public float TotalNumberOfItems(Base.GoodType type)
    {
        foreach (Inventory inv in Inventories)
        {
            float quantity = inv.FindQuantity(type);
            if (quantity != -1)
            {
                return quantity;
            }
        }
        return -1;
    }
    private void Start()
    {
        Comparator = new Dictionary<Base.GoodType, MarketWrapper>();
        Sellables = new List<Sellable>();
        foreach (MarketWrapper wrap in Reference)
        {
            Comparator.Add(wrap.type, wrap);
        }
    }
}
