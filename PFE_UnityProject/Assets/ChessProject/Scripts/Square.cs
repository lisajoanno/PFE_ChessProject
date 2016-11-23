using UnityEngine;
using System.Collections;

/// <summary>
/// Case as element of the board.
/// </summary>
public class Square : MonoBehaviour {

    // The board the case is on
    private Board board;

    // The color
    private Color color = new Color();
    public Color Color
    {
        get
        {
            return color;
        }
        set
        {
            this.color = value;
        }
    }

    // Use this for initialization
    void Start () {
        this.color = GetComponent<Renderer>().material.color;
    }

    public void SetBoard(Board b)
    {
        this.board = b;
    }
}
