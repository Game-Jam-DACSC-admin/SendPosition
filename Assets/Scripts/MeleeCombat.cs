using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;

public class MeleeCombat : MonoBehaviour {

    SocketIOComponent Socket;

    // Use this for initialization
    void Start () {
        Socket = GameObject.FindObjectOfType<SocketIOComponent>();
    }
	
	// Update is called once per frame
	void Update () {

        if (gameObject.tag == "Player")
        {
            Attack();
        }

    }

	public void Attack()
	{
		if(Input.GetMouseButtonDown(0))
		{
            if (!GetComponentInChildren<Animator>().GetBool("IsAttack"))
            {
                GetComponentInChildren<Animator>().SetTrigger("Attack");
            }

        }
		else if(gameObject.tag == "PlayerClone")
		{
			GetComponentInChildren<Animator>().SetTrigger("Attack");
		}
    }
}
