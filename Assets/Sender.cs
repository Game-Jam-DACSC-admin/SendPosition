using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Sender : MonoBehaviour {

	private SocketIOComponent socket;
    public Dictionary<string, string> Data = new Dictionary<string, string>();

    // Use this for initialization
    void Start () {

        Data.Add("PlayerPosition", Vector3.zero.ToString());

    }
	
	// Update is called once per frame
	void Update () {
        StartCoroutine(Sending());
    }

	IEnumerator Sending()
	{
		yield return new WaitForSeconds(1f);
        socket.Emit("beep",new JSONObject(Data["PlayerPosition"]));
    }
}
