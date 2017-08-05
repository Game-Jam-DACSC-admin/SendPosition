using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using UnityEngine.UI;

public class Sender : MonoBehaviour
{
    public Text Text;
    private SocketIOComponent socket;
    public Dictionary<string, string> Data = new Dictionary<string, string>();

    void Start()
    {

        /*         Data["x"] = "0";
                Data["y"] = "0";
                Data["z"] = "0"; */

        socket = GameObject.FindObjectOfType<SocketIOComponent>().GetComponent<SocketIOComponent>();
        socket.On("user", Groups);
        socket.On("boop", TestBoop);
        socket.On("error", TestError);
        socket.On("close", TestClose);
    }

    void Update()
    {


    }

    public void Sending()
    {
        socket.Emit("beep", new JSONObject(Data));
    }


    // public void TestOpen(SocketIOEvent e){
    // Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
    // }

    public void TestBoop(SocketIOEvent e)
    {
        Debug.Log(e.data);
        if (e.data["msg"])
        {
            Text.text = e.data["msg"].ToString();
            // users[0] = e.data["msg"].ToString();
            // Debug.Log(users);
        }
    }

    public void TestError(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
    }

    public void TestClose(SocketIOEvent e)
    {
        Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
    }


    public void Groups(SocketIOEvent e)
    {
        Debug.Log(e.data);
        // users[users.Length] = e.data["uid"].ToString();
        // Debug.Log(users);
    }
}
