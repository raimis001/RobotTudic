using UnityEngine;
using System.Collections.Generic;

public class Storage : MonoBehaviour
{
    public Transform entryPosition => entryPos != null ? entryPos : transform;

    public string accept;
    public int capacity = 100;
    private int currentLoad = 0;

    public Transform itemsParent;
    [SerializeField]
    Transform entryPos;

    Transform[] itemsObj;

    private void Start()
    {
        itemsObj = itemsParent.GetComponentsInChildren<Transform>(true);
        foreach (var item in itemsObj)
        {
            item.gameObject.SetActive(false);
        }
    }

    public static bool FindStorageByItem(string item, out Storage storage)
    {
        Storage[] storages = FindObjectsByType<Storage>(FindObjectsSortMode.InstanceID);
        foreach (Storage s in storages)
        {
            if (s.accept.Equals(item, System.StringComparison.OrdinalIgnoreCase))
            {
                storage = s;
                return true;
            }
        }
        storage = null;
        return false;
    }

    public void StoreItem(string item, int quantity)
    {
        if (!accept.Equals(item, System.StringComparison.OrdinalIgnoreCase))
        {
            Debug.LogWarning($"Storage does not accept {item}");
            return;
        }
        if (currentLoad + quantity > capacity)
        {
            Debug.LogWarning($"Not enough capacity to store {quantity} of {item}. Current load: {currentLoad}/{capacity}");
            return;
        }
        currentLoad += quantity;
        int prc = Mathf.RoundToInt((float)currentLoad / capacity * itemsObj.Length);
        for (int i = 0; i < itemsObj.Length; i++)
        {
            itemsObj[i].gameObject.SetActive(prc >= (i ));
        }

        Debug.Log($"Stored {quantity} of {item}. Current load: {currentLoad}/{capacity}");
    }
}
