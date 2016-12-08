using UnityEngine;
using System.Collections;
using System;
using System.Net.Sockets;

public class ConnexionManager : MonoBehaviour
{
    TcpClient client;
    NetworkStream stream;
    private static String IP_MAC = "10.212.119.247"; 
    private static String IP = "192.168.43.190";
    private static Int32 PORT = 1234;

    // The controller of moves, to execute the moves received from the other player
    MoveController moveController;

    /// <summary>
    /// Initialisation of the multiplayer connexion.
    /// </summary>
    /// <returns>the team of this client (0 or 1)</returns>
    public int StartConnexion()
    {

        // Init of move controller via the scene
        //moveController = GameObject.FindGameObjectWithTag("GamePlay").GetComponentInChildren<MoveController>();
        // Init of the connexion
        //Connect();
        // obligé de faire un write avant de lancer la coroutine, sinon ça plante
        //Write(Builder(0, 0, 0, 0, 0, 0));
        // On lance la coroutine du read
        //StartCoroutine(Read());

        //stream.Close();
        //client.Close();

        // TODO GET THE TEAM FROM SERVER
        return 1;
    }

    /// <summary>
    /// Initializes the connection between the client and the server.
    /// </summary>
    void Connect()
    {
        try
        {
            client = new TcpClient(IP, PORT);
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
        Debug.Log("Connected.");
    }

    /// <summary>
    /// Writes on the server a move that was just made.
    /// </summary>
    /// <param name="faceOld">the board of the pawn to be moved</param>
    /// <param name="xOld">the x coo of the pawn to be moved</param>
    /// <param name="yOld">the y coo of the pawn to be moved</param>
    /// <param name="faceNew">the board of the square where to move it</param>
    /// <param name="xNew">the x coo of the square where to move it</param>
    /// <param name="yNew">the y coo of the square where to move it</param>
    public void MakeAMoveOnServer(int faceOld, int xOld, int yOld, int faceNew, int xNew, int yNew)
    {
        Write(Builder(faceOld, xOld, yOld, faceNew, xNew, yNew));
    }

    /// <summary>
    /// Builds a JSON string to be sent on the server.
    /// </summary>
    /// <param name="faceOld">the board of the pawn to be moved</param>
    /// <param name="xOld">the x coo of the pawn to be moved</param>
    /// <param name="yOld">the y coo of the pawn to be moved</param>
    /// <param name="faceNew">the board of the square where to move it</param>
    /// <param name="xNew">the x coo of the square where to move it</param>
    /// <param name="yNew">the y coo of the square where to move it</param>
    /// <returns></returns>
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

    /// <summary>
    /// Writes on the server a message.
    /// </summary>
    /// <param name="message"></param>
    public void Write(String message)
    {
        Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
        // Send the message to the connected TcpServer.
        //TODO: uncomment line !!
        //stream.Write(data, 0, data.Length);
        Debug.Log("Sent: " + message);
    }

    /// <summary>
    /// Reads from the server a move 
    /// // TODO mieux le faire (pb de bloquage)?
    /// </summary>
    /// <returns></returns>
    public IEnumerator Read()
    {
        Byte[] bytes = new Byte[256];
        String data = null;
        int i;
        for (;;)
        {
            if (stream.DataAvailable)
            {
                i = stream.Read(bytes, 0, bytes.Length);
                // Translate data bytes to a ASCII string.
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                Debug.Log("Received: " + data);

                // Make the move received from the other player
                try
                {
                    moveController.MakeMoveFromOtherPlayer(data);
                }
                catch (Exception)
                {
                    Debug.Log("There was an exception in the move : the pawn or the square was not recognized ?");
                }
            }
            yield return null;
        }
    }


}
