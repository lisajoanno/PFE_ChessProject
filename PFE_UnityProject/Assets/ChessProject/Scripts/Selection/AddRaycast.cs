using UnityEngine;
using System.Collections;

public class AddRaycast : MonoBehaviour {
    [SerializeField]
    private LayerMask whatFirstCanSelect;   //the layer of the first select script
    [SerializeField]
    private LayerMask whatSecondCanSelect;  //the layer of the second select script

    private Pawn pawn;
    private Square square;

    private bool hasHit;
    private GameObject go;

    LineRenderer lr;

    // Use this for initialization
    void Awake () {
        lr = gameObject.GetComponent<LineRenderer>();
        hasHit = false;
        go = new GameObject();
	}

    /*public float speed = 10.0F;
    public float rotationSpeed = 100.0F;*/
    
    void Update() {

        /*float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;
        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);*/

        //create a ray from the specified position
        Transform t = gameObject.transform;

        Vector3 fwd = /*gameObject.transform.TransformDirection*/(-Vector3.forward);
        Ray rayToSelect = new Ray(gameObject.transform.position, fwd);

        RaycastHit hit = new RaycastHit();
        //if the ray hit an object with a layer we can select
        Debug.DrawRay(gameObject.transform.position, 100000 * fwd, Color.green, 1);
        if (Physics.Raycast(rayToSelect, out hit, float.MaxValue, whatFirstCanSelect | whatSecondCanSelect))
        {
            GameObject newSelected = hit.transform.gameObject;
            //ManageSelection(newSelected);
            hasHit = true;
            go = newSelected;

        }

        /*if (Physics.Raycast(rayToSelect, out hit, float.MaxValue, submitSelect))
        {
            //we have to select 2 gameobject before the validation of the movement
            if (select[0].HasSthSelected && select[1].HasSthSelected)
            {
                Pawn pawn = select[0].LastSelected.GetComponent<Pawn>();
                //if (pawn.GetTeam == teamTurn.GetTeamTurn)
                {
                    Square square = select[1].LastSelected.GetComponent<Square>();
                    //do the move; eat the pawn at the selected square if needed

                    ResetAllSelection();
                    moveCtrl.Move(pawn, square);
                }
            }
        }*/
    }

    public bool HasHit()
    {
        return hasHit;
    }

    public void SetHasHit(bool hasHit)
    {
        this.hasHit = hasHit;
    }

    public GameObject SelectedGameObject()
    {
        return go;
    }
}
