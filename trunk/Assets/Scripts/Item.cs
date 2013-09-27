using UnityEngine;
using System.Collections;

public class Item : uLink.MonoBehaviour
{
    public enum eItemType { IT_WEAPON, IT_POTION, IT_CONTAINER, IT_MONEY, IT_JEWELRY, NUM_ITEM_TYPES }

    public eItemType itemType;

    public int id;

    public bool onGround;

    public GameObject physicalObj;

    public void SetBaseProperties(eItemType _type, int _id, bool _onGround = true)
    {
        itemType = _type;
        id = _id;
        onGround = _onGround;
    }

	void Start () 
    {
		tag = "Item";
	}
	
	void Update () 
    {
	
	}
    
    void OnTriggerEnter(Collider _other)
    {
        Debug.Log("Item::OnTriggerEnter()");
        if (Network.isServer)
        {
            Debug.Log("Item " + name + " collided with " + _other.gameObject.name);
        }
    }

    public virtual short GetSubtype() { return -1; }
}
