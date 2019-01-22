using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviourPunCallbacks
{

    //SERIALIZED VARIABLES

    [SerializeField] byte maxPlayersPerRoom = 2;
    [SerializeField] GameObject battleButton;
    [SerializeField] GameObject connectingText;

    Text lobbyStatus;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true; //all clients in the same room sync their level automatically with master client
    }

    void Start()
    {
        battleButton.SetActive(false);
        lobbyStatus = connectingText.GetComponent<Text>();
        Connect();
    }

    public void Connect()
    {
        if (!PhotonNetwork.IsConnected) //join a room if we are already connected to the photon network
        {
            PhotonNetwork.ConnectUsingSettings(); //we must first connect to Photon Online Server.
        }
    }

    //CALLBACKS FOR CONNECTION

    public override void OnConnectedToMaster()
    {
        print("OnConnectedToMaster() was called by PUN");
        battleButton.SetActive(true);
        lobbyStatus.text = "Connected";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("OnDisconnected() was called by PUN with reason "+ cause);
    }

    //CALLBACKS FOR ROOM JOINING

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("OnJoinRandomFailed() was called by PUN. No random room available, so we create one");
        lobbyStatus.text = "No room avaiable, creating a new one...";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        print("OnJoinedRoom() called by PUN. Now this client is in the room named: "+PhotonNetwork.CurrentRoom.Name);
        lobbyStatus.text = "Room joined";
        LoadArena();
    }

    //FUNCTIONS

    public void SearchBattle()
    {
        PhotonNetwork.JoinRandomRoom(); //If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
    }

    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            print("Trying to Load a level but we are not the master Client");
        }
        print("Loading Level Arena");
        PhotonNetwork.LoadLevel("Arena");
    }
}
