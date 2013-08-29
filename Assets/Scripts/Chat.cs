using UnityEngine;
using System.Collections;

public class Chat : uLink.MonoBehaviour 
{
    bool bChatActive = false;

    public Vector2 vChatSize;
    Vector2 vChatPos;
    public Vector2 vChatOffsetFromBottomLeft;

    string strTextToSend;
    public string strDefaultChatText = "Text to send...";

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
            if (Input.GetKeyUp(KeyCode.Return))
            {
                // TODO::send any text
                bChatActive = false;
                return;
            }
            else if (Input.GetKeyUp(KeyCode.Escape))
            {
                bChatActive = false;
                Debug.Log("Chat DEactivated");
                return;
            }
        }
        else if (Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.Tab))
        {
            bChatActive = true;
            Debug.Log("Chat activated");
        }
	}

    void OnGUI()
    {
        if (bChatActive)
        {
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
            {
                Debug.Log("Sending chat text: " + strTextToSend);
                bChatActive = false;
                return;
            }
            else if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
            {
                Debug.Log("Chat DEactivated");
                bChatActive = false;
                return;
            }

            GUILayout.BeginArea(chatRect);
            //GUILayout.Box("Chat", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.BeginVertical();
            {
                strTextToSend = GUILayout.TextField(strTextToSend);
            }
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }
}
