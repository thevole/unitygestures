using UnityEngine;
using System.Collections;

public class DemoMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI(){
		int startX=Screen.width/2-100;
		int startY=Screen.height/2-100;
		int width=200;
		int height=40;
		int spaceY=50;
		
		if(GUI.Button(new Rect(startX, startY, width, height), "RTS Camera")){
			Application.LoadLevel("RTSCam");
		}
		if(GUI.Button(new Rect(startX, startY+=spaceY, width, height), "Orbit Camera")){
			Application.LoadLevel("OrbitCam");
		}
		if(GUI.Button(new Rect(startX, startY+=spaceY, width, height), "Swipe Example")){
			Application.LoadLevel("SwipeDemo");
		}
		if(GUI.Button(new Rect(startX, startY+=spaceY, width, height), "General Tap/Click")){
			Application.LoadLevel("TapDemo");
		}
		if(GUI.Button(new Rect(startX, startY+=spaceY, width, height), "DPad Example")){
			Application.LoadLevel("DPad");
		}
		
		GUI.Label(new Rect(5, Screen.height-25, 500, 25), "Input.Touches version1.1 Demo by K.SongTan");
	}
}
