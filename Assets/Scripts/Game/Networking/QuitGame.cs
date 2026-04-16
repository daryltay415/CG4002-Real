using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// This class manages the process of quiting the game
/// </summary>
public class QuitGame : NetworkBehaviour
{
    [SerializeField] private Button quitButton;
    [SerializeField] private Button tutQuitButton;
    [SerializeField] private Button gameplayQuitButton;
    // Start is called before the first frame update
    void Start()
    {
        quitButton.onClick.AddListener(RequestServerToQuitGameServerRpc);
        tutQuitButton.onClick.AddListener(RequestServerToQuitGameServerRpc);
        gameplayQuitButton.onClick.AddListener(RequestServerToQuitGameServerRpc);
    }

    // Loads the loading scene
    [ServerRpc(RequireOwnership = false)]
    void RequestServerToQuitGameServerRpc()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("LoadingScene", LoadSceneMode.Single);
    }
}
