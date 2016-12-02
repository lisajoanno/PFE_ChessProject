using UnityEngine;
using System.Collections;
using System;
using System.Net.Sockets;

public class ConnexionManager : MonoBehaviour
{
    TcpClient client;
    NetworkStream stream;
    private static String IP_SIMON = "10.212.119.247"; 
    private static Int32 PORT = 1234;

    // The controller of moves, to execute the moves received from the other player
    MoveController moveController;

    // The boards are needed to make a Position
    private Board[] allBoards;


    // Use this for initialization
    public void StartConnexion(Board[] boards)
    {
        allBoards = boards;
        // Init of move controller via the scene
        moveController = GameObject.FindGameObjectWithTag("GamePlay").GetComponentInChildren<MoveController>();

        Connect();

        Write(Builder(0, 0, 0, 0, 0, 0));
        StartCoroutine(Read());


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

    public void MakeAMove(int faceOld, int xOld, int yOld, int faceNew, int xNew, int yNew)
    {
        Write(Builder(faceOld, xOld, yOld, faceNew, xNew, yNew));
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
                    Debug.Log("There was an exception in the move : the pawn or the square were no recognized ?");
                }

                yield return new WaitForSeconds(10);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }


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
        DataReceived pi = DataReceived.CreateFromJSON(data);

        int faceOld = pi.face_old;
        int xOld = pi.x_old;
        int yOld = pi.y_old;
        int faceNew = pi.face_new;
        int xNew = pi.x_new;
        int yNew = pi.y_new;

        Position oldPos = new Position(allBoards[faceOld], new Vector2(xOld, yOld));
        Position newPos = new Position(allBoards[faceNew], new Vector2(xNew, yNew));

        Position[] positions = new Position[2];
        positions[0] = oldPos;
        positions[1] = newPos;

        return positions;
    }

}
