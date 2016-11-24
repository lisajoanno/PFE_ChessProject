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

    // To get the tab of boards
    private Board[] getBoards()
    {
        return GameObject.FindGameObjectWithTag("BoardInitializer").GetComponent<BoardInitializer>().boards;
    }

    // Use this for initialization
    public void Initialize() {
        goTest.GetComponent<Pawn>().Position = new Position(getBoards()[0], new Vector2(1, 1));
        GameObject hehe = PlacePawn(goTest);
        hehe.GetComponent<Pawn>().Team = 0;
        hehe.GetComponent<Pawn>().Position = new Position(getBoards()[0], new Vector2(1, 1));

        goTest.GetComponent<Pawn>().Position = new Position(getBoards()[0], new Vector2(6, 6));
        GameObject hoho = PlacePawn(goTest);
        hoho.GetComponent<Pawn>().Team = 1;
        hoho.GetComponent<Pawn>().Position = new Position(getBoards()[0], new Vector2(6, 6));

        /**
        goTest.GetComponent<Pawn>().Position = new Position(getBoards()[0], new Vector2(7, 7));
        goTest.GetComponent<Pawn>().Team = 0;
        PlacePawn(goTest);
        goTest.GetComponent<Pawn>().Position = new Position(getBoards()[0], new Vector2(3, 2));
        goTest.GetComponent<Pawn>().Team = 0;
        PlacePawn(goTest);
        goTest.GetComponent<Pawn>().Position = new Position(getBoards()[2], new Vector2(1, 3));
        goTest.GetComponent<Pawn>().Team = 0;
        PlacePawn(goTest);
        goTest.GetComponent<Pawn>().Position = new Position(getBoards()[3], new Vector2(4, 2));
        goTest.GetComponent<Pawn>().Team = 1;
        PlacePawn(goTest);
        goTest.GetComponent<Pawn>().Position = new Position(getBoards()[0], new Vector2(0, 7));
        goTest.GetComponent<Pawn>().Team = 1;
        PlacePawn(goTest);
        goTest.GetComponent<Pawn>().Position = new Position(getBoards()[0], new Vector2(6, 2));
        goTest.GetComponent<Pawn>().Team = 1;
        PlacePawn(goTest);**/
    }

    /// <summary>
    /// Places a pawn at the right position.
    /// </summary>
    /// <param name="pawn"></param>
    GameObject PlacePawn(GameObject pawn)
    {
        Board board = pawn.GetComponent<Pawn>().Position.board;
        Vector2 pos = pawn.GetComponent<Pawn>().Position.coo;
        // Set of the rigth direction : forward compared to the case (not the board..)
        pawn.gameObject.transform.transform.up = -(board.GetSquare(pos).gameObject.transform.forward);
        // Set of the rigth position, on the right board
        pawn.gameObject.transform.position = board.GetSquare(pos).gameObject.transform.position;
        // The pawn needs to be on the case
        pawn.gameObject.transform.position += (pawn.gameObject.transform.transform.up) * 3;
        return GameObject.Instantiate(pawn);
    }

}
