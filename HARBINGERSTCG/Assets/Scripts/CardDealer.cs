using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDealer : NetworkBehaviour
{
    public My_PlayerController playerCtrlr;
    public GameObject ctrl;

    public 
    // Start is called before the first frame update
    void Start()
    {
        ctrl = GameObject.Find("BattleController");
        // StartCoroutine(wait4AllPlayer());
    }

    public void OnClick()
    {
        NetworkIdentity netID = NetworkClient.connection.identity;
        playerCtrlr = netID.GetComponent<My_PlayerController>();
        playerCtrlr.CmdDrawCard(3);
    }

    public void finishTurn()
    {
        /*
        NetworkIdentity netID = NetworkClient.connection.identity;
        playerCtrlr = netID.GetComponent<My_PlayerController>();
        playerCtrlr.writeTextLog("finishing turn");
        playerCtrlr.rpcFinish();*/
        
      //  ctrl.GetComponent<BattleController>().changeTurn(playerCtrlr.gameObject);
      //  ctrl.GetComponent<BattleController>().changeTurn();
       // playerCtrlr.GetComponent<My_PlayerController>().rpcFinish();
    }
}
