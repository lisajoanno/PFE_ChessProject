using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PawnsInitializer : MonoBehaviour {

    // Pour le test (automatisé par la suite)
    //TODO virer ça
    public GameObject goTest;

    // List of all PAWNS of the game (by Position, so Board and Vector2)
    private IDictionary<Position, Pawn> positions = new Dictionary<Position, Pawn>();
    public IDictionary<Position, Pawn> GetPositions
    {
        get
        {
            return positions;
        }
    }

    private Board[] getBoards()
    {
        return GameObject.FindGameObjectWithTag("BoardInitializer").GetComponent<BoardInitializer>().boards;
    }

    // Use this for initialization
    public void Initialize() {
        goTest.GetComponent<Pawn>().Position = new Position(getBoards()[1], new Vector2(3, 2));
        PlacePawn(goTest);
        goTest.GetComponent<Pawn>().Position = new Position(getBoards()[2], new Vector2(1, 3));
        PlacePawn(goTest);
        goTest.GetComponent<Pawn>().Position = new Position(getBoards()[3], new Vector2(4, 2));
        PlacePawn(goTest);
        goTest.GetComponent<Pawn>().Position = new Position(getBoards()[4], new Vector2(0, 7));
        PlacePawn(goTest);
    }

    /// <summary>
    /// Places a pawn at the right position.
    /// </summary>
    /// <param name="pawn"></param>
    void PlacePawn(GameObject pawn)
    {
        Board board = pawn.GetComponent<Pawn>().Position.board;
        Vector2 pos = pawn.GetComponent<Pawn>().Position.coo;
        // Set of the rigth direction : forward compared to the case (not the board..)
        pawn.gameObject.transform.transform.up = -(board.GetSquare(pos).gameObject.transform.forward);
        // Set of the rigth position, on the right board
        pawn.gameObject.transform.position = board.GetSquare(pos).gameObject.transform.position;
        // The pawn needs to be on the case
        pawn.gameObject.transform.position += (pawn.gameObject.transform.transform.up) * 3;
        GameObject.Instantiate(pawn);
    }

}
