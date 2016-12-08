
using UnityEngine;
using System.Collections;

public class GameInitializer : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        GetComponentInChildren<BoardInitializer>().Initialize();
        GetComponentInChildren<PawnsInitializer>().Initialize();

        // THE BOARDS
        Board[] boards = GetComponentInChildren<BoardInitializer>().boards;

        // Set of the boards when needed
        GameObject.FindGameObjectWithTag("GamePlay").GetComponent<TeamTurn>().AllBoard = boards;
        int thisTeam = GameObject.FindGameObjectWithTag("ConnexionManager").GetComponent<ConnexionManager>().StartConnexion();
        GameObject.FindGameObjectWithTag("GamePlay").GetComponent<TeamTurn>().YourTeam = thisTeam;
    }

}

