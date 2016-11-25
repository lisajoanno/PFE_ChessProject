using UnityEngine;
using System.Collections;

public class MoveController : MonoBehaviour {

    //the component managing the teams
    private TeamTurn teamTurn;

    void Start()
    {
        teamTurn = GetComponentInParent<TeamTurn>();
    }

    public bool Move(Pawn pawn, Square square)
    {
        // Light check that the pawn is on the right team
        if (pawn.Team != teamTurn.CurrentTeamPlaying) Debug.Log("Warning : the pawn you're moving is not of the right team.");
        
        
        //TODO verifier si un autre pion est sur la case

        pawn.Position = square.Position;
        Board board = pawn.GetComponent<Pawn>().Position.board;
        Vector2 pos = pawn.GetComponent<Pawn>().Position.coo;
        // Set of the rigth direction : forward compared to the case (not the board..)
        pawn.gameObject.transform.transform.up = -(board.GetSquare(pos).gameObject.transform.forward);
        // Set of the rigth position, on the right board
        pawn.gameObject.transform.position = board.GetSquare(pos).gameObject.transform.position;
        // The pawn needs to be on the case
        pawn.gameObject.transform.position += (pawn.gameObject.transform.transform.up) * 3;

        // The pawn was moved : the team can change
        teamTurn.ChangeTeam();

        return true;
    }
}
