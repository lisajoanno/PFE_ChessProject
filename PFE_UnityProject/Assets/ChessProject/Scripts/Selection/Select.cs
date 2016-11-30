using UnityEngine;
using System.Collections;

/// <summary>
///     Script to select a specified game object
///     from its layer
/// </summary>
public class Select
{
    private LayerMask whatYouCanSelect;     //the Layer that the objects you want to select needs to have
    public LayerMask WhatYouCanSelect
    {
        set
        {
            whatYouCanSelect = value;
        }
    }

    private Color selectColor;              //the color that gameobject will have when selected
    public Color SelectColor
    {
        get
        {
            return selectColor;
        }
    }

    private bool hasSthSelected;            //true if a gameobject is selected
    public bool HasSthSelected
    {
        get
        {
            return hasSthSelected;
        }
    }


    /// <summary>
    ///     To select a gameobject we need to have the layer of the
    ///     objects we can select and the color it will have
    ///     when selected
    /// </summary>
    /// <param name="color"></param>
    /// <param name="whatYouCanSelect"></param>
    public Select(Color color, LayerMask whatYouCanSelect)
    {
        this.whatYouCanSelect = whatYouCanSelect;
        this.selectColor = color;
    }

    private GameObject lastSelected;        //the last game object we selected
    public GameObject LastSelected
    {
        get
        {
            return lastSelected;
        }
    }


    


    /// <summary>
    ///     Launch the selection of an object
    /// </summary>
    /// <param name="position">the position from where the raycast will begin</param>
    /// <returns>true if it selects something, false otherwise</returns>
    public void LaunchSelect(GameObject newSelected)
    {
        if (CanSelect(newSelected))
        {
            // we set the game object as the selected game object
            SelectGameObject(newSelected);
        }
        //we clicked on the same gameobject so we unselect it
        else
        {
            RemoveSelection();
        }
    }


    /// <summary>
    /// Recolors all component of the model : all the children renderers colors are set as the select color.
    /// </summary>
    public virtual void RecolorModel()
    {
        if (!LastSelected) return;
        lastSelected.gameObject.GetComponent<ChessElement>().SetChessElementColor(selectColor);
    }

    
    /// <summary>
    /// Resets all the components as their original color.
    /// </summary>
    public virtual void ResetColorModel()
    {
        if (!LastSelected) return;

        ChessElement ce = lastSelected.GetComponent<ChessElement>();
        ce.ResetChessElementColor();
    }

    /// <summary>
    ///     Remove the current selection
    /// </summary>
    public void RemoveSelection()
    {
        lastSelected = null;
        hasSthSelected = false;
    }
    

    /// <summary>
    ///     Do the selection on the newSelected object
    /// </summary>
    /// <param name="newSelected">the gameobject we want to select</param>
    /// <returns>true if we succeed to select something, false otherwise</returns>
    public virtual bool CanSelect(GameObject newSelected)
    {
        //if we select a gameobject different from the previous one
        return !hasSthSelected || lastSelected != newSelected;
    }


    /// <summary>
    ///     Put the color on the gameobject to explicite that it is selected
    /// </summary>
    /// <param name="newSelected"></param>
    private void SelectGameObject(GameObject newSelected)
    {
        //save the new selected object
        lastSelected = newSelected;
        hasSthSelected = true;
    }
}
