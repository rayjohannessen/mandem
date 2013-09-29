using UnityEngine;
using System.Collections;

public class Weapon : Item
{
    public enum eWeaponType { WT_KNIFE, NUM_WEAPON_TYPES }

    public eWeaponType weaponType;

    public void SetProperties(int _id, int _ownerID, eWeaponType _weaponType)
    {
        base.SetBaseProperties(Item.eItemType.IT_WEAPON, _id, _ownerID);

        weaponType = _weaponType;
    }

	void Start () 
    {
        tag = "Item";

        //
        // NOTE: this object will represent the actual item's mesh
        //          it's temporarily just an empty object in case it needs to be utilized
        //          as the real items may be at some point
        physicalObj = new GameObject();
        physicalObj.transform.position = transform.position;
        physicalObj.transform.parent = gameObject.transform;

        gameObject.AddComponent<SphereCollider>();
        gameObject.GetComponent<SphereCollider>().isTrigger = true;

        // NOTE: temporary sphere to represent items that are able to be picked up by players
        gameObject.AddComponent<MeshRenderer>();
        gameObject.AddComponent<MeshFilter>();
        GameObject tempMesh = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        gameObject.GetComponent<MeshFilter>().mesh = tempMesh.GetComponent<MeshFilter>().mesh;
        DestroyImmediate(tempMesh);
	}
	
	void Update () 
    {
	
	}

    public override short GetSubtype()
    {
        return (short)weaponType;
    }

    void OnTriggerEnter(Collider _other)
    {
        HandleTriggerEnter(_other);
    }
}
