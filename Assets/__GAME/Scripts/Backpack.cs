using UnityEngine;
using System.Collections.Generic;

public class Backpack : MonoBehaviour
{
    public Dictionary<string, int> items = new Dictionary<string, int>();

    public void AddItem(string item, int quantity)
    {
        if (items.ContainsKey(item))
            items[item] += quantity;
        else
            items[item] = quantity;
        Debug.Log($"Added {quantity} of {item} to backpack. Total: {items[item]}");
    }
    public bool RemoveItem(string item, int quantity)
    {
        if (!items.ContainsKey(item) || items[item] < quantity)
        {
            Debug.LogWarning($"Not enough {item} in backpack to remove {quantity}");
            return false;
        }
        items[item] -= quantity;
        if (items[item] <= 0)
            items.Remove(item);
        Debug.Log($"Removed {quantity} of {item} from backpack. Remaining: { (items.ContainsKey(item) ? items[item] : 0)}");
        return true;
    }
    public bool HasItem(string item)
    {
        return items.ContainsKey(item) && items[item] > 0;
    }
}
