using UnityEngine;

public class ColliderManager : MonoBehaviour
{

    private bool hasCollide;
    private GameObject go;
    
    void Start()
    {
        hasCollide = false;
        go = new GameObject();
    }
    
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        go = other.gameObject;
        hasCollide = true;
    }

    void OnTriggerExit(Collider other)
    {
        hasCollide = false;
        go = null;
    }

    public bool HasCollide()
    {
        return hasCollide;
    }

    public void SetHasCollide(bool b)
    {
        hasCollide = b;
    }

    public GameObject SelectedGameObject()
    {
        return go;
    }

}
