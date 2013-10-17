using UnityEngine;
using System.Collections;

/// <summary>
/// Used by both the server and the client to create items in the level based on the preplaced itemSpawn's
/// </summary>
public class ItemFactory : uLink.MonoBehaviour 
{
    public GameObject itemPrefab;

    static public int currItemID = 0;

    public int[] currencyAmountMin = new int[(int)Currency.eDenomination.NUM_DENOMINATIONS];
    public int[] currencyAmountMax = new int[(int)Currency.eDenomination.NUM_DENOMINATIONS];

    ItemManager itemMngr;

    void Start()
    {
        Debug.Log("uLink.Network.isClient = " + uLink.Network.isClient);
        if (GameObject.Find("ItemManager"))
        {
            itemMngr = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        }

        // TODO::these could possibly be read in from a file on the server for ease of tweaking:
        for (int i = 0; i < (int)Currency.eDenomination.NUM_DENOMINATIONS; ++i)
        {
            currencyAmountMin[i] = 1;
            currencyAmountMax[i] = 100;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_pos">This is the position where the itemSpawn prefab has been placed to represent an available spawn location for an item.</param>
    /// <param name="_rot">Client assumes it gets no rotation info currently</param>
    /// <returns></returns>
    public GameObject GenerateRandomObject(Vector3 _pos, Quaternion _rot)
    {
        Item.eItemType val;

        /* TEMPORARY */
        if (UnityEngine.Random.Range(0f, 1f) < 0.5f)
        {
            val = Item.eItemType.IT_WEAPON;
        } 
        else
        {
            val = Item.eItemType.IT_MONEY;
        }
        //val = Item.eItemType.IT_WEAPON;// (Item.eItemType)UnityEngine.Random.Range(0, (int)Item.eItemType.NUM_ITEM_TYPES);

        Debug.Log("Generating item of type " + val + " at location " + _pos);

        GameObject item = GenerateItem(val, _pos, _rot);
        return item;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_pos"></param>
    /// <param name="_rot"></param>
    /// <param name="_subType"></param>
    /// <param name="_id"></param>
    /// <param name="_ownerID">-1 if no owner</param>
    /// <returns></returns>
    public GameObject GenerateItem(Item.eItemType _type, Vector3 _pos, Quaternion _rot, int _subType = -1, int _id = -1, int _ownerID = -1)
    {
        GameObject newObj = null;

        if (uLink.Network.isServer)
        {
            //Debug.Log("GenerateItem() for server, id = " + currItemID);
            _id = currItemID;
            ++currItemID;
        }
        else if (_id == -1 && uLink.Network.isClient)
        {
            Debug.LogError("GenerateItem() - ID is -1 !");
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

                    newObj.GetComponent<Weapon>().SetProperties(_id, _ownerID, wt);

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
                    newObj = Instantiate(itemPrefab, _pos, _rot) as GameObject;
                    newObj.AddComponent<Currency>();
                    
                    Currency.eDenomination denom;
                    if (_subType == -1)
                        denom = (Currency.eDenomination)_RandRange(0, (int)Currency.eDenomination.NUM_DENOMINATIONS);
                    else
                        denom = (Currency.eDenomination)_subType;

                    newObj.name = denom.ToString();
                    int amount = UnityEngine.Random.Range(currencyAmountMin[(int)denom], currencyAmountMax[(int)denom]);
                    newObj.GetComponent<Currency>().SetProperties(_id, _ownerID, amount, denom);

                    break;
                }
            case Item.eItemType.IT_POTION:
                {
                    break;
                }
        }
        
        Debug.Log("GenerateItem() - id is " + _id);

        if (itemMngr != null)
            itemMngr.AddItem(newObj.GetComponent<Item>());
        else
            Debug.Log(" - ItemManager is null.");

        return newObj;
    }

    int _RandRange(int _low, int _high)
    {
        return UnityEngine.Random.Range(_low, _high);
    }
}
