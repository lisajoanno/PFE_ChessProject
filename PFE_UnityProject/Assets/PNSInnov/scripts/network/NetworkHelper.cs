using UnityEngine;
using UnityEngine.Networking;
using System;

[RequireComponent(typeof(NetworkManager))]
public class NetworkHelper : MonoBehaviour
{

    private NetworkManager net;

    [SerializeField]
    private String ip;

    // Use this for initialization
    void Start()
    {
        net = GetComponent<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        net.networkAddress = ip;
    }

}
