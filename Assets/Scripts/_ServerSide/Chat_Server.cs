using UnityEngine;
using System.Collections;

public class Chat_Server : uLink.MonoBehaviour 
{


    [RPC]
    void BroadcastChatMessage(string _message, uLink.NetworkMessageInfo _info)
    {
        // TODO::get player id - need to add this functionality (create (or just use ulink's player id) when player logs in)
        Debug.Log("BroadcastChatMessage()");
        networkView.RPC("ChatMessage", uLink.RPCMode.OthersExceptOwner, _message, _info.sender.id);
    }
}
