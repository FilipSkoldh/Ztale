﻿using System.Collections.Generic;
using UnityEngine;

namespace QuantumTek.QuantumInventory
{
    /// <summary>
    /// QI_ItemDatabase stores a list of item data.
    /// </summary>
    [CreateAssetMenu(menuName = "Quantum Tek/Quantum Inventory/Item Database")]
    public class QI_ItemDatabase : ScriptableObject
    {
        public List<QI_ItemData> Items = new List<QI_ItemData>();

        /// <summary>
        /// Returns an item by the given name.
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <returns></returns>
        public QI_ItemData GetItem(string name)
        {
            foreach (var item in Items)
                if (item.Name == name)
                    return item;
            return null;
        }

        /// <summary>
        /// Returns full database dictionary
        /// </summary>
        /// <returns>The database dictionary</returns>
        public Dictionary<string, QI_ItemData> Getdictionary()
        {
            Dictionary<string, QI_ItemData> entireDatabase = new Dictionary<string, QI_ItemData>();

            foreach (var item in Items)
            {
                string name = item.Name;
                entireDatabase.Add(name, item);
            }
            return entireDatabase;
        }
    }
}