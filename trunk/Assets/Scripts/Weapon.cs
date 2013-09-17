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
	
	}
	
	void Update () 
    {
	
	}
}
