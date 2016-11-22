using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Control : NetworkBehaviour
{
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
            return;
	}
}
