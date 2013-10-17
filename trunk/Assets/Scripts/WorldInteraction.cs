using UnityEngine;
using System.Collections;

public class WorldInteraction : uLink.MonoBehaviour
{
    public enum eWorldInterType { WIT_PICKUP_ITEM, WIT_KILL, NUM_WORLD_INTERACTION_TYPES }


	void Start () 
    {
	
	}
	
	void Update () 
    {
	
	}

    [RPC]
    void Collided(eWorldInterType _interactionType, int _interObjID)
    {
        Debug.Log("Collided RPC - interaction type = " + _interactionType + ", interaction object ID = " + _interObjID);

        PlayerData d = gameObject.GetComponent<PlayerData>();
		
        switch (_interactionType)
        {
            case eWorldInterType.WIT_PICKUP_ITEM:
                {
                    ItemManager mngr = GameObject.Find("ItemManager").GetComponent<ItemManager>();
                    Item item = mngr.GetItem(_interObjID);
                    item.ownerID = networkView.owner.id;

                    d.ObtainedItem(item, false, false);

                    Debug.Log(" - set owner id to " + item.ownerID);

                    break;
                }
            case eWorldInterType.WIT_KILL:
                {
                    if (d.job == "Human")
                    {
                        gameObject.GetComponent<MatchManager>().ShowMessage("You have died.");
                    	d.job = "Ghost";
                    }
                    break;
                }
        }


    }
}
