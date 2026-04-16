using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
/// <summary>
/// This class stores the player data for each player
/// </summary>
public struct PlayerData : IEquatable<PlayerData>, INetworkSerializable
{
    public ulong clientID;
    public int score;
    public float lifePoints;
    public bool playerPlaced;
    public bool playerGuarding;


    public PlayerData(ulong clientID, int score, float lifePoints, bool playerPlaced, bool playerGuarding)
    {
        this.clientID = clientID;
        this.score = score;
        this.lifePoints = lifePoints;
        this.playerPlaced = playerPlaced;
        this.playerGuarding = playerGuarding;
    }
    
    
    // Checks if one player data is the same as this player data
    public bool Equals(PlayerData other)
    {
        return (
            other.playerPlaced == playerPlaced &&
            other.lifePoints == lifePoints &&
            other.score == score &&
            other.clientID == clientID
        );
    }

    // Serializes the playerdata across the network
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientID);
        serializer.SerializeValue(ref score);
        serializer.SerializeValue(ref lifePoints);
        serializer.SerializeValue(ref playerPlaced);
        serializer.SerializeValue(ref playerGuarding);
    }
}