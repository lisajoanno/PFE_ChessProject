using UnityEngine;

/// <summary>
///     The Position of a chess element is :
///         - The board where it is
///         - the coordinate of the element on this board
/// </summary>
public struct Position
{
    public Board board;
    public Vector2 coo;

    public Position(Board b, Vector2 coo)
    {
        this.board = b;
        this.coo = coo;
    }
}