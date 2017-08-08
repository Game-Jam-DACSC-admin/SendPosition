using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using UnityEngine.UI;

public class Sender : MonoBehaviour
{
    UISystem UISystem;
    public GameObject Player;
    public Text Text;
    private SocketIOComponent socket;
    public Dictionary<string, string> Data = new Dictionary<string, string>();

    void Start()
    {
        UISystem = GameObject.FindObjectOfType<UISystem>();
        socket = GameObject.FindObjectOfType<SocketIOComponent>().GetComponent<SocketIOComponent>();
        socket.On("world", socketWorld);
        socket.On("group", socketGroup);
        socket.On("error", socketError);
        socket.On("close", socketClose);
    }

    public void Sending()
    {
        socket.Emit("world", new JSONObject(Data));
    }

    public void socketWorld(SocketIOEvent e)
    {
        GameObject PlayerObject = GameObject.Find(e.data["uid"].ToString());
        //Debug.Log(e.data);
         if(e.data["type"].ToString() == "\"position\"")
        { 
            //print("UseData");
            Vector3 position = PlayerObject.transform.position;
            float x = float.Parse(e.data["x"].ToString());
            float y = float.Parse(e.data["y"].ToString());
            float z = float.Parse(e.data["z"].ToString());
            position.x = x;
            position.y = y;
            position.z = z;
            PlayerObject.transform.position = position;
        }
        else if(e.data["type"].ToString() == "\"rotation\"")
        {
            //print("GotRotation");
            Quaternion rotation = PlayerObject.transform.rotation;
            float y = float.Parse(e.data["y"].ToString());
            rotation.y = y;
            PlayerObject.transform.eulerAngles = new Vector3(0, rotation.y, 0);
        }
        
    }

    public void socketError(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
    }

    public void socketClose(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
    }

    public void socketGroup(SocketIOEvent e)
    {
        Debug.Log(e.data);
        if(e.data["join"])
        {
            //Debug.Log("Join");
            GameObject Clone = Instantiate(Player);
            Clone.name = e.data["uid"].ToString();
        }
        else if(e.data["leave"]){
             GameObject.Destroy(GameObject.Find(e.data["uid"].ToString()), 1.0f);
        }
        // users[users.Length] = e.data["uid"].ToString();
        // Debug.Log(users);
    }
}
