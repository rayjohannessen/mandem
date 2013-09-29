using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 
/// NOTE: there is currently no ItemManager used on the server
/// </summary>
public class ItemManager : MonoBehaviour 
{
    Dictionary<int, Item> items = new Dictionary<int, Item>();

	void Start () 
    {
	
	}
	
	void Update () 
    {
	
	}

    public void AddItem(Item _item)
    {
        if (!items.ContainsKey(_item.id))
        {
            Debug.Log("ItemManager::AddItem() - added item with id " + _item.id);
            items.Add(_item.id, _item);
        }
    }

    public void RemoveItem(int _key)
    {
        if (items.ContainsKey(_key))
        {
            items.Remove(_key);
        }
    }

    public void ClearItems()
    {
        foreach (KeyValuePair<int, Item> obj in items)
        {
            DestroyImmediate(obj.Value.gameObject);
        }
        items.Clear();
    }

    public Item GetItem(int _id)
    {
        Item item = null;

        if (items.ContainsKey(_id))
            item = items[_id];
        else
            Debug.Log("ItemManager::GetItem() - with id " + _id + " does not exist.");

        return item;
    }
}
