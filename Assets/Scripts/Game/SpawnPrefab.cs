using UnityEngine;

public class SpawnPrefab : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Transform parentObjectTrans;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Spawn();
    }

    void Spawn()
    {
        //Instantiate prefab
        GameObject prefab = Instantiate(prefabToSpawn, Vector3.zero, Quaternion.identity);

        prefab.transform.SetParent(parentObjectTrans, false);
    }
}
