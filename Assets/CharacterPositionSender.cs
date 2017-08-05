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
        CheckIsWalkAndSend();

    }

    private void CheckIsWalkAndSend()
    {
        if(GetComponent<Rigidbody>().velocity.magnitude > .1f)
        {
            Sender.Sending();
        }
    }

    private void SendPosition()
    {
        Sender.Data["x"] = transform.position.x.ToString();
        Sender.Data["y"] = transform.position.y.ToString();
        Sender.Data["z"] = transform.position.z.ToString();

    }
}
