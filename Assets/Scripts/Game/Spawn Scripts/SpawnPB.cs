using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPB : MonoBehaviour
{
    private GameObject player;
    public GameObject punchingBagPrefab;
    public void SpawnPunchingBag()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 spawnPos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 0.5f);
        GameObject punchingBag = Instantiate(punchingBagPrefab,spawnPos,Quaternion.identity);
        Debug.Log("Spawning punching bag");
    }
}
