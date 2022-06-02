using UnityEngine;
using System.Collections.Generic;

public class Market : MonoBehaviour
{
    public Account player_checking;
    public Inventory player_inventory;
    public List<Inventory> Inventories;
    public List<MarketWrapper> Reference;
    public Dictionary<Base.GoodType, MarketWrapper> Comparator;

    public ShelterHandler shelters;
    /*
    public struct SellableAnimal
    {

    }
    */
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
    public List<Sellable> Sellables;

    public bool BuyItem(MarketWrapper item, float quantity)
    {
        var cost = Mathf.Round(item.PriceOf() * quantity);
        if (quantity == 0 || cost > player_checking.Balance())
        {
            return false;
        }

        if (FreeSpace() == 0)
        {
            return false;
        }

        player_checking.Debit((int)cost);
        var obj = Instantiate(item.item_prefab);

        obj.AddComponent<TypeLabel>();
        TypeLabel tmpLabel = obj.GetComponent<TypeLabel>();
        tmpLabel.Type = item.type;

        Item tmp = new Item();
        tmp.obj = obj;
        tmp.quantity = quantity;

        foreach (Inventory i in Inventories)
        {
            if (i.Add(tmp) >= 0)//i.Add(obj) >= 0)
            {
                return true;
            }
        }

        return false;
    }

    public bool SellItem(Sellable item, float quantity)
    {
        if (item.Animal == null && (quantity == 0 || quantity > item.inv[item.index].quantity))
        {
            return false;
        }

        player_checking.Credit((int)(item.wrap.PriceOf() * quantity));

        if (item.Animal == null)
        {
            var obj = item.inv.Remove(item.index);
            Destroy(obj.obj);
        }
        else shelters.RemoveAnimal(item.Animal);
        Sellables.Remove(item);

        return true;
    }

    public int FreeSpace()
    {
        int free = 0;
        foreach (Inventory i in Inventories)
        {
            free += i.FreeSpace;
        }
        return free;
    }

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

    public float TotalNumberOfItems(Base.GoodType type)
    {
        foreach (Inventory inv in Inventories)
        {
            float quantity = inv.FindQuantity(type);
            if (quantity != -1) return quantity;
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
