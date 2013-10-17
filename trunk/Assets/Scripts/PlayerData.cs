using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerData : MonoBehaviour 
{
    //[HideInInspector]
    public int playerID;
	public string job = "unemployed";
	public int score = 0;

    public List<Item> items;

    public bool hasWeapon = false;

	void Start () 
    {
	}
	
	void Update () 
    {
	}
    
    public void ObtainedItem(Item _item, bool _colliderOn, bool _visible = false)
    {
        items.Add(_item);
        _item.OnPickedUp(_visible, _colliderOn);

        // TODO: "add the item to the proprietary dark shaman flower of life radial menu"
        hasWeapon = true;
    }
}
