using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class LoadingScript : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Singleton.Shutdown();
        List<GameObject> netObjects =
            FindObjectsOfType<NetworkObject>().Select(obj => obj.transform.gameObject).ToList();

        foreach (var obj in netObjects)
        {
            Destroy(obj);
        }


        //GameObject startGameARObject = FindObjectOfType<StartGameAR>().gameObject;
        //Destroy(startGameARObject);
        //GameObject startGameARObject = FindObjectOfType<StartGameNonAR>().gameObject;
        //Destroy(startGameARObject);
        
        Destroy(FindObjectOfType<NetworkManager>().transform.gameObject);
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);        
    }

}
