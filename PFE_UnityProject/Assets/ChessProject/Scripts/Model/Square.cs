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
}
