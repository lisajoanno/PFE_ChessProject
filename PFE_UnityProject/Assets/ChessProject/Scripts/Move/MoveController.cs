using UnityEngine;
using System.Collections;

public class MoveController : MonoBehaviour {

	public bool Move(Pawn pawn, Square square)
    {
        pawn.Position = square.Position;
        Board board = pawn.GetComponent<Pawn>().Position.board;
        Vector2 pos = pawn.GetComponent<Pawn>().Position.coo;
        // Set of the rigth direction : forward compared to the case (not the board..)
        pawn.gameObject.transform.transform.up = -(board.GetSquare(pos).gameObject.transform.forward);
        // Set of the rigth position, on the right board
        pawn.gameObject.transform.position = board.GetSquare(pos).gameObject.transform.position;
        // The pawn needs to be on the case
        pawn.gameObject.transform.position += (pawn.gameObject.transform.transform.up) * 3;

        return true;
    }
}
