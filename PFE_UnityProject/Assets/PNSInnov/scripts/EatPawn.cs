using UnityEngine;
using System.Collections.Generic;


[RequireComponent(typeof(GameGoal))]
public class EatPawn : MonoBehaviour {
    private GameGoal goal;

    void Start()
    {
        goal = GetComponent<GameGoal>();
    }

    /// <summary>
    ///   Eats the pawn at the given position. Makes it disappear from the board.
    ///   Checking if the pawn is on a different pawn is done before.
    /// </summary>
    /// <param name="allPawns"> list of all the pawns</param>
    /// <param name="pos">the position of the pawn to eat</param>
    public void Eat(IDictionary<Position, GameObject> allPawns, Position pos)
    {
        GameObject go;
        if (allPawns.TryGetValue(pos, out go))
        {
            // Checking if the pawn to be eaten is the target
            if (go == goal.GetGoal1 || go == goal.GetGoal2)
            {
                goal.ToDisplay = "La partie est finie, ";
                PawnElement pe = go.GetComponent<PawnElement>();
                goal.ToDisplay += "le joueur " + pe.GetTeam + " a gagné !";
                // TODO Stop the game ?
            }
            // Destroying the game object
            Destroy(go);
            // Destroying the GO does not set the position null apparently, so
            allPawns[pos] = null;
        }
    }
    
}
