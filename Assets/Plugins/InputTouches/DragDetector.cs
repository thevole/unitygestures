using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DragDetector : MonoBehaviour {

	private List<int> fingerIndex=new List<int>();
	private List<int> mouseIndex=new List<int>();
	
	public int minDragDistance=15;
	public bool enableMultiDrag=false;

	//private Vector2 lastPos;
	//private bool dragging=false;
	//private bool draggingInitiated=false;

	// Use this for initialization
	void Start () {
	
	}
	
	private int multiDragCount=0;
	IEnumerator MultiDragRoutine(int count){
		
		bool dragStarted=false;
		
		Vector2 startPos=Vector2.zero;
		for(int i=0; i<Input.touchCount; i++){
			startPos+=Input.touches[i].position;
		}
		startPos/=Input.touchCount;
		Vector2 lastPos=startPos;
		
		while(Input.touchCount==count){
			Vector2 curPos=Vector2.zero;
			Vector2[] allPos=new Vector2[count];
			bool moving=true;
			for(int i=0; i<count; i++){
				Touch touch=Input.touches[i];
				curPos+=touch.position;
				allPos[i]=touch.position;
				if(touch.phase!=TouchPhase.Moved) moving=false;
			}
			curPos/=count;
			
			bool sync=true;
			if(moving){
				for(int i=0; i<count-1; i++){
					Vector2 v1=Input.touches[i].deltaPosition.normalized;
					Vector2 v2=Input.touches[i+1].deltaPosition.normalized;
					if(Vector2.Dot(v1, v2)<0.85f) sync=false;
				}
			}
			
			if(moving && sync){
				if(!dragStarted){
					if(Vector2.Distance(curPos, startPos)>minDragDistance){
						dragStarted=true;
						Vector2 delta=curPos-startPos;
						DragInfo dragInfo=new DragInfo(curPos, delta, count);
						Gesture.DraggingStart(dragInfo);
					}
				}
				else{
					if(curPos!=lastPos){
						Vector2 delta=curPos-lastPos;
						DragInfo dragInfo=new DragInfo(curPos, delta, count);
						Gesture.Dragging(dragInfo);
					}
				}
			}
			
			lastPos=curPos;
			
			yield return null;
		}
		
		if(dragStarted){
			Vector2 delta=lastPos-startPos;
			DragInfo dragInfo=new DragInfo(lastPos, delta, count);
			Gesture.DraggingEnd(dragInfo);
		}
		
	}
	
	IEnumerator MouseRoutine(int index){
		mouseIndex.Add(index);
		
		bool dragStarted=false;
		
		Vector2 startPos=Input.mousePosition;
		Vector2 lastPos=startPos;
		
		while(mouseIndex.Contains(index)){
			
			Vector2 curPos=Input.mousePosition;
			
			if(!dragStarted){
				if(Vector3.Distance(curPos, startPos)>minDragDistance){
					dragStarted=true;
					Vector2 delta=curPos-startPos;
					DragInfo dragInfo=new DragInfo(curPos, delta, 1, index, true);
					Gesture.DraggingStart(dragInfo);
				}
			}
			else{
				if(curPos!=lastPos){
					Vector2 delta=curPos-lastPos;
					DragInfo dragInfo=new DragInfo(curPos, delta, 1, index, true);
					Gesture.Dragging(dragInfo);
				}
			}
			
			lastPos=curPos;
			
			yield return null;
		}
		
		if(dragStarted){
			Vector2 delta=lastPos-startPos;
			DragInfo dragInfo=new DragInfo(lastPos, delta, 1, index, true);
			Gesture.DraggingEnd(dragInfo);
		}
	}
	
	IEnumerator TouchRoutine(int index){
		fingerIndex.Add(index);
		
		bool dragStarted=false;
		
		Vector2 startPos=Gesture.GetTouch(index).position;
		Vector2 lastPos=startPos;
		
		while((enableMultiDrag && Input.touchCount>0) || (!enableMultiDrag && Input.touchCount==1)){
			
			Touch touch=Gesture.GetTouch(index);
			if(touch.position==Vector2.zero) break;
			
			Vector2 curPos=touch.position;
			
			if(!dragStarted){
				if(Vector3.Distance(curPos, startPos)>minDragDistance){
					dragStarted=true;
					Vector2 delta=curPos-startPos;
					DragInfo dragInfo=new DragInfo(curPos, delta, 1, index, false);
					Gesture.DraggingStart(dragInfo);
				}
			}
			else{
				if(curPos!=lastPos){
					Vector2 delta=curPos-lastPos;
					DragInfo dragInfo=new DragInfo(curPos, delta, 1, index, false);
					Gesture.Dragging(dragInfo);
				}
			}
			
			lastPos=curPos;
			
			yield return null;
		}
		
		if(dragStarted){
			Vector2 delta=lastPos-startPos;
			DragInfo dragInfo=new DragInfo(lastPos, delta, 1, index, false);
			Gesture.DraggingEnd(dragInfo);
		}
		
		fingerIndex.Remove(index);
	}
		
	
	// Update is called once per frame
	void Update () {
		
		//drag event detection goes here
		if(Input.touchCount>0){
			if(enableMultiDrag){
				foreach(Touch touch in Input.touches){
					if(fingerIndex.Count==0 || !fingerIndex.Contains(touch.fingerId)){
						StartCoroutine(TouchRoutine(touch.fingerId));
					}
				}
			}
			else{
				if(Input.touchCount==1){
					if(fingerIndex.Count==0){
						StartCoroutine(TouchRoutine(Input.touches[0].fingerId));
					}
				}
			}
			
			if(Input.touchCount!=multiDragCount){
				multiDragCount=Input.touchCount;
				StartCoroutine(MultiDragRoutine(Input.touchCount));
			}
		}
		else if(Input.touchCount==0){
			if(Input.GetMouseButtonDown(0)){
				if(!mouseIndex.Contains(0)) StartCoroutine(MouseRoutine(0)); 
			}
			else if(Input.GetMouseButtonUp(0)){
				if(mouseIndex.Contains(0)) mouseIndex.Remove(0); 
			}
			
			if(Input.GetMouseButtonDown(1)){
				if(!mouseIndex.Contains(1)) StartCoroutine(MouseRoutine(1)); 
			}
			else if(Input.GetMouseButtonUp(1)){
				if(mouseIndex.Contains(1)) mouseIndex.Remove(1); 
			}
			
		}
		
	}
	
	
}


