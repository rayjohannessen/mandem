using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerData_Server
{
    public enum ePlayerstate { PS_WAITING, PS_READY_AND_WAITING, PS_IN_MATCH, NUM_PLAYER_STATES }

    public ePlayerstate state;

    public bool isKiller;

    public List<Item> items;

    public PlayerData_Server(bool _isKiller)
    {
        state = ePlayerstate.PS_WAITING;
        isKiller = _isKiller;

        items = new List<Item>();
    }

    public void ObtainedItem(Item _item, bool _colliderOn, bool _visible = false)
    {
        items.Add(_item);
        _item.OnPickedUp(_visible, _colliderOn);
    }
}
