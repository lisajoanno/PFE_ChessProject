using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(PositionPawns))]
public class GameGoal : MonoBehaviour {

    // ----- 1st goal
    [SerializeField]
    private int x1;
    [SerializeField]
    private int y1;
    [SerializeField]
    [Range(0,5)]
    private int board1;

    // ----- for 2nd goal
    [SerializeField]
    private int x2;
    [SerializeField]
    private int y2;
    [SerializeField]
    [Range(0, 5)]
    private int board2;


    private string toDisplay;
    public string ToDisplay
    {
        get
        {
            return toDisplay;
        }
        set
        {
            toDisplay = value;
        }
    }

    private GameObject goal1;
    public GameObject GetGoal1
    {
        get
        {
            return goal1;
        }
    }

    private GameObject goal2;
    public GameObject GetGoal2
    {
        get
        {
            return goal2;
        }
    }

    public void InitGameGoal(IDictionary<Position, GameObject> chessboard, Board[] boards)
    {
        goal1 = new GameObject();
        Board board1ToDo = boards[board1];

        goal2 = new GameObject();
        Board board2ToDo = boards[board2];

        Position pos1 = new Position(board1ToDo, new Vector2(x1, y1));
        Position pos2 = new Position(board2ToDo, new Vector2(x2, y2));

        if (chessboard[pos1] != null)
        {
            goal1 = chessboard[pos1];
			Renderer[] tabChildren = goal1.GetComponentsInChildren<Renderer>();
        }
        else
        {
            Debug.LogError("Il n'y a pas de pions à tuer à cette position, il n'y a donc pas de but à cette partie");
        }

        if (chessboard[pos2] != null)
        {
            goal2 = chessboard[pos2];
            Renderer[] tabChildren = goal2.GetComponentsInChildren<Renderer>();
        }
        else
        {
            Debug.LogError("Il n'y a pas de pions à tuer à cette position, il n'y a donc pas de but à cette partie");
        }

    }

    void OnGUI()
    {
        GUIStyle myStyle = new GUIStyle();
        myStyle.fontSize = 40;
        GUI.Label(new Rect(10, 10, 1000, 30), toDisplay, myStyle);
    }
}
