using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
/// <summary>
/// Manages the UI in the game at different states of the game
/// </summary>
public class UIManager : NetworkBehaviour
{
    [SerializeField] private Canvas CreateGameCanvas;
    [SerializeField] private Canvas ControllerCanvas;
    [SerializeField] private Canvas RestartQuitCanvas;
    [SerializeField] private Canvas TutorialCanvas;
    [SerializeField] private TextMeshProUGUI winnerTextDisplay;
    [SerializeField] private TextMeshProUGUI loserTextDisplay;
    [SerializeField] private Canvas specialMeterDisplay;
    [SerializeField] private Canvas lifePointsDisplay;
    public Image specialMeterBar; //The level of the special meter


    private void Start()
    {
        ShowCreateGameCanvas();
        PlayerDataManager.Instance.OnPlayerDead += InstanceOnOnPlayerDead;
        //RestartGame.OnRestartGame += RestartGameOnOnRestartGame;
    }

    //private void RestartGameOnOnRestartGame()
    //{
    //    ShowPlayerControlsServerRpc();
    //}
//
    //[ServerRpc(RequireOwnership = false)]
    //void ShowPlayerControlsServerRpc()
    //{
    //    ShowPlayerControlsClientRpc();
    //}

    public void ShowPlayerControls()
    {
        CreateGameCanvas.gameObject.SetActive(false);
        ControllerCanvas.gameObject.SetActive(true);
        RestartQuitCanvas.gameObject.SetActive(false);
        TutorialCanvas.gameObject.SetActive(false);
        winnerTextDisplay.gameObject.SetActive(false);
        loserTextDisplay.gameObject.SetActive(false);
        specialMeterDisplay.gameObject.SetActive(true);
        lifePointsDisplay.gameObject.SetActive(true);
        Debug.Log("Why u no dispaly?");
    }

    public void ShowTutorialControls()
    {
        CreateGameCanvas.gameObject.SetActive(false);
        ControllerCanvas.gameObject.SetActive(true);
        RestartQuitCanvas.gameObject.SetActive(false);
        TutorialCanvas.gameObject.SetActive(true);
        winnerTextDisplay.gameObject.SetActive(false);
        loserTextDisplay.gameObject.SetActive(false);
        specialMeterDisplay.gameObject.SetActive(true);
        lifePointsDisplay.gameObject.SetActive(true);
    }

    private void InstanceOnOnPlayerDead(ulong obj)
    {
        if (IsServer)
        {
            PlayerIsDeadClientRpc(obj);
        }
    }

    // This function is called when one of the player is dead. It shows the winner/loser and quit button
    [ClientRpc]
    void PlayerIsDeadClientRpc(ulong id)
    {
        
        CreateGameCanvas.gameObject.SetActive(false);
        ControllerCanvas.gameObject.SetActive(false);
        RestartQuitCanvas.gameObject.SetActive(true);
        TutorialCanvas.gameObject.SetActive(false);
        specialMeterDisplay.gameObject.SetActive(false);
        lifePointsDisplay.gameObject.SetActive(false);
        if(NetworkManager.Singleton.LocalClientId != id)
        {
            winnerTextDisplay.gameObject.SetActive(true);
            loserTextDisplay.gameObject.SetActive(false);
        }
        else
        {
            winnerTextDisplay.gameObject.SetActive(false);
            loserTextDisplay.gameObject.SetActive(true);
        }
        
    }

    //Shows the menu screen
    void ShowCreateGameCanvas()
    {
        CreateGameCanvas.gameObject.SetActive(true);
        ControllerCanvas.gameObject.SetActive(false);
        RestartQuitCanvas.gameObject.SetActive(false);
        TutorialCanvas.gameObject.SetActive(false);
        winnerTextDisplay.gameObject.SetActive(false);
        loserTextDisplay.gameObject.SetActive(false);
        specialMeterDisplay.gameObject.SetActive(false);
        lifePointsDisplay.gameObject.SetActive(false);
    }

    public override void OnNetworkSpawn()
    {
        CreateGameCanvas.gameObject.SetActive(true);
        ControllerCanvas.gameObject.SetActive(false);
        RestartQuitCanvas.gameObject.SetActive(false);
        TutorialCanvas.gameObject.SetActive(false);
        winnerTextDisplay.gameObject.SetActive(false);
        loserTextDisplay.gameObject.SetActive(false);
        specialMeterDisplay.gameObject.SetActive(false);
        lifePointsDisplay.gameObject.SetActive(false);
    }

    public override void OnNetworkDespawn()
    {
        PlayerDataManager.Instance.OnPlayerDead -= InstanceOnOnPlayerDead;
        //RestartGame.OnRestartGame -= RestartGameOnOnRestartGame;
    }
}