using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class ThirdPersonController : MonoBehaviour {

    SocketIOComponent Socket;
    Rigidbody RB;
    public float speed;

	public float WalkAcceleration;//行走時給角色的力
	public float WalkAirAcceleration = 0.1F;//不再地上時角色控制的nerf

	public float Reaction;//停止移動時給角色的反作用力
	float ReactionVx;//SmoothDamp
	float ReactionVz;//SmppthDamp

	public float MaxWalkSpeed;//角色行走的最高速度
	Vector2 HorizontalMovement;//平面移動的二維量

	public float JumpVelocity = 200;//跳躍時給角色的作用力
	public bool IsGrounded;//角色是否在地面上的布林值
	public float MaxSlope;//角色允許行走的最高角度

    public Camera MainCamera;

	/***********滑鼠控制**************/
    public float LookSensitivity = 5;//滑鼠靈敏度
    public float Y_Rotation;//Y軸旋轉量
    public float X_Rotation;//X軸旋轉量
    public bool AttackStart = false;
    public GameObject CameraRoot;

    Quaternion LastRotation;
    /*********************************/

    // Use this for initialization
    void Start () {
        Socket = GameObject.FindObjectOfType<SocketIOComponent>();
        RB = GetComponent<Rigidbody>();
    }

	void FixedUpdate()
	{
        Move();
        CameraRot();
        //Sending();
    }

    void Sending(Dictionary<string, string> Data)
    {
        Socket.Emit("world", new JSONObject(Data)); 
    }

    // Update is called once per frame
    void Move () 
	{
		Dictionary<string, string> Data = new Dictionary<string, string>();
		/********設定水平移動的二維量*********/
		HorizontalMovement.x = RB.velocity.x;
		HorizontalMovement.y = RB.velocity.z;
        /*************************************/

		/*******限制行走的最高速度************/
		if (HorizontalMovement.magnitude > MaxWalkSpeed)
		{
            /*當角色的速度大於最高速度時
            * 角色速度歸回1
            * 設定速度為最高速度*/
			HorizontalMovement = HorizontalMovement.normalized;
			HorizontalMovement *= MaxWalkSpeed;
		}
        /*************************************/

		//執行加速度
		RB.velocity = new Vector3(HorizontalMovement.x, RB.velocity.y, HorizontalMovement.y);

		/**********角色停止*******************/
		if(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0 && IsGrounded)
		{
            /*當玩家希望角色停止且角色在地面時
            * 玩家速度會以曲線減少至0*/
			RB.velocity = new Vector3(Mathf.SmoothDamp(RB.velocity.x, 0, ref ReactionVx, Reaction), RB.velocity.y, Mathf.SmoothDamp(RB.velocity.z, 0, ref ReactionVz, Reaction));
		}
        /*************************************/


		/*********鍵盤控制與未在地面控制******/ 
		if(IsGrounded)
		{
            /*當角色在地面時
            * 玩家控制與行走的作用力相乘*/
		    RB.AddRelativeForce(Input.GetAxis("Horizontal") * WalkAcceleration * Time.deltaTime, 0, Input.GetAxis("Vertical") * WalkAcceleration * Time.deltaTime);
            if (RB.velocity.magnitude > .1f)
            {
                Data["x"] = transform.position.x.ToString();
                Data["y"] = transform.position.y.ToString();
                Data["z"] = transform.position.z.ToString();
                Data["type"] = "position";
				print("SendPos");
                Sending(Data);
            }
        }
		else
		{
			if (RB.velocity.magnitude > .1f)
            {
                Data["x"] = transform.position.x.ToString();
                Data["y"] = transform.position.y.ToString();
                Data["z"] = transform.position.z.ToString();
                Data["type"] = "position";
				print("SendPosJump");
                Sending(Data);
            }
		}

		/***************跳躍BJ4*****************/
		if (Input.GetButtonDown("Jump") && IsGrounded == true)
		{
			RB.AddForce(0, JumpVelocity, 0);
		}
        /***************************************/
		
	}

	void CameraRot()
	{
        Dictionary<string, string> Data = new Dictionary<string, string>();
        Y_Rotation += Input.GetAxis("Mouse X") * LookSensitivity;
		X_Rotation += Input.GetAxis("Mouse Y") * LookSensitivity* -1;
        transform.rotation = Quaternion.Euler(0, Y_Rotation, 0);
        CameraRoot.transform.rotation = Quaternion.Euler(X_Rotation, Y_Rotation, 0);
        Data["y"] = (transform.rotation.y*180).ToString();
		Data["type"] = "rotation";
        //Debug.Log(transform.rotation);
        if (transform.rotation.y != LastRotation.y && transform.rotation.y-LastRotation.y !=0)
        {
            print("SendRot" + RB.angularVelocity.magnitude);
            Sending(Data);
        }
        LastRotation = transform.rotation;
    }

	void OnCollisionStay (Collision collisionInfo)
	{
		//print("CP" + collisionInfo.contacts[0].normal);//DEBUG
		//print("A" + Vector3.Angle(collisionInfo.contacts[0].normal, Vector3.up));//DEBUG

		if(Vector3.Angle(collisionInfo.contacts[0].normal, Vector3.up) < MaxSlope)
		{
            /*當玩家與訂面的接觸點的角度小於最高角度時
            * 布林值為真*/
			IsGrounded = true;
		}
		else
		{
            /*反之
            * 為假*/
			IsGrounded = false;
		}
	}
    /********************************************/
	
    /********當角色的碰撞器中沒有物體時*********/
	void OnCollisionExit (Collision collisionInfo)
	{
		IsGrounded = false;
	}
    /*******************************************/
}
