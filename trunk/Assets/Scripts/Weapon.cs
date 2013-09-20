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
        physicalObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        physicalObj.transform.position = transform.position;
        physicalObj.transform.parent = gameObject.transform;
        physicalObj.collider.isTrigger = true;
	}
	
	void Update () 
    {
	
	}

    public override short GetSubtype()
    {
        return (short)weaponType;
    }
}
