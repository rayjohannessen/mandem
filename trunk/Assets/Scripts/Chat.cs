using UnityEngine;
using System.Collections;

public class Chat : uLink.MonoBehaviour 
{
    bool bChatActive = false;
    bool textSent = false;
    bool newTextTyped = false;

    public Vector2 vChatSize;
    Vector2 vChatPos;
    public Vector2 vChatOffsetFromBottomLeft;

    string strTextToSend;
    public string strDefaultChatText = "Type to chat...";

    Rect chatRect;

	void Start () 
    {
        vChatPos = new Vector2(0f, Screen.height);
        vChatPos.y -= vChatOffsetFromBottomLeft.y;
        vChatPos.x += vChatOffsetFromBottomLeft.x;
        strTextToSend = strDefaultChatText;

        chatRect = new Rect(vChatPos.x, vChatPos.y, vChatSize.x, vChatSize.y);
	}
	
	void Update ()
    {
        if (bChatActive)
        {
        }
        // make sure enter on send doesn't activate the chat again immediately, i.e. the same frame
        // that the text is sent. And only bring it up if we're connected to the server
        //
        // TODO:: there will be cases where you can't yet chat...
        //
        else if (!textSent && (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.Tab)) &&
                    uLink.Network.status == uLink.NetworkStatus.Connected)
        {
            bChatActive = true;
            Debug.Log("Chat activated");
        }
        textSent = false;
	}

    void OnGUI()
    {
        if (bChatActive)
        {
            if (Event.current.type == EventType.KeyDown)
            {
                // get rid of default text on the first valid key press
                if (!newTextTyped && Input.inputString.Length > 0 && Input.inputString[0] != '\n')
                {
                    newTextTyped = true;
                    strTextToSend = "";
                }
            }
            else if (Event.current.type == EventType.KeyUp)
            {
                // Only send if they've typed something legit
                if (Event.current.keyCode == KeyCode.Return && newTextTyped)
                {
                    Debug.Log("Sending chat text: " + strTextToSend);

                    networkView.RPC("BroadcastChatMessage", uLink.RPCMode.Server, strTextToSend);

                    textSent = true;
                    _DeactivateChat();
                    return;
                }
                else if (Event.current.keyCode == KeyCode.Escape)
                {
                    _DeactivateChat();
                    return;
                }
            }

            GUI.SetNextControlName("ChatText");

            GUILayout.BeginArea(chatRect);
            //GUILayout.Box("Chat", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.BeginVertical();
            {
                strTextToSend = GUILayout.TextField(strTextToSend);
            }
            GUILayout.EndVertical();
            GUILayout.EndArea();

            if (GUI.GetNameOfFocusedControl() == string.Empty)
            {
                GUI.FocusControl("ChatText");
            }
            if (strTextToSend.Length == 0)
            {
                newTextTyped = false;
                strTextToSend = strDefaultChatText;
            }
        }
    }

    void _DeactivateChat()
    {
        newTextTyped = false;
        bChatActive = false;
        strTextToSend = strDefaultChatText;
        Debug.Log("Chat DEactivated");
    }

    [RPC]
    void ChatMessage(string _message, int _playedID/*, uLink.NetworkMessageInfo _info*/)
    {
        Debug.Log("ChatMessage() - received message " + _message + " from player with ID " + _playedID);

        GameObject playerObj = GameObject.Find("PlayerManager").GetComponent<PlayerManager>().GetPlayer(_playedID);
        if (playerObj)
        {
            ChatLabel label = playerObj.GetComponent<ChatLabel>();
            if (label)
            {
                Debug.Log("ChatMessage() - label text set.");
                label.SetText(_message);
            }
            else
            {
                Debug.Log("ChatMessage() - label text not set, label == null.");
            }
        }
        else
        {
            Debug.Log("GetPlayer() - player with id " + _playedID + " does not exist in dictionary.");            
        }
    }
}
