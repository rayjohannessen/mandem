using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour 
{
    Dictionary<int, GameObject> players;

	void Start () 
    {
        players = new Dictionary<int, GameObject>();
	}

    public void AddPlayer(int _id, GameObject _playerObj)
    {
        if (!players.ContainsKey(_id))
        {
            players.Add(_id, _playerObj);
            Debug.Log("PlayerManager::AddPlayer() - player " + _playerObj + " with id " + _id + " added.");
        }
    }
    public void RemovePlayer(int _id)
    {
        if (players.ContainsKey(_id))
        {
            Debug.Log("PlayerManager::RemovePlayer() - player" + players[_id] + " with id " + _id + " removed.");
            players.Remove(_id);
        }
    }
    public GameObject GetPlayer(int _id)
    {
        if (players.ContainsKey(_id))
        {
            return players[_id];
        }
        Debug.Log("GetPlayer() - player with id " + _id + " does not exist in dictionary.");
        return null;
    }
    public void Clear()
    {
        players.Clear();
    }
}
