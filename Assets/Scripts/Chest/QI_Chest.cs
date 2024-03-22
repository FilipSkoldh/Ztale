using System.Collections.Generic;
using UnityEngine;

namespace QuantumTek.QuantumInventory
{
    /// <summary>
    /// QI_Vendor handles everything that a vendor would need: currency, items, and the ability to exchange between them.
    /// </summary>
    [AddComponentMenu("Quantum Tek/Quantum Inventory/Vendor")]
    [DisallowMultipleComponent]
    public class QI_Chest : MonoBehaviour
    {
        public QI_Inventory Inventory;

        /// <summary>
        /// Completes a transaction between a buyer and a seller.
        /// </summary>
        /// <param name="buyer">The character buying the items.</param>
        /// <param name="seller">The character selling the items.</param>
        /// <param name="item">The item to buy.</param>
        /// <param name="amount">The amount to buy.</param>
        /// <returns></returns>
        public static bool Transaction(QI_Chest buyer, QI_Chest seller, QI_ItemData item, int amount)
        {
            if (!seller.CanSell(item.Name, amount))
                return false;
            buyer.Buy(item, amount);
            seller.Sell(item, amount);

            return true;
        }

        /// <summary>
        /// Returns if the vendor buying something was successful.
        /// </summary>
        /// <param name="item">The item to buy.</param>
        /// <param name="amount">The amount to buy.</param>
        /// <returns></returns>
        public bool Buy(QI_ItemData item, int amount)
        {
            Inventory.AddItem(item, amount);
            return true;
        }

        /// <summary>
        /// Returns if the vendor selling something was successful.
        /// </summary>
        /// <param name="item">The item to buy.</param>
        /// <param name="amount">The amount to sell.</param>
        /// <returns></returns>
        public bool Sell(QI_ItemData item, int amount)
        {
            if (!CanSell(item.Name, amount))
                return false;

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