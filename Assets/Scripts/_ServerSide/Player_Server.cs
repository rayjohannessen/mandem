using UnityEngine;
using System.Collections;

public class Player_Server
{
    public enum ePlayerstate { PS_WAITING, PS_READY_AND_WAITING, PS_IN_MATCH, NUM_PLAYER_STATES }

    public ePlayerstate state;

    public bool isKiller;

    public Player_Server(bool _isKiller)
    {
        state = ePlayerstate.PS_WAITING;
        isKiller = _isKiller;
    }
}
