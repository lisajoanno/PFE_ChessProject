using UnityEngine;

public class ColliderManager : MonoBehaviour
{

    private bool pawnSelected;
    private bool squareSelected;
    private bool submitSelected;
    private GameObject go;
    
    void Start()
    {
        pawnSelected = false;
        squareSelected = false;
        submitSelected = false;
        go = new GameObject();
    }
    
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Pawn"))
        {
            pawnSelected = true;
            go = other.gameObject;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Square"))
        {
            squareSelected = true;
            go = other.gameObject;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Submit"))
        {
            submitSelected = true;
            Debug.Log("Submit hit !");
        }
    }

    public bool PawnIsSelected()
    {
        return pawnSelected;
    }

    public void SetPawnIsSelected(bool ps)
    {
        pawnSelected = ps;
    }

    public bool SquareIsSelected()
    {
        return squareSelected;
    }

    public void SetSquareIsSelected(bool sqs)
    {
        squareSelected = sqs;
    }

    public bool SubmitIsSelected()
    {
        return submitSelected;
    }

    public void SetSumbitIsSelected(bool subs)
    {
        submitSelected = subs;
    }

    public GameObject SelectedGameObject()
    {
        return go;
    }

}
