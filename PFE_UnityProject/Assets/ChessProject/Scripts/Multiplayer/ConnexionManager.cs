using UnityEngine;
using System.Collections;
using System;
using System.Net.Sockets;

public class ConnexionManager : MonoBehaviour
{
    TcpClient client;
    NetworkStream stream;
    private static String IP = "10.212.119.247"; 
    private static Int32 PORT = 1234;

    // The controller of moves, to execute the moves received from the other player
    MoveController moveController;

    // The boards are needed to make a Position
    private Board[] allBoards;


    /// <summary>
    /// Initialisation of the multiplayer connexion.
    /// </summary>
    /// <param name="boards"></param>
    public void StartConnexion(Board[] boards)
    {
        allBoards = boards;
        // Init of move controller via the scene
        moveController = GameObject.FindGameObjectWithTag("GamePlay").GetComponentInChildren<MoveController>();
        // Init of the connexion
        Connect();
        // obligé de faire un write avant de lancer la coroutine, sinon ça plante
        Write(Builder(0, 0, 0, 0, 0, 0));
        // On lance la coroutine du read
        StartCoroutine(Read());

        //stream.Close();
        //client.Close();
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

    public void MakeAMove(int faceOld, int xOld, int yOld, int faceNew, int xNew, int yNew)
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
        stream.Write(data, 0, data.Length);
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
        //for (;;)
        {
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                
                // Translate data bytes to a ASCII string.
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                Debug.Log("Received: " + data);

                // Make the move received from the other player
                try {
                    moveController.MakeMoveFromOtherPlayer(TranslatePositions(data));
                } catch (Exception)
                {
                    Debug.Log("There was an exception in the move : the pawn or the square was not recognized ?");
                }

                yield return new WaitForSeconds(10);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    /// <summary>
    /// A class used to parse the data received from the server.
    /// It is serializable and an object is creatable from a json string.
    /// </summary>
    [System.Serializable]
    public class DataReceived
    {

        public int face_old;
        public int x_old;
        public int y_old;
        public int face_new;
        public int x_new;
        public int y_new;

        public static DataReceived CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<DataReceived>(jsonString);
        }
    }

    /// <summary>
    /// From the data received from the server, creates 2 Position :
    ///     - the first one is the original position (so the pawn)
    ///     - the second one is the square where to move it.
    /// </summary>
    /// <param name="data">the JSON string (WARNING : string with lower s)</param>
    /// <returns>a table of 2 positions</returns>
    private Position[] TranslatePositions(string data)
    {
        DataReceived dataReceived = DataReceived.CreateFromJSON(data);
        Position oldPos = new Position(allBoards[dataReceived.face_old], new Vector2(dataReceived.x_old, dataReceived.y_old));
        Position newPos = new Position(allBoards[dataReceived.face_new], new Vector2(dataReceived.x_new, dataReceived.y_new));
        Position[] positions = new Position[2];
        positions[0] = oldPos;
        positions[1] = newPos;
        return positions;
    }

}
