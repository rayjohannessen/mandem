using UnityEngine;
using System.Collections;

/// <summary>
/// This item will probably just be destroyed once it's picked up, and added to the player data's
///     count of currency in each denomination.
/// </summary>
public class Currency : Item
{
    public enum eDenomination { DENOM_COPPER, DENOM_SILVER, DENOM_GOLD, NUM_DENOMINATIONS }
    public eDenomination denomination;
    public int amount;

    public void SetProperties(int _id, int _ownerID, int _amount, eDenomination _denom)
    {
        base.SetBaseProperties(Item.eItemType.IT_MONEY, _id, _ownerID);
        denomination = _denom;
        amount = _amount;
    }

	void Start () 
    {
        tag = "Item";

        gameObject.AddComponent<SphereCollider>();
        gameObject.GetComponent<SphereCollider>().isTrigger = true;

        // NOTE: temporary sphere to represent items that are able to be picked up by players
        gameObject.AddComponent<MeshRenderer>();
        gameObject.AddComponent<MeshFilter>();
        GameObject tempMesh = GameObject.CreatePrimitive(PrimitiveType.Cube);
        gameObject.GetComponent<MeshFilter>().mesh = tempMesh.GetComponent<MeshFilter>().mesh;
        DestroyImmediate(tempMesh);
	}
	
	void Update () 
    {
	
	}

    void OnTriggerEnter(Collider _other)
    {
        HandleTriggerEnter(_other);
    }

    public override short GetSubtype()
    {
        return (short)denomination;
    }
}
