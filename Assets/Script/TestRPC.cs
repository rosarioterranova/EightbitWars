using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRPC : MonoBehaviour {

    TextMesh mytxt;
    PhotonView pv;

    void Start()
    {
        mytxt = GetComponent<TextMesh>();
        pv = GetComponent<PhotonView>();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pv.RPC("RPC_Function", RpcTarget.AllBuffered, "Hi!");
            print("request RPC");
        }
    }

    [PunRPC] // method executed on remote clients, including caller with "AllBuffered" and other client that joins later
    void RPC_Function(string syncVar)
    {
        mytxt.text = syncVar;
    }
}
