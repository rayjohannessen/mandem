using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour
{
    public enum eItemType { IT_WEAPON, IT_POTION, IT_CONTAINER, IT_MONEY, IT_JEWELRY, NUM_ITEM_TYPES }

    public eItemType itemType;

    public int id;

    public void SetBaseProperties(eItemType _type, int _id)
    {
        itemType = _type;
        id = _id;
    }

	void Start () 
    {
	
	}
	
	void Update () 
    {
	
	}
}
