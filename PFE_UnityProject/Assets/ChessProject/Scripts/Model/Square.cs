using UnityEngine;
using System.Collections;

/// <summary>
/// Case as element of the board.
/// </summary>
public class Square : ChessElement
{
    // Use this for initialization
    void Start()
    {
        Color = GetComponent<Renderer>().material.color;
    }

    /// <summary>
    /// Allows to change the color of the chess element (needs to be overrid by each type of chess element).
    /// </summary>
    /// <param name="newColor">the color to set to the material</param>
    public override void SetChessElementColor(Color newColor)
    {
        Renderer[] tabChildren = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in tabChildren)
        {
            if (r.gameObject.tag.Equals("Square"))
                r.material.color = newColor;
        }
        GetComponent<Renderer>().material.color = newColor;
    }
}
