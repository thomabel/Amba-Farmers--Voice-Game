using UnityEngine;
using Currency = System.Int32;

/// <summary>
/// Bank account for handling money and finances.
/// May be set up as a checking, savings, or loan account.
/// </summary>
[CreateAssetMenu(
    fileName = "account",
    menuName = "Financials/Account"
    )]
public class Account : ScriptableObject
{
    [SerializeField] private Currency balance;
    [SerializeField] private Currency debt_floor;
    [SerializeField] private float annual_rate;
    [SerializeField] private int annual_periods;

    /// <summary>
    /// Return the current balance on the account.
    /// </summary>
    /// <returns></returns>
    public Currency Balance()
    {
        return balance;
    }

    /// <summary>
    /// Add funds to the account.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public Currency Credit(Currency amount)
    {
        if (balance + amount <= int.MaxValue)
        {
            balance += amount;
        }
        return balance;
    }

    /// <summary>
    /// Remove funds from the account.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public int Debit(Currency amount)
    {
        if (balance - amount >= debt_floor)
        {
            balance -= amount;
        }
        return balance;
    }

    /// <summary>
    /// Calculate interest accrural.
    /// </summary>
    /// <returns></returns>
    public Currency Interest()
    {
        if (annual_periods != 0)
        {
            return Credit((int)(annual_rate / annual_periods * balance));
        }
        return balance;
    }
}
