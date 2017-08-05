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

        //Sender.Data["PlayerPosition"] = transform.position.ToString();

    }
}
