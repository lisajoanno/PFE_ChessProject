using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PawnsInitializer : MonoBehaviour {

    // List of all PAWNS of the game (by Position, so Board and Vector2)
    private IDictionary<Position, Pawn> positions = new Dictionary<Position, Pawn>();
    public IDictionary<Position, Pawn> GetPositions
    {
        get
        {
            return positions;
        }
    }

    // Use this for initialization
    void Start () {
        // TODO
        // Recupérer les boards ?
        Pawn pawnTest = new Pawn();
        pawnTest.Position = new Position();
	}
}
