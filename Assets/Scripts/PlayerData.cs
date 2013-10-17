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

    public int[] currency = new int[(int)Currency.eDenomination.NUM_DENOMINATIONS];

    public bool hasWeapon = false;

	void Start () 
    {
	}
	
	void Update () 
    {
	}
    
    public void ObtainedItem(Item _item, bool _colliderOn, bool _visible = false)
    {
        if (_item.itemType == Item.eItemType.IT_MONEY)
        {
            currency[_item.GetSubtype()] = ((Currency)_item).amount;
            // money item is destroyed in OnPickedUp()
        }
        else
            items.Add(_item);

        _item.OnPickedUp(_visible, _colliderOn);

        // TODO: "add the item to the proprietary dark shaman flower of life radial menu"
        // *** do that in OnPickedUp() ^^^
        if (_item.itemType == Item.eItemType.IT_WEAPON)
        {
            hasWeapon = true;
        }
    }
}
