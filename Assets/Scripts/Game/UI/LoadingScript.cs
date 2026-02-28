using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
/// <summary>
/// This class manages the loading of the main game scene
/// </summary>
public class LoadingScript : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Destroys all the network objects in the previous scene
        NetworkManager.Singleton.Shutdown();
        List<GameObject> netObjects =
            FindObjectsOfType<NetworkObject>().Select(obj => obj.transform.gameObject).ToList();

        foreach (var obj in netObjects)
        {
            Destroy(obj);
        }

        // Destroys the startgameAR object and networkmanager to reload the entire scene
        GameObject startGameARObject = FindObjectOfType<StartGameAR>().gameObject;
        Destroy(startGameARObject);
        Destroy(FindObjectOfType<NetworkManager>().transform.gameObject);
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);        
    }

}
