using UnityEngine;
using System.Collections;

public class TapDemo : MonoBehaviour {

	public ParticleSystem Indicator;
	
	public Transform shortTapObj;
	public Transform longTapObj;
	public Transform doubleTapObj;
	public Transform chargeObj;
	
	public TextMesh chargeTextMesh;
	
	public Transform dragObj1;
	public TextMesh dragTextMesh1;
	
	public Transform dragObj2;
	public TextMesh dragTextMesh2;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	void OnEnable(){
		//these events are obsolete, replaced by onMultiTapE, but it's still usable
		//Gesture.onShortTapE += OnShortTap;
		//Gesture.onDoubleTapE += OnDoubleTap;
		
		Gesture.onMultiTapE += OnMultiTap;
		Gesture.onLongTapE += OnLongTap;
		
		Gesture.onChargingE += OnCharging;
		Gesture.onChargeEndE += OnChargeEnd;
		
		Gesture.onDraggingStartE += OnDraggingStart;
		Gesture.onDraggingE += OnDragging;
		Gesture.onDraggingEndE += OnDraggingEnd;
	}
	
	void OnDisable(){
		//these events are obsolete, replaced by onMultiTapE, but it's still usable
		//Gesture.onShortTapE -= OnShortTap;
		//Gesture.onDoubleTapE -= OnDoubleTap;
		
		Gesture.onMultiTapE -= OnMultiTap;
		Gesture.onLongTapE -= OnLongTap;
		
		Gesture.onChargingE -= OnCharging;
		Gesture.onChargeEndE -= OnChargeEnd;
		
		Gesture.onDraggingStartE -= OnDraggingStart;
		Gesture.onDraggingE -= OnDragging;
		Gesture.onDraggingEndE -= OnDraggingEnd;
	}
	
	
	//called when a multi-Tap event is detected
	void OnMultiTap(Tap tap){
		//do a raycast base on the position of the tap
		Ray ray = Camera.main.ScreenPointToRay(tap.pos);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, Mathf.Infinity)){
			//if the tap lands on the shortTapObj, then shows the effect.
			if(hit.collider.transform==shortTapObj){
				//place the indicator at the object position and assign a random color to it
				Indicator.transform.position=shortTapObj.position;
				Indicator.startColor=GetRandomColor();
				//emit a set number of particle
				Indicator.Emit(30);
			}
			//if the tap lands on the doubleTapObj
			else if(hit.collider.transform==doubleTapObj){
				//check to make sure if the tap count matches
				if(tap.count==2){
					//place the indicator at the object position and assign a random color to it
					Indicator.transform.position=doubleTapObj.position;
					Indicator.startColor=GetRandomColor();
					//emit a set number of particle
					Indicator.Emit(30);
				}
			}
		}
	}
	
	
	
	//called when a long tap event is ended
	void OnLongTap(Tap tap){
		//do a raycast base on the position of the tap
		Ray ray = Camera.main.ScreenPointToRay(tap.pos);
		RaycastHit hit;
		//if the tap lands on the longTapObj
		if(Physics.Raycast(ray, out hit, Mathf.Infinity)){
			if(hit.collider.transform==longTapObj){
				//place the indicator at the object position and assign a random color to it
				Indicator.transform.position=longTapObj.position;
				Indicator.startColor=GetRandomColor();
				//emit a set number of particle
				Indicator.Emit(30);
			}
		}
	}
	
	//called when a double tap event is ended
	//this event is used for onDoubleTapE in v1.0, it's still now applicabl
	void OnDoubleTap(Vector2 pos){
		Ray ray = Camera.main.ScreenPointToRay(pos);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, Mathf.Infinity)){
			if(hit.collider.transform==doubleTapObj){
				//place the indicator at the object position and assign a random color to it
				Indicator.transform.position=doubleTapObj.position;
				Indicator.startColor=GetRandomColor();
				//emit a set number of particle
				Indicator.Emit(30);
			}
		}
	}
	
	
	
	//called when a charge event is ended
	void OnChargeEnd(ChargedInfo cInfo){
		Ray ray = Camera.main.ScreenPointToRay(cInfo.pos);
		RaycastHit hit;
		//use raycast at the cursor position to detect the object
		if(Physics.Raycast(ray, out hit, Mathf.Infinity)){
			if(hit.collider.transform==chargeObj){
				//place the indicator at the object position and assign a random color to it
				Indicator.transform.position=chargeObj.position;
				Indicator.startColor=GetRandomColor();
				
				//adjust the indicator speed with respect to the charged percent
				Indicator.startSpeed=1+3*cInfo.percent;
				//emit a set number of particles with respect to the charged percent
				Indicator.Emit((int)(10+cInfo.percent*75f));
				
				//reset the particle speed, since it's shared by other event
				StartCoroutine(ResumeSpeed());
			}
		}
		chargeTextMesh.text="HoldToCharge";
	}
	
	//reset the particle emission speed of the indicator
	IEnumerator ResumeSpeed(){
		yield return new WaitForSeconds(Indicator.startLifetime);
		Indicator.startSpeed=2;
	}
	
	void Update(){
		//Debug.Log(currentDragIndex+"   "+dragByMouse);
	}
	
	private int currentDragIndex1=-1;
	private int currentDragIndex2=-1;
	void OnDraggingStart(DragInfo dragInfo){
		//currentDragIndex=dragInfo.index;
		Debug.Log("ondragging start");
		//if(currentDragIndex==-1){
			Ray ray = Camera.main.ScreenPointToRay(dragInfo.pos);
			RaycastHit hit;
			//use raycast at the cursor position to detect the object
			if(Physics.Raycast(ray, out hit, Mathf.Infinity)){
				if(hit.collider.transform==dragObj1){
					dragObj1.localScale*=1.1f;
					Obj1ToCursor(dragInfo);
					currentDragIndex1=dragInfo.index;
				}
				else if(hit.collider.transform==dragObj2){
					dragObj2.localScale*=1.1f;
					Obj2ToCursor(dragInfo);
					currentDragIndex2=dragInfo.index;
				}
			}
		//}
	}
	
	void OnDragging(DragInfo dragInfo){
		if(dragInfo.index==currentDragIndex1){
			Obj1ToCursor(dragInfo);
		}
		else if(dragInfo.index==currentDragIndex2){
			Obj2ToCursor(dragInfo);
		}
	}
	
	void Obj1ToCursor(DragInfo dragInfo){
		Vector3 p=Camera.main.ScreenToWorldPoint(new Vector3(dragInfo.pos.x, dragInfo.pos.y, 30));
		dragObj1.position=p;
		
		if(dragInfo.isMouse){
			dragTextMesh1.text="Dragging with mouse"+(dragInfo.index+1);
		}
		else{
			dragTextMesh1.text="Dragging with finger"+(dragInfo.index+1);
		}
	}
	
	void Obj2ToCursor(DragInfo dragInfo){
		Vector3 p=Camera.main.ScreenToWorldPoint(new Vector3(dragInfo.pos.x, dragInfo.pos.y, 30));
		dragObj2.position=p;
		
		if(dragInfo.isMouse){
			dragTextMesh2.text="Dragging with mouse"+(dragInfo.index+1);
		}
		else{
			dragTextMesh2.text="Dragging with finger"+(dragInfo.index+1);
		}
	}
	
	void OnDraggingEnd(DragInfo dragInfo){
		
		//drop the dragObj being drag by this particular cursor
		if(dragInfo.index==currentDragIndex1){
			currentDragIndex1=-1;
			dragObj1.localScale*=10f/11f;
			
			Vector3 p=Camera.main.ScreenToWorldPoint(new Vector3(dragInfo.pos.x, dragInfo.pos.y, 30));
			dragObj1.position=p;
			dragTextMesh1.text="DragMe";
		}
		else if(dragInfo.index==currentDragIndex2){
			currentDragIndex2=-1;
			dragObj2.localScale*=10f/11f;
			
			Vector3 p=Camera.main.ScreenToWorldPoint(new Vector3(dragInfo.pos.x, dragInfo.pos.y, 30));
			dragObj2.position=p;
			dragTextMesh2.text="DragMe";
		}
		
	}
	
	
	//return a random color when called
	private Color GetRandomColor(){
		return new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
	}
	

	private bool instruction=false;
	void OnGUI(){
		if(!instruction){
			if(GUI.Button(new Rect(10, 55, 130, 35), "Instruction On")){
				instruction=true;
			}
		}
		else{
			if(GUI.Button(new Rect(10, 55, 130, 35), "Instruction Off")){
				instruction=false;
			}
			
			GUI.Box(new Rect(10, 100, 200, 65), "");
			
			GUI.Label(new Rect(15, 105, 190, 65), "interact with each object using the interaction stated on top of each of them");
		}
	}
	
	
	//************************************************************************************************//
	//following function is used in v1.0 and is now obsolete
	
	//called when a short tap event is ended
	//this event is used for onShortTapE in v1.0, it's still now applicable
	void OnShortTap(Vector2 pos){
		Ray ray = Camera.main.ScreenPointToRay(pos);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, Mathf.Infinity)){
			if(hit.collider.transform==shortTapObj){
				//place the indicator at the object position and assign a random color to it
				Indicator.transform.position=shortTapObj.position;
				Indicator.startColor=GetRandomColor();
				//emit a set number of particle
				Indicator.Emit(30);
			}
		}
	}
	
	//called when a charging event is detected
	void OnCharging(ChargedInfo cInfo){
		Ray ray = Camera.main.ScreenPointToRay(cInfo.pos);
		RaycastHit hit;
		//use raycast at the cursor position to detect the object
		if(Physics.Raycast(ray, out hit, Mathf.Infinity)){
			if(hit.collider.transform==chargeObj){
				//display the charged percentage on screen
				chargeTextMesh.text="Charging "+(cInfo.percent*100).ToString("f1")+"%";
			}
		}
	}

}
