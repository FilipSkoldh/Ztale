using System.Collections.Generic;
using UnityEngine;

namespace QuantumTek.QuantumInventory
{
    /// <summary>
    /// QI_Chest handles everything that a chest would need: items, and the ability to exchange between them.
    /// </summary>
    [AddComponentMenu("Quantum Tek/Quantum Inventory/Vendor")]
    [DisallowMultipleComponent]
    public class QI_Chest : MonoBehaviour
    {
        public QI_Inventory Inventory;

        /// <summary>
        /// Completes a transaction between the player and the chest.
        /// </summary>
        /// <param name="buyer">The player/chest getting the items.</param>
        /// <param name="seller">The player/chest loosing the items.</param>
        /// <param name="item">The item to move.</param>
        /// <param name="amount">The amount to move.</param>
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
        /// Returns if the player/chest moving something was successful.
        /// </summary>
        /// <param name="item">The item to move.</param>
        /// <param name="amount">The amount to move.</param>
        /// <returns></returns>
        public bool Buy(QI_ItemData item, int amount)
        {
            Inventory.AddItem(item, amount);
            return true;
        }

        /// <summary>
        /// Returns if the player/chest moving something was successful.
        /// </summary>
        /// <param name="item">The item to move.</param>
        /// <param name="amount">The amount to move.</param>
        /// <returns></returns>
        public bool Sell(QI_ItemData item, int amount)
        {
            if (!CanSell(item.Name, amount))
                return false;

            Inventory.RemoveItem(item.Name, amount);

            return true;
        }


        /// <summary>
        /// Returns if there is enough of an item for the vendor to move.
        /// </summary>
        /// <param name="itemName">The name of the item.</param>
        /// <param name="amount">The amount to move.</param>
        /// <returns></returns>
        public bool CanSell(string itemName, int amount)
        {
            int stock = Inventory.GetStock(itemName);
            Debug.Log($"amount:{amount} stock:{stock}");

            return stock >= amount;
        }
    }
}