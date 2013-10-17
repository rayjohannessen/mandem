using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerData_Server
{
    public enum ePlayerstate { PS_WAITING, PS_READY_AND_WAITING, PS_IN_MATCH, NUM_PLAYER_STATES }

    public ePlayerstate state;

    public bool isKiller;

    public List<Item> items;

    public int[] currency = new int[(int)Currency.eDenomination.NUM_DENOMINATIONS];

    public PlayerData_Server(bool _isKiller)
    {
        state = ePlayerstate.PS_WAITING;
        isKiller = _isKiller;

        items = new List<Item>();
    }

    public void ObtainedItem(Item _item, bool _colliderOn, bool _visible = false)
    {
        if (_item.itemType == Item.eItemType.IT_MONEY)
        {
            currency[_item.GetSubtype()] = ((Currency)_item).amount;
            // money item is destroyed in OnPickedUp()
        }
        else
        {
            items.Add(_item);
        }

        _item.OnPickedUp(_visible, _colliderOn);
    }
}
