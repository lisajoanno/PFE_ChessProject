using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(GameManager))]
[RequireComponent(typeof(PositionPawn))]
public class MyNetworkManager : NetworkManager
{
    private PositionPawn pp;
    private GameManager gm;

    short playerControllerHighestId = 0;

    void Start()
    {
        gm = GetComponent<GameManager>();
        pp = GetComponent<PositionPawn>();
    }

}