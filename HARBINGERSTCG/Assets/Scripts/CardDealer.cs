using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDealer : NetworkBehaviour
{
    public My_PlayerController playerCtrlr;

    public 
    // Start is called before the first frame update
    void Start()
    {
    }

    public void OnClick()
    {
        NetworkIdentity netID = NetworkClient.connection.identity;
        playerCtrlr = netID.GetComponent<My_PlayerController>();
        playerCtrlr.CmdDrawCard(3);
    }
}
