using UnityEngine;
using System.Collections;

public class DeathTrap : Trap {
    public override void Apply()
    {
        Debug.Log("you will die");
        Pawn pawn = square.GetComponentInChildren<Pawn>();
        DestroyImmediate(pawn.gameObject);
    }
}
