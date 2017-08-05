using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Sender : MonoBehaviour {

	private SocketIOComponent socket;
    public Dictionary<string, string> Data = new Dictionary<string, string>();

    // Use this for initialization
    void Start () {

        Data.Add("PlayerPositionX", "");
		Data.Add("PlayerPositionY", "");
		Data.Add("PlayerPositionZ", "");

        socket = GameObject.FindObjectOfType<SocketIOComponent>().GetComponent<SocketIOComponent>();

        socket.On("open", TestOpen);
		socket.On("boop", TestBoop);
		socket.On("error", TestError);
		socket.On("close", TestClose);


    }
	
	// Update is called once per frame
	void Update () {
        StartCoroutine(Sending());
    }

	IEnumerator Sending()
	{
        socket.Emit("beep",new JSONObject(Data["PlayerPositionX"]));
		yield return new WaitForSeconds(.1f);
		socket.Emit("beep",new JSONObject(Data["PlayerPositionY"]));
		yield return new WaitForSeconds(.1f);
		socket.Emit("beep",new JSONObject(Data["PlayerPositionZ"]));
		yield return new WaitForSeconds(2f);
    }

		public void TestOpen(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
	}
	
	public void TestBoop(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Boop received: " + e.name + " " + e.data);

		if (e.data == null) { return; }

		Debug.Log(
			"#####################################################" +
			"THIS: " + e.data.GetField("this").str +
			"#####################################################"
		);
	}
	
	public void TestError(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
	}
	
	public void TestClose(SocketIOEvent e)
	{	
		Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
	}
}
