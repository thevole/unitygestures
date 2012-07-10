using UnityEngine;
using System.Collections;

public class DPad : MonoBehaviour {

	public GUITexture up;
	public GUITexture down;
	public GUITexture left;
	public GUITexture right;
	
	public Transform controlObject;
	
	// Use this for initialization
	void Start () {
	
	}
	
	void OnEnable(){
		Gesture.onMouse1E += Pressed;
		Gesture.onTouchE += Pressed;
	}
	
	void OnDisable(){
		Gesture.onMouse1E -= Pressed;
		Gesture.onTouchE -= Pressed;
	}
	
	// Update is called once per frame
	void Update () {
		//limit the position of the control object
		float x=controlObject.position.x;
		float z=controlObject.position.z;
		
		x=Mathf.Clamp(x, -5, 5);
		z=Mathf.Clamp(z, -5, 5);
		
		controlObject.position=new Vector3(x, controlObject.position.y, z);
	}
	
	//call when the screen is touched/clicked
	void Pressed(Vector2 pos){
		
		//set a zero vector
		Vector3 moveDir=Vector3.zero;
		
		//if any of the button is pressed, set the corresponding  move direction
		if(up.HitTest(pos)){
			moveDir+=new Vector3(0, 0, 1);
		}
		if(down.HitTest(pos)){
			moveDir+=new Vector3(0, 0, -1);
		}
		if(left.HitTest(pos)){
			moveDir+=new Vector3(-1, 0, 0);
		}
		if(right.HitTest(pos)){
			moveDir+=new Vector3(1, 0, 0);
		}
		
		//normalized the total moveDir
		moveDir=moveDir.normalized;
		
		//move the controlObject according to the input move direction
		controlObject.Translate(moveDir*Time.deltaTime*3);
	}
	
	
	void OnGUI(){
		string title="DPad demo, click/touch the arrows on the bottom left corner to move the sphere.";
		GUI.Label(new Rect(150, 15, 500, 40), title);
	}
}
