using UnityEngine;
using System.Collections;

public class Item : uLink.MonoBehaviour
{
    public enum eItemType { IT_WEAPON, IT_POTION, IT_CONTAINER, IT_MONEY, IT_JEWELRY, NUM_ITEM_TYPES }

    public eItemType itemType;
    public int id;
    public bool onGround;
    public GameObject physicalObj;

    public int ownerID;

    public void SetBaseProperties(eItemType _type, int _id, int _ownerID, bool _onGround = true)
    {
        itemType = _type;
        id = _id;
        ownerID = _ownerID;
        onGround = _onGround;

        Debug.Log("Item ID set to " + _id);
    }

	void Start () 
    {
		tag = "Item";
	}
	
	void Update () 
    {
	
	}
    
    protected void HandleTriggerEnter(Collider _other)
    {
        if (_other.gameObject.GetComponent<WorldInteraction_Server>())
        {
            Debug.Log(_other.gameObject.name + " collided with " + name);

            _other.gameObject.GetComponent<WorldInteraction_Server>().HandleItemCollide(gameObject);
        }
    }

    public void OnPickedUp(bool _visible, bool _colliderOn)
    {
        onGround = false;
        gameObject.renderer.enabled = _visible;
        gameObject.GetComponent<SphereCollider>().enabled = _colliderOn;
    }

    public virtual short GetSubtype() { return -1; }
}
