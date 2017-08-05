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

    void Start () {

/*         Data["x"] = "0";
        Data["y"] = "0";
        Data["z"] = "0"; */

      socket = GameObject.FindObjectOfType<SocketIOComponent>().GetComponent<SocketIOComponent>();
      socket.On("open", TestOpen);
      socket.On("boop", TestBoop);
      socket.On("error", TestError);
      socket.On("close", TestClose);
    }
	
	void Update () {
    StartCoroutine(Sending());
  }

	IEnumerator Sending(){
    yield return new WaitForSeconds(1);
    // Debug.Log(Data.GetType());
    socket.Emit("beep", new JSONObject(Data));
  }

  public void TestOpen(SocketIOEvent e){
		Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
	}
	
	public void TestBoop(SocketIOEvent e){
		Debug.Log(e.data["uid"]);
        Text.text = e.data["uid"].ToString();
	}
	
	public void TestError(SocketIOEvent e){
		Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
	}
	
	public void TestClose(SocketIOEvent e){
		Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
	}
}
