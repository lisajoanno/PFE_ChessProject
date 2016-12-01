using UnityEngine;
using System.Collections;
using System;
using System.Net.Sockets;

public class TestSocket : MonoBehaviour
{
    TcpClient client;
    NetworkStream stream;

    // Use this for initialization
    void Start()
    {
        //Debug.Log("HI!");
        Connect("https://bishop-chessproject.herokuapp.com", "{\"face_old\": 0, \"x_old\": 7, \"y_old\":7, \"face_new\": 1, \"x_new\": 7, \"y_new\":7}");
        //Debug.Log("Connected!");
        Write("{\"face_old\": 0, \"x_old\": 7, \"y_old\": 7, \"face_new\": 1, \"x_new\": 2, \"y_new\":3}");
        //Debug.Log("Write!");

        //stream.Close();
        //client.Close();
    }

    void Connect(String server, String message)
    {
        try
        {
            Int32 port = 1234;
            String ip = "10.212.119.247";
            client = new TcpClient(ip, port);
            stream = client.GetStream();

            Write(message);

            StartCoroutine("Read");

            // Close everything.
            //            stream.Close();
            //           client.Close();
        }
        catch (ArgumentNullException e)
        {
            Debug.Log("ArgumentNullException: " + e);
        }
        catch (SocketException e)
        {
            Debug.Log("SocketException: " + e);
        }

        Debug.Log("\nConnected plutot");
        //Console.Read();
    }

    public void Write(String message)
    {
        Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
        // Send the message to the connected TcpServer.
        stream.Write(data, 0, data.Length);
        Debug.Log("Sent: " + message);
    }

    public IEnumerator Read()
    {
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
                yield return null;
            }
            yield return null;
        }
    }
}
