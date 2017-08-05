using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Speaker : MonoBehaviour {

	private SocketIOComponent socket;


	// Use this for initialization
	void Start () {

		socket = GameObject.FindObjectOfType<SocketIOComponent>().GetComponent<SocketIOComponent>();
		
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Z))
		{
			socket.On("beep", ButtonEvent);
		}
		
	}

	public void ButtonEvent(SocketIOEvent e)
	{
		Debug.Log(e.data["uid"]);
	}
}
