using UnityEngine;
using System.Collections;

public class SquareElement : ChessElement
{
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
        //Debug.Log("Couleur : " + color.ToString());
    }
}
