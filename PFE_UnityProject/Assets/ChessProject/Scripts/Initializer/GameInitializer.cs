
using UnityEngine;
using System.Collections;

public class GameInitializer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponentInChildren<BoardInitializer>().Initialize();

        GameObject.FindGameObjectWithTag("GamePlay").GetComponent<TeamTurn>().AllBoard = GetComponentInChildren<BoardInitializer>().boards;

        GetComponentInChildren<PawnsInitializer>().Initialize();
    }

}

