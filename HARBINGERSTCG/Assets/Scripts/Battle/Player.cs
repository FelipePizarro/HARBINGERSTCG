using System;
using Mirror;
using UnityEngine;

public enum PlayerType { PLAYER, ENEMY };

public class Player : NetworkBehaviour
{
    [SyncVar(hook = nameof(UpdatePlayerName))] public string username; // SyncVar hook to call a command whenever a username changes (like when players load in initially).

    [Header("Stats")]
    [SyncVar] public int maxMana = 10;
    [SyncVar] public int currentMax = 0;
    [SyncVar] public int _mana = 0;

    // Quicker access for UI scripts
    [HideInInspector] public static Player localPlayer;
    [HideInInspector] public bool hasEnemy = false; // If we have set an enemy.
    [HideInInspector] public PlayerInfo enemyInfo; // We can't pass a Player class through the Network, but we can pass structs. 
    // We store all our enemy's info in a PlayerInfo struct so we can pass it through the network when needed.
    [HideInInspector] public static BattleController gameManager;
    [SyncVar, HideInInspector] public bool firstPlayer = false; // Is it player 1, player 2, etc.


    void UpdatePlayerName(string oldUser, string newUser)
    {
        // Update username
        username = newUser;

        // Update game object's name in editor (only useful for debugging).
        gameObject.name = newUser;
    }   
}
