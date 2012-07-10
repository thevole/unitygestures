using UnityEngine;
using System.Collections;

public class OrbitCam : MonoBehaviour {

	public Vector3 center=Vector3.zero;
	public float dist;
	
	private Vector2 orbitSpeed;
	private float rotateSpeed;
	private float zoomSpeed;
	
	public float orbitSpeedModifier=1;
	public float zoomSpeedModifier=5;
	public float rotateSpeedModifier=1;
	

	// Use this for initialization
	void Start () {
		dist=transform.position.z;
	}
	
	void OnEnable(){
		Gesture.onDraggingE += OnDragging;
		Gesture.onRotateE += OnRotate;
		Gesture.onPinchE += OnPinch;
	}
	
	void OnDisable(){
		Gesture.onDraggingE -= OnDragging;
		Gesture.onRotateE += OnRotate;
		Gesture.onPinchE += OnPinch;
	}
	
	// Update is called once per frame
	void Update () {
		dist+=Time.deltaTime*zoomSpeed*0.01f;
		dist=Mathf.Clamp(dist, -15, -3);
		
		transform.position=center;
		transform.rotation*=Quaternion.Euler(-orbitSpeed.y, orbitSpeed.x, rotateSpeed);
		transform.position=transform.TransformPoint(new Vector3(0, 0, dist));
		
		orbitSpeed*=(1-Time.deltaTime*3);
		rotateSpeed*=(1-Time.deltaTime*4f);
		zoomSpeed*=(1-Time.deltaTime*4);
	}
	
	void OnDragging(DragInfo dragInfo){
		orbitSpeed=dragInfo.delta*orbitSpeedModifier;
	}
	
	void OnRotate(float val){
		rotateSpeed=Mathf.Lerp(rotateSpeed, val*rotateSpeedModifier, 0.75f);
	}
	
	void OnPinch(float val){
		zoomSpeed-=val*zoomSpeedModifier;
	}
	
	
	private bool instruction=false;
	void OnGUI(){
		string title="free orbit camera, the camera will orbit around the object";
		GUI.Label(new Rect(150, 15, 500, 40), title);
		
		if(!instruction){
			if(GUI.Button(new Rect(10, 55, 130, 35), "Instruction On")){
				instruction=true;
			}
		}
		else{
			if(GUI.Button(new Rect(10, 55, 130, 35), "Instruction Off")){
				instruction=false;
			}
			
			GUI.Box(new Rect(10, 100, 400, 100), "");
			
			string instInfo="";
			instInfo+="- swipe or drag on screen to rotate the camera in x and y-axis\n";
			instInfo+="- pinch or using mouse wheel to zoom in/out\n";
			instInfo+="- rotate two fingers on screen to rotate the camera in z-axis\n";
			instInfo+="- single finger interaction can be simulate using left mosue button\n";
			instInfo+="- two fingers interacion can be simulate using right mouse button";
			
			GUI.Label(new Rect(15, 105, 390, 90), instInfo);
		}
	}
	
}

