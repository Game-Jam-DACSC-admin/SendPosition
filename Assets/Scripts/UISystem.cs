using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

public class UISystem : MonoBehaviour {

    SocketIOComponent Socket;
    [Header("PlayerPrefab---JoinGame")]
    public GameObject Player;
    [Header("MainMenuCamera")]
    public Camera MainMenuCamera;
	[Header("-----------------")]
    public Canvas MainMenu;
    public Text RoomInformation;
    bool YouJoinInfoIsWork = false;

    // Use this for initialization
    void Start () {
        Socket = GameObject.FindObjectOfType<SocketIOComponent>();
    }
	
	// Update is called once per frame
	void Update () {
		if(Socket.IsConnected && !YouJoinInfoIsWork)
		{
            YouJoinInfo();
            print("Is connected");
        }	
	}

	public void JoinTheGameBTN()
	{
        Socket.Connect();
        MainMenuCamera.enabled = false;
        Instantiate(Player);
        MainMenu.gameObject.SetActive(false);
    }

	void YouJoinInfo()
	{
		Socket.Emit("world", new JSONObject("You have join the game"));
        YouJoinInfoIsWork = true;
    }

	public void NewJoinInfo(string ID)
	{
        RoomInformation.text = ID + "have join the game";
    }
}
