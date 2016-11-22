using UnityEngine;
using System.Collections;

/// <summary>
///     Script to select a specified game object
///     from its layer
/// </summary>
public class Select{
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
    private Color colorLastSelected;        //the initial color of the object we selected
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
    public bool LaunchSelect(Vector3 position)
    {
        Ray rayToSelect = Camera.main.ScreenPointToRay(position);                           //create a ray from the specified position
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(rayToSelect, out hit, float.MaxValue, whatYouCanSelect))    //if the ray hit an object with a layer we can select
        {
            GameObject newSelected = hit.transform.gameObject;

            if (CanSelect(newSelected))
            {
                //reset the previously put color because we have selected another gameobject
                if (hasSthSelected)
                {
                    ResetColor();
                }
                SelectGameObject(newSelected);
                return true;
            }

            //we clicked on the same gameobject so we unselect it
            if (hasSthSelected && newSelected == lastSelected)
            {
                RemoveSelection();
                return true;
            }
            return false;
        }
        return false;
    }

    /// <summary>
    ///     Remove the current selection
    /// </summary>
    public void RemoveSelection()
    {
        ResetColor();
        lastSelected = null;
        colorLastSelected = Color.black;
        hasSthSelected = false;
    }

    /// <summary>
    ///     Revert the color of the last selected object into its original one
    /// </summary>
    private void ResetColor(){
        //we put back the original color of the previously selected game object
		Renderer[] tabChildren = lastSelected.GetComponentsInChildren<Renderer>();
		foreach (Renderer r in tabChildren) {
			r.material.color = colorLastSelected;
		}  
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
        colorLastSelected = lastSelected.GetComponentsInChildren<Renderer>()[0].material.color;

		//set the color of the game object to the specified select color
		Renderer[] tabChildren = lastSelected.GetComponentsInChildren<Renderer>();
		foreach (Renderer r in tabChildren) {
			r.material.color = selectColor;
		}

        hasSthSelected = true;
    }
}
