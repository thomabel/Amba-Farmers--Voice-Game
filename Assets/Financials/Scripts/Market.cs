using UnityEngine;
using System.Collections.Generic;

public class Market : MonoBehaviour
{
    public Account player_checking;
    public Inventory player_inventory;
    public List<Inventory> Inventories;
    public List<MarketWrapper> Reference;
    public Dictionary<Base.GoodType, MarketWrapper> Comparator;

    public struct Sellable
    {
        public Inventory inv;
        public int index;
        public MarketWrapper wrap;

        public Sellable(Inventory inv, int index, MarketWrapper wrap)
        {
            this.inv = inv;
            this.index = index;
            this.wrap = wrap;
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

        foreach (Inventory i in Inventories)
        {
            if (i.Add(obj) >= 0)
            {
                return true;
            }
        }

        return false;
    }

    public bool SellItem(Sellable item, float quantity)
    {
        if (quantity == 0 || quantity > item.inv[item.index].quantity)
        {
            return false;
        }

        player_checking.Credit((int)(item.wrap.PriceOf() * quantity));

        var obj = item.inv.Remove(item.index);
        Destroy(obj.obj);
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

                Sellables.Add(new Sellable(inv, i, wrap));
            }
        }
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
