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
        goTest.GetComponent<Pawn>().Position = new Position(getBoards()[0], new Vector2(1, 0));
        GameObject hehe = PlacePawn(goTest);
        hehe.gameObject.name = "pawn hehe";
        hehe.GetComponent<Pawn>().Team = 0;
        hehe.GetComponent<Pawn>().Position = new Position(getBoards()[0], new Vector2(1, 0));
        

        goTest.GetComponent<Pawn>().Position = new Position(getBoards()[0], new Vector2(0, 1));
        GameObject hoho = PlacePawn(goTest);
        hoho.gameObject.name = "pawn hoho";
        hoho.GetComponent<Pawn>().Team = 0;
        hoho.GetComponent<Pawn>().Position = new Position(getBoards()[0], new Vector2(0, 1));
        
        goTest.GetComponent<Pawn>().Position = new Position(getBoards()[0], new Vector2(7, 6));
        GameObject hihi = PlacePawn(goTest);
        hihi.gameObject.name = "pawn hihi";
        hihi.GetComponent<Pawn>().Team = 1;
        hihi.GetComponent<Pawn>().Position = new Position(getBoards()[0], new Vector2(7, 6));
        
        goTest.GetComponent<Pawn>().Position = new Position(getBoards()[0], new Vector2(6, 7));
        GameObject haha = PlacePawn(goTest);
        haha.gameObject.name = "pawn haha";
        haha.GetComponent<Pawn>().Team = 1;
        haha.GetComponent<Pawn>().Position = new Position(getBoards()[0], new Vector2(6, 7));
        
    }

    /// <summary>
    /// Places a pawn at the right position.
    /// </summary>
    /// <param name="pawn"></param>
    GameObject PlacePawn(GameObject pawn)
    {
        GameObject pawnCreated = GameObject.Instantiate(pawn);
        Board board = pawn.GetComponent<Pawn>().Position.board;
        Vector2 pos = pawn.GetComponent<Pawn>().Position.coo;
        
        MoveController moveCtrl = GameObject.FindGameObjectWithTag("GamePlay").GetComponent<MoveController>();
        moveCtrl.MakeMove(pawnCreated.GetComponent<Pawn>(), board.GetSquare(pos).GetComponent<Square>());
        return pawnCreated;
    }

}
