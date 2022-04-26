using UnityEngine;
using System.Collections.Generic;

public class Market : MonoBehaviour
{
    public List<Inventory> Inventories;
    public List<MarketWrapper> Reference;
    public Dictionary<Financials.GoodType, MarketWrapper> Comparator;

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

    public void PopulateSellables()
    {
        foreach (Inventory inv in Inventories)
        {
            for(int i = 0; i < inv.Size; i++)
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
        //PopulateComparator();
    }
    /*
    private void PopulateComparator()
    {
        foreach (MarketWrapper wrap in Reference)
        {
            Comparator.Add(wrap.type, wrap);
        }
    }
    */
}
