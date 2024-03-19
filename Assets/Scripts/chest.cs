using System.Collections.Generic;
using UnityEngine;

namespace QuantumTek.QuantumInventory
{
    /// <summary>
    /// QI_Vendor handles everything that a vendor would need: currency, items, and the ability to exchange between them.
    /// </summary>
    [AddComponentMenu("Quantum Tek/Quantum Inventory/Vendor")]
    [DisallowMultipleComponent]
    public class chest : MonoBehaviour
    {
        public QI_Inventory Inventory;
        public Dictionary<string, QI_CurrencyStash> Currencies = new Dictionary<string, QI_CurrencyStash>();

        /// <summary>
        /// Completes a transaction between a buyer and a seller.
        /// </summary>
        /// <param name="buyer">The character buying the items.</param>
        /// <param name="seller">The character selling the items.</param>
        /// <param name="item">The item to buy.</param>
        /// <param name="currencyName">The name of the currency needed.</param>
        /// <param name="amount">The amount to buy.</param>
        /// <returns></returns>
        public static bool Transaction(QI_Vendor buyer, QI_Vendor seller, QI_ItemData item, string currencyName, int amount)
        {
            if (!buyer.CanBuy(currencyName, item.Price, amount) || !seller.CanSell(item.Name, amount))
                return false;
            buyer.Buy(item, currencyName, amount);
            seller.Sell(item, currencyName, amount);

            return true;
        }

        /// <summary>
        /// Returns if the vendor buying something was successful.
        /// </summary>
        /// <param name="item">The item to buy.</param>
        /// <param name="currencyName">The name of the currency needed.</param>
        /// <param name="amount">The amount to buy.</param>
        /// <returns></returns>
        public bool Buy(QI_ItemData item, string currencyName, int amount)
        {
            if (!CanBuy(currencyName, item.Price, amount))
                return false;

            QI_CurrencyStash currency = Currencies[currencyName];
            currency.Amount -= item.Price * amount;
            Currencies[currencyName] = currency;
            Inventory.AddItem(item, amount);

            return true;
        }

        /// <summary>
        /// Returns if the vendor selling something was successful.
        /// </summary>
        /// <param name="item">The item to buy.</param>
        /// <param name="currencyName">The name of the currency gained.</param>
        /// <param name="amount">The amount to sell.</param>
        /// <returns></returns>
        public bool Sell(QI_ItemData item, string currencyName, int amount)
        {
            if (!CanSell(item.Name, amount))
                return false;

            if (Currencies.ContainsKey(currencyName))
            {
                QI_CurrencyStash currency = Currencies[currencyName];
                currency.Amount += item.Price * amount;
                Currencies[currencyName] = currency;
            }

            Inventory.RemoveItem(item.Name, amount);

            return true;
        }

        /// <summary>
        /// Returns if there is enough curreny for the vendor to buy the given item.
        /// </summary>
        /// <param name="currencyName">The name of the currency required.</param>
        /// <param name="itemCost">The cost of the item.</param>
        /// <param name="amount">The amount to buy.</param>
        /// <returns></returns>
        public bool CanBuy(string currencyName, float itemCost, int amount)
        {
            float cost = itemCost * amount;
            if (!Currencies.ContainsKey(currencyName))
                return false;
            if (Currencies[currencyName].Amount < cost && !Mathf.Approximately(Currencies[currencyName].Amount, cost))
                return false;

            return true;
        }

        /// <summary>
        /// Returns if there is enough of an item for the vendor to sell.
        /// </summary>
        /// <param name="itemName">The name of the item.</param>
        /// <param name="amount">The amount to sell.</param>
        /// <returns></returns>
        public bool CanSell(string itemName, int amount)
        {
            int stock = Inventory.GetStock(itemName);

            return stock >= amount;
        }
    }
}