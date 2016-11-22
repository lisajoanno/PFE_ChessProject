using UnityEngine;

/// <summary>
///     The Position of a chess element is :
///         - The board where it is
///         - the coordinate of the element on this board
/// </summary>
public struct Position{
    public Board board;
    public Vector2 coo;

    public Position(Board b, Vector2 coo)
    {
        this.board = b;
        this.coo = coo;
    }
}

/// <summary>
///     A ChessElement is an object necessited by a chess game
///     as the square of the board or the pawns
/// </summary>
public class ChessElement : MonoBehaviour {
    private Position elementPosition;            //the position of the element for the chessboard (eg (3, 4) for D5)
    public Position Position
    {
        get
        {
            return elementPosition;
        }
        set
        {
            elementPosition = value;
        }
    }
}
