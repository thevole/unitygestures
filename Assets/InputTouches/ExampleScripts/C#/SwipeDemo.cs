using UnityEngine;
using System.Collections;

public class SwipeDemo : MonoBehaviour {

	public Transform cursorIndicator;
	public Transform swipeIndicator;
	public Transform projectileObject;
	
	private Rigidbody body;
	
	public GUIText label;
	private float labelTimer=-1;
	
	void Start(){
		body=projectileObject.gameObject.GetComponent<Rigidbody>();
	}
	
	void OnEnable(){
		Gesture.onTouchE += OnOn;
		Gesture.onMouse1E += OnOn;
		Gesture.onSwipeE += OnSwipe;
	}
	
	void OnDisable(){
		Gesture.onTouchE -= OnOn;
		Gesture.onMouse1E -= OnOn;
		Gesture.onSwipeE -= OnSwipe;
	}
	
	void OnSwipe(SwipeInfo sw){
		//position the projectile object at the start point of the swipe
		Vector3 p=Camera.main.ScreenToWorldPoint(new Vector3(sw.startPoint.x, sw.startPoint.y, 35));
		projectileObject.position=p;
		
		//clear the projectile current velocity before apply a new force in the swipe direction, take account of the swipe speed
		body.velocity=new Vector3(0, 0, 0);
		body.AddForce(new Vector3(sw.direction.x, 0, sw.direction.y) * sw.speed*0.0035f);
		
		//show the swipe info 
		string labelText="Swipe Detected, ";
		if(sw.isMouse) labelText+="mouse "+sw.index.ToString()+"\n\n";
		else labelText+="finger "+sw.index.ToString()+"\n\n";
		
		//labelText+="\n\n";
		labelText+="direction: "+sw.direction+"\n";
		labelText+="angle: "+sw.angle.ToString("f1")+"\n";
		labelText+="speed: "+sw.speed.ToString("f1")+"\n";
		label.text=labelText;
		
		//if the label is previous cleared, re-initiate the coroutine to clear it
		if(labelTimer<0){
			StartCoroutine(ClearLabel());
		}
		//else just extend the timer
		else labelTimer=5;
		
		StartCoroutine(ShowSwipeIndicator(sw));
	}
	
	IEnumerator ShowSwipeIndicator(SwipeInfo sw){
		//position the projectile object at the start point of the swipe
		Vector3 p=Camera.main.ScreenToWorldPoint(new Vector3(sw.startPoint.x, sw.startPoint.y, 5));
		swipeIndicator.position=p;
		
		float t=0;
		while(t<=1){
			p=Vector2.Lerp(sw.startPoint, sw.endPoint, t);
			p=Camera.main.ScreenToWorldPoint(new Vector3(p.x, p.y, 5));
			//Debug.Log(sw.startPoint+"  "+sw.endPoint+"   "+p);
			swipeIndicator.position=p;
			t+=0.2f;
			yield return null;
		}
		
		
	}
	
	//clear the lable, if no new input has been assigned within 5 sec, the label will be cleared
	IEnumerator ClearLabel(){
		labelTimer=5;
		while(labelTimer>0){
			labelTimer-=Time.deltaTime;
			yield return null;
		}
		label.text="";
	}
	
	
	//called when the touch or a click is detected
	void OnOn(Vector2 pos){
		//place the curose at the position where the tap/click take place
		Vector3 p=Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, 5));
		cursorIndicator.position=p;
	}
	
	
	private bool instruction=false;
	void OnGUI(){
		string title="swipe demo, use swipe to throw the ball object in the scene";
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
			
			GUI.Box(new Rect(10, 100, 250, 50), "");
			
			GUI.Label(new Rect(15, 105, 260, 40), "swipe on screen with 1 finger.\ncan use mouse to simulate that ");
		}
	}
}


