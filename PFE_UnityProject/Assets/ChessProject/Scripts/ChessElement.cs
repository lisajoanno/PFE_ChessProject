using UnityEngine;

/// <summary>
///     A ChessElement is an object necessited by a chess game
///     as the square of the board or the pawns
/// </summary>
public class ChessElement : MonoBehaviour {

    private Position position;            //the position of the element for the chessboard (eg (3, 4) for D5)
    public Position Position
    {
        get
        {
            return position;
        }
        set
        {
            position = value;
        }
    }
}
