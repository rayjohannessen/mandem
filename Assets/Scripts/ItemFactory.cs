using UnityEngine;
using System.Collections;

/// <summary>
/// Used by both the server and the client to create items in the level based on the preplaced itemSpawn's
/// </summary>
public class ItemFactory : uLink.MonoBehaviour 
{
    public GameObject itemPrefab;

    static public int currItemID = 0;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_pos">This is the position where the itemSpawn prefab has been placed to represent an available spawn location for an item.</param>
    /// <param name="_rot">Client assumes it gets no rotation info currently</param>
    /// <returns></returns>
    public GameObject GenerateRandomObject(Vector3 _pos, Quaternion _rot)
    {
        Item.eItemType val = Item.eItemType.IT_WEAPON;// (Item.eItemType)UnityEngine.Random.Range(0, (int)Item.eItemType.NUM_ITEM_TYPES);

        Debug.Log("Generating item of type " + val + " at location " + _pos);

        return GenerateItem(val, _pos, _rot);
    }

    public GameObject GenerateItem(Item.eItemType _type, Vector3 _pos, Quaternion _rot, int _subType = -1, int _id = -1)
    {
        GameObject newObj = null;
        
        if (Network.isServer)
        {
            _id = currItemID;
            ++currItemID;
        }
        else if (_id == -1 && Network.isClient)
        {
            Debug.LogError("GenerateRandomObject() - ID is -1 !");
            return null;
        }

        switch (_type)
        {
            case Item.eItemType.IT_WEAPON:
                {
                    newObj = Instantiate(itemPrefab, _pos, _rot) as GameObject;
                    newObj.AddComponent<Weapon>();

                    Weapon.eWeaponType wt;
                    if (_subType == -1)
                        wt = (Weapon.eWeaponType)_RandRange(0, (int)Weapon.eWeaponType.NUM_WEAPON_TYPES);
                    else
                        wt = (Weapon.eWeaponType)_subType;

                    // TODO :: probably create an array of nice display names for the subtypes...
                    newObj.name = wt.ToString();

                    newObj.GetComponent<Weapon>().SetProperties(_id, wt);

                    break;
                }
            case Item.eItemType.IT_CONTAINER:
                {
                    break;
                }
            case Item.eItemType.IT_JEWELRY:
                {
                    break;
                }
            // TODO :: is money actually going to be an object that spawns like the others??
            case Item.eItemType.IT_MONEY:
                {
                    break;
                }
            case Item.eItemType.IT_POTION:
                {
                    break;
                }
        }

        return newObj;
    }

    int _RandRange(int _low, int _high)
    {
        return UnityEngine.Random.Range(_low, _high);
    }
}
