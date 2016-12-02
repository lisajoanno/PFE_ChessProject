
using UnityEngine;
using System.Collections;

public class GameInitializer : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        GetComponentInChildren<BoardInitializer>().Initialize();

        Board[] boards = GetComponentInChildren<BoardInitializer>().boards;

        GameObject.FindGameObjectWithTag("GamePlay").GetComponent<TeamTurn>().AllBoard = boards;

        GetComponentInChildren<PawnsInitializer>().Initialize();

        GameObject.FindGameObjectWithTag("ConnexionManager").GetComponent<ConnexionManager>().StartConnexion(boards);
    }

}

