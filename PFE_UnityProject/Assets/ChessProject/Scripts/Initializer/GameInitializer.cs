using UnityEngine;
using System.Collections;

public class GameInitializer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponentInChildren<BoardInitializer>().Initialize();
        GetComponentInChildren<PawnsInitializer>().Initialize();
    }

}
