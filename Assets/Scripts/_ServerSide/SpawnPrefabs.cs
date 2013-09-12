using UnityEngine;
using System.Collections;

public class SpawnPrefabs : uLink.MonoBehaviour
{
    public GameObject charOwnerPrefab = null;
    public GameObject charCreatorPrefab = null;
    public GameObject charProxyPrefab = null;

    public GameObject spawnLocation;

    public void SpawnPlayer(uLink.NetworkPlayer _player)
    {
        GameObject player = uLink.Network.Instantiate(_player, charProxyPrefab, charOwnerPrefab, charCreatorPrefab, spawnLocation.transform.position, spawnLocation.transform.rotation, 0, _player.id);
        // make it real simple to find the player object, use their playerID:
        player.name = _player.id.ToString();
	}
}
