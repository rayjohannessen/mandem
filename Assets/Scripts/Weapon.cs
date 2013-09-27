using UnityEngine;
using System.Collections;

public class Weapon : Item
{
    public enum eWeaponType { WT_KNIFE, NUM_WEAPON_TYPES }

    public eWeaponType weaponType;

    public void SetProperties(int _id, eWeaponType _weaponType)
    {
        base.SetBaseProperties(Item.eItemType.IT_WEAPON, _id);

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
        gameObject.AddComponent<MeshRenderer>();
//         gameObject.AddComponent<MeshFilter>();
//         gameObject.GetComponent<MeshFilter>().mesh = GameObject.CreatePrimitive(PrimitiveType.Sphere).GetComponent<MeshFilter>().mesh;
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
        if (_other.gameObject.GetComponent<WorldInteraction_Server>())
        {
            Debug.Log(_other.gameObject.name + " collided with Weapon " + name);

            _other.gameObject.GetComponent<WorldInteraction_Server>().HandleItemCollide(gameObject);
        }
    }
}
