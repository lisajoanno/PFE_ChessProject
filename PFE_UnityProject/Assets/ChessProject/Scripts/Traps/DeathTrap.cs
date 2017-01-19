using UnityEngine;
using System.Collections;

public class DeathTrap : Trap {
    public override void Apply()
    {
        Debug.Log("you will die");
        Pawn pawn = square.GetComponentInChildren<Pawn>();
        DestroyImmediate(pawn.gameObject);
    }

    public override void ApplyEffectOnServer(ConnexionManager coo)
    {
        Debug.Log("You must send to the server a message to say that the pawn has been destroyed");
    }
}
