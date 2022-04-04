using UnityEngine;
using Currency = System.Int32;

[CreateAssetMenu]
public class Account : ScriptableObject
{
    [SerializeField] private Currency balance;
    [SerializeField] private Currency debt_floor;
    [SerializeField] private float annual_rate;
    [SerializeField] private int annual_periods;

    public Currency Balance()
    {
        return balance;
    }
    public Currency Credit(Currency amount)
    {
        if (balance + amount <= int.MaxValue)
        {
            balance += amount;
        }
        return balance;
    }
    public int Debit(Currency amount)
    {
        if (balance - amount >= debt_floor)
        {
            balance -= amount;
        }
        return balance;
    }
    public Currency Interest()
    {
        return Credit((int)(annual_rate / annual_periods * balance));
    }
}
