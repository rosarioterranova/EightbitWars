using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    //SERIALIZED VARIABLES

    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject player2Prefab;

    //PUBLIC VARIABLES

    public static GameManager instance = null;

    //PRIVATE VARIABLES

    int currentScene;
    bool isGameLoaded;

    void Awake()
    {
        CreateSingleton();
    }

    private void CreateSingleton()
    {
        if (instance == null) //Check if instance already exists
        {
            SceneManager.sceneLoaded += OnSceneFinishedLoading; //set a listener on the scene loaded function
            instance = this; //if not, set instance to this
        }
        else if (instance != this) //If instance already exists and it's not this..
        {
            SceneManager.sceneLoaded -= OnSceneFinishedLoading; //remove the listener on the scene loaded function
            Destroy(gameObject); //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager
        }
        DontDestroyOnLoad(gameObject); //Sets this to not be destroyed when reloading scene
    }


    void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;
        if (currentScene == 1) //if the scne is the arena
        {
            if(PhotonNetwork.PlayerList[0]==PhotonNetwork.LocalPlayer) //if we are the first player on the room
                PhotonNetwork.Instantiate(playerPrefab.name, GameObject.FindGameObjectsWithTag("SpawnPoint")[0].transform.position, Quaternion.identity);
            else
                PhotonNetwork.Instantiate(player2Prefab.name, GameObject.FindGameObjectsWithTag("SpawnPoint")[1].transform.position, Quaternion.identity);
        }
    }
}
