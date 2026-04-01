using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
/// <summary>
/// This class manages all the player data of the connected clients in the game
/// </summary>
public class PlayerDataManager : NetworkBehaviour
{
    public static PlayerDataManager Instance;

    private NetworkList<PlayerData> allPlayerData;
    public NetworkVariable<ulong> winner;
    public int LIFEPOINTS = 20;
    private int clientLifePoints = 20;
    public int LIFEPOINTS_TO_REDUCE = 1;

    public event Action<ulong> OnPlayerDead;
    public event Action<ulong> OnPlayerHealthChanged;
    
    private void Awake()
    {
        allPlayerData = new NetworkList<PlayerData>();
        
        if (Instance != null && Instance != this)
        {
            Destroy(Instance);
        }

        Instance = this;
        
    }
    
    // Adds the spawned player's data into the list of all player datas
    public void AddPlacedPlayer(ulong id)
    {
        for (int i = 0; i < allPlayerData.Count; i++)
        {
            if (allPlayerData[i].clientID == id)
            {
                PlayerData newData = new PlayerData(
                    allPlayerData[i].clientID,
                    allPlayerData[i].score,
                    allPlayerData[i].lifePoints,
                    true,
                    allPlayerData[i].playerGuarding
                );
                
                allPlayerData[i] = newData;
            }
        }
    }

    // Checks if the player has placed his sprite
    public bool GetHasPlayerPlaced(ulong id)
    {
        for (int i = 0; i < allPlayerData.Count; i++)
        {
            if (allPlayerData[i].clientID == id)
            {
                return allPlayerData[i].playerPlaced;
            }
        }

        return false;
    }


    public override void OnNetworkSpawn()
    {
        if (UIButtonToggle.Instance.isCheatModeOn)
        {
            LIFEPOINTS = 999;
            if (!IsServer)
            {
                notifyGodmodeServerRpc();
            }
            
        }
        if (IsServer)
        {
            AddNewClientToList(NetworkManager.LocalClientId);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void notifyGodmodeServerRpc()
    {
        clientLifePoints = 999;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += AddNewClientToList;
        PlayerStateMachineMultiplayer.OnHitPlayer += PlayerHitboxOnHitPlayer;
        RestartGame.OnRestartGame += RestartGameCallback;
    }

    public void OnDisable()
    {
        if (IsServer)
        {
            allPlayerData.Clear();
            NetworkManager.Singleton.OnClientConnectedCallback -= AddNewClientToList;
        }
        PlayerStateMachineMultiplayer.OnHitPlayer -= PlayerHitboxOnHitPlayer;
        RestartGame.OnRestartGame -= RestartGameCallback;
    }

    // Restarts the game
    private void RestartGameCallback()
    {
        if(!IsServer) return;

        List<NetworkObject> playerObjects = FindObjectsOfType<PlayerMovement>()
            .Select(x => x.transform.GetComponent<NetworkObject>()).ToList();

        foreach (var playerobj in playerObjects)
        {
            playerobj.Despawn(); 
        }

        ResetNetworkList();
    }
    
    void ResetNetworkList()
    {
        for (int i = 0; i < allPlayerData.Count; i++)
        {
            PlayerData resetPLayer = new PlayerData(
                allPlayerData[i].clientID,
                playerPlaced: false,
                lifePoints: LIFEPOINTS,
                score: 0,
                playerGuarding: false
                );

            allPlayerData[i] = resetPLayer;
        }
    }

    // Gets the player health
    public float GetPlayerHealth(ulong id)
    {
        for (int i = 0; i < allPlayerData.Count; i++)
        {
            if (allPlayerData[i].clientID == id)
            {
                return allPlayerData[i].lifePoints;
            }
        }

        return default;
    }

    // Updates the player data on whether the player is guarding or not
    [ServerRpc(RequireOwnership = false)]
    public void PlayerGuardStateServerRpc(ulong id, bool isGuarding)
    {
        Debug.Log("Why u no guard");
        for (int i = 0; i < allPlayerData.Count; i++)
        {
            if (allPlayerData[i].clientID == id)
            {
                PlayerData newData = new PlayerData(
                            allPlayerData[i].clientID,
                            allPlayerData[i].score,
                            allPlayerData[i].lifePoints,
                            allPlayerData[i].playerPlaced,
                            isGuarding
                        );
                Debug.Log(allPlayerData[i].clientID + "is guarding: " + isGuarding);
                allPlayerData[i] = newData;
            }
        }
    }

    // Determines how much life points to reduce for the player that has been hit
    private void PlayerHitboxOnHitPlayer((ulong from, ulong to, int _lifePointsToReduce) ids)
    {
        if (IsServer && ids.from != ids.to)
        {
            for (int i = 0; i < allPlayerData.Count; i++)
            {
                if (allPlayerData[i].clientID == ids.to)
                {
                    if (allPlayerData[i].playerGuarding)
                    {
                        return;
                    }
                    else
                    {
                        int lifePointsToReduce = allPlayerData[i].lifePoints == 0 ? 0 : ids._lifePointsToReduce;
                        Debug.Log("lifepoints to reduce " + ids._lifePointsToReduce );
                        PlayerData newData = new PlayerData(
                            allPlayerData[i].clientID,
                            allPlayerData[i].score,
                            allPlayerData[i].lifePoints - lifePointsToReduce,
                            allPlayerData[i].playerPlaced,
                            allPlayerData[i].playerGuarding
                        );
                        if (newData.lifePoints <= 0)
                        {
                            winner.Value = ids.from;
                            OnPlayerDead?.Invoke(ids.to);
                        }
                        Debug.Log("Player got hit " + ids.to + " lifepoints left => " + newData.lifePoints +  " hit by " + ids.from);
                        allPlayerData[i] = newData;
                        TriggerDamageAnimation(ids.to);
                        
                        break;
                    }
                    
                }
            }
            
        }

        SyncReducePlayerHealthClientRpc(ids.to);
    }

    // Triggers the damage animation for the player that was hit
    private void TriggerDamageAnimation(ulong targetClientId)
    {
        // Find the object assigned to this client on the server
        if (NetworkManager.Singleton.ConnectedClients.TryGetValue(targetClientId, out var client))
        {
            if (client.PlayerObject != null)
            {
                // Send the NetworkObjectId (which exists on all clients)
                SyncDamageAnimationClientRpc(client.PlayerObject.NetworkObjectId);
            }
        }
    }

    // Syncs the damage animation for both the host and client
    [ClientRpc]
    void SyncDamageAnimationClientRpc(ulong targetNetObjId)
    {
        // Every client looks into their own "SpawnedObjects" list using the shared ID
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(targetNetObjId, out var netObj))
        {
            if (netObj.TryGetComponent(out PlayerStateMachineMultiplayer stateMachine))
            {
                stateMachine.takingDmg = 1;
                Debug.Log($"Animation triggered for object: {targetNetObjId}");
            }
        }
    }

    // Syncs the player's health for both the client and host
    [ClientRpc]
    void SyncReducePlayerHealthClientRpc(ulong hitID)
    {
        OnPlayerHealthChanged?.Invoke(hitID);
    }

    // Adds a new client to the list with a player data
    void AddNewClientToList(ulong clientID)
    {
        
        if(!IsServer) return;


        foreach (var playerData in allPlayerData)
        {
            if(playerData.clientID == clientID) return;
        }
        int newLifePoints;
        if(clientID != 0)
        {
            newLifePoints = clientLifePoints;
        }
        else
        {
            newLifePoints = LIFEPOINTS;
        }
        PlayerData newPlayerData = new PlayerData();
        newPlayerData.clientID = clientID;
        newPlayerData.score = 0;
        newPlayerData.lifePoints = newLifePoints;
        newPlayerData.playerPlaced = false;
        newPlayerData.playerGuarding = false;
        
        if(allPlayerData.Contains(newPlayerData)) return;
        
        allPlayerData.Add(newPlayerData);
        SyncPlayerListClientRpc();
        PrintAllPlayerPlayerList();
        

    }

    // Prints all the player's in the game
    void PrintAllPlayerPlayerList()
    {
        foreach (var playerData in allPlayerData)
        {
            Debug.Log("Player ID => " + playerData.clientID + " hasPlaced " + playerData.playerPlaced + " Called by " + NetworkManager.Singleton.LocalClientId);
        }
    }

    [ClientRpc]
    void SyncPlayerListClientRpc()
    {
        PrintAllPlayerPlayerList();
    }

}