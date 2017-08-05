using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;

public class CharacterPositionSender : MonoBehaviour {

    Sender Sender;

    // Use this for initialization
    void Start () {
        Sender = GameObject.FindObjectOfType<Sender>();
    }
	
	// Update is called once per frame
	void Update () {

        SendPosition();

    }

    private void SendPosition()
    {
        Sender.Data["X"] = transform.position.x.ToString();
        Sender.Data["Y"] = transform.position.y.ToString();
        Sender.Data["Z"] = transform.position.z.ToString();
    }
}
