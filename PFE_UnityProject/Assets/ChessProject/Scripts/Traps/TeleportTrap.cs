using UnityEngine;
using System.Collections;

public class TeleportTrap : Trap {
    [SerializeField]
    private Square output;
    private MoveController moveController;

    public override void Init()
    {
        base.Init();
        moveController = GameObject.FindGameObjectWithTag("GamePlay").GetComponent<MoveController>();
    }

	public override void Apply()
    {
        Pawn enemy = output.GetComponentInChildren<Pawn>();

        if (enemy)
        {
            DestroyImmediate(enemy);
        }

        Pawn me = square.GetComponentInChildren<Pawn>();
        moveController.MakeMove(me, output);
    }
}
