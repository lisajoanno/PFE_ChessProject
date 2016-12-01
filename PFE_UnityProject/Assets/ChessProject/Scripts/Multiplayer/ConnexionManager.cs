using UnityEngine;
using System.Collections;
using System;
using System.Net.Sockets;

public class ConnexionManager : MonoBehaviour
{
    private static String IP_SIMON = "10.212.119.247"; 
    private static Int32 PORT = 1234;


    TcpClient client;
    NetworkStream stream;

    // Use this for initialization
    void Start()
    {
        //Debug.Log(Builder(0, 1, 2, 1, 3, 4));

        Connect();
        

        Write(Builder(1, 2, 2, 1, 4, 4));
        StartCoroutine("Read");
        Write(Builder(1, 3, 3, 1, 5, 5));

        //stream.Close();
        //client.Close();
    }

    void Connect()
    {
        try
        {
            client = new TcpClient(IP_SIMON, PORT);
            stream = client.GetStream();
        }
        catch (ArgumentNullException e)
        {
            Debug.Log("ArgumentNullException: " + e);
        }
        catch (SocketException e)
        {
            Debug.Log("SocketException: " + e);
        }

        Debug.Log("\nConnected.");
    }


    private String Builder(int faceOld, int xOld, int yOld, int faceNew, int xNew, int yNew)
    {
        String res = "";
        res += "{\"face_old\": ";
        res += faceOld;
        res += ", \"x_old\": ";
        res += xOld;
        res += ", \"y_old\": ";
        res += yOld;
        res += ", \"face_new\": ";
        res += faceNew;
        res += ", \"x_new\": ";
        res += xNew;
        res += ", \"y_new\":";
        res += yNew;
        res += "}";

        return  res;
    }

    public void Write(String message)
    {
        Debug.Log("je write");
        Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
        // Send the message to the connected TcpServer.
        stream.Write(data, 0, data.Length);
        Debug.Log("Sent: " + message);
    }

    public IEnumerator Read()
    {
        Debug.Log("je read");
        Byte[] bytes = new Byte[256];
        String data = null;
        int i;
        for (;;)
        {
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                // Translate data bytes to a ASCII string.
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                Debug.Log("Received: " + data);
                yield return new WaitForSeconds(1);
            }
            yield return new WaitForSeconds(3);
        }
    }
}
