using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

public enum _ChargeMode{Once, Clamp, Loop, PingPong}

public class MultiTapTracker{
	public int index;
	public int count=0;
	public float lastTapTime=-1;
	public Vector2 lastPos;
	
	public int fingerCount=1;
	 
	
	public MultiTapTracker(int ind){
		index=ind;
	}
}


public class TapDetector : MonoBehaviour {
	
	public void SetChargeMode(_ChargeMode mode){
		chargeMode=mode;
	}
	
	private List<int> fingerIndex=new List<int>();
	private List<int> mouseIndex=new List<int>();
	
	private MultiTapTracker[] multiTapMFTouch=new MultiTapTracker[5];
	private MultiTapTracker[] multiTapTouch=new MultiTapTracker[5];
	private MultiTapTracker[] multiTapMouse=new MultiTapTracker[2];

	private float tapStartTime=0;
	//private bool touched=false;
	
	private enum _DTapState{Clear, Tap1, Complete}
	private _DTapState dTapState=_DTapState.Clear;
	private Vector2 lastPos;
	
	private enum _ChargeState{Clear, Charging, Charged}
	//private _ChargeState chargeState=_ChargeState.Clear;
	//private float chargedValue=0;
	
	//private bool longTap;
	private Vector2 startPos;
	
	private bool posShifted;
	
	private float lastShortTapTime;
	private Vector2 lastShortTapPos;
	
	//public bool enableShortTap=true;
	public float shortTapTime=0.2f;
	public float longTapTime=0.8f;
	//public float maxLTapSpacing=10;
	
	public float multiTapInterval=0.35f;
	public float multiTapPosSpacing=20;
	public int maxMultiTapCount=2;
	
	public _ChargeMode chargeMode;
	public float minChargeTime=0.15f;
	public float maxChargeTime=2.0f;
	
	private bool firstTouch=true;
	
	
	// Use this for initialization
	void Start () {
		
		for(int i=0; i<multiTapMouse.Length; i++){
			multiTapMouse[i]=new MultiTapTracker(i);
		}
		for(int i=0; i<multiTapTouch.Length; i++){
			multiTapTouch[i]=new MultiTapTracker(i);
		}
		for(int i=0; i<multiTapMFTouch.Length; i++){
			multiTapMFTouch[i]=new MultiTapTracker(i);
		}
		
		StartCoroutine(CheckMultiTapCount());
		StartCoroutine(MultiFingerRoutine());
		
		//Debug.Log(Gesture.GetTouch(0).position);
	}
	
	void CheckMultiTapMouse(int index, Vector2 pos){
		if(multiTapMouse[index].lastTapTime>Time.time-multiTapInterval){
			if(Vector2.Distance(pos, multiTapMouse[index].lastPos)<multiTapPosSpacing){
				multiTapMouse[index].count+=1;
				multiTapMouse[index].lastPos=pos;
				multiTapMouse[index].lastTapTime=Time.time;
				
				Gesture.MultiTap(new Tap(pos, multiTapMouse[index].count, index, true));
				
				if(multiTapMouse[index].count>=maxMultiTapCount) multiTapMouse[index].count=0;
			}
			else{
				multiTapMouse[index].count=1;
				multiTapMouse[index].lastPos=pos;
				multiTapMouse[index].lastTapTime=Time.time;
				
				Gesture.MultiTap(new Tap(pos, 1, index, true));
			}
		}
		else{
			multiTapMouse[index].count=1;
			multiTapMouse[index].lastPos=pos;
			multiTapMouse[index].lastTapTime=Time.time;
			
			Gesture.MultiTap(new Tap(pos, 1, index, true));
		}
	}
	
	void CheckMultiTapTouch(int index, Vector2 pos){
		if(multiTapTouch[index].lastTapTime>Time.time-multiTapInterval){
			if(Vector2.Distance(pos, multiTapTouch[index].lastPos)<multiTapPosSpacing){
				multiTapTouch[index].count+=1;
				multiTapTouch[index].lastPos=pos;
				multiTapTouch[index].lastTapTime=Time.time;
				
				Gesture.MultiTap(new Tap(pos, multiTapTouch[index].count, index, false));
				
				if(multiTapTouch[index].count>=maxMultiTapCount) multiTapTouch[index].count=0;
			}
			else{
				multiTapTouch[index].count=1;
				multiTapTouch[index].lastPos=pos;
				multiTapTouch[index].lastTapTime=Time.time;
				
				Gesture.MultiTap(new Tap(pos, 1, index, false));
			}
		}
		else{
			multiTapTouch[index].count=1;
			multiTapTouch[index].lastPos=pos;
			multiTapTouch[index].lastTapTime=Time.time;
			
			Gesture.MultiTap(new Tap(pos, 1, index, false));
		}
	}
	
	
	public void CheckMultiTapMFTouch(int fCount, Vector2[] posL, int[] indexes){
		Vector2 pos=Vector2.zero;
		foreach(Vector2 p in posL){
			pos+=p;
		}
		pos/=posL.Length;
		
		int index=0;
		bool match=false;
		foreach(MultiTapTracker multiTap in multiTapMFTouch){
			if(multiTap.fingerCount==fCount){
				if(Vector2.Distance(pos, multiTap.lastPos)<multiTapPosSpacing){
					match=true;
					break;
				}
			}
			index+=1;
		}
		
		if(!match){
			index=0;
			foreach(MultiTapTracker multiTap in multiTapMFTouch){
				if(multiTap.lastPos==Vector2.zero && multiTap.count==0){
					break;
				}
				index+=1;
			}
		}
		
		if(multiTapMFTouch[index].lastTapTime>Time.time-multiTapInterval){
			multiTapMFTouch[index].count+=1;
			multiTapMFTouch[index].lastPos=pos;
			multiTapMFTouch[index].fingerCount=fCount;
			multiTapMFTouch[index].lastTapTime=Time.time;
			
			Gesture.MultiTap(new Tap(multiTapMFTouch[index].count, fCount, posL, indexes));
			
			if(multiTapMFTouch[index].count>=maxMultiTapCount) multiTapMFTouch[index].count=0;
		}
		else{
			multiTapMFTouch[index].count=1;
			multiTapMFTouch[index].lastPos=pos;
			multiTapMFTouch[index].fingerCount=fCount;
			multiTapMFTouch[index].lastTapTime=Time.time;
			
			Gesture.MultiTap(new Tap(multiTapMFTouch[index].count, fCount, posL, indexes));
		}
	}
	
	
	IEnumerator CheckMultiTapCount(){
		while(true){
			foreach(MultiTapTracker multiTap in multiTapMouse){
				if(multiTap.count>0){
					if(multiTap.lastTapTime+multiTapInterval<Time.time){
						multiTap.count=0;
						multiTap.lastPos=Vector2.zero;
					}
				}
			}
			foreach(MultiTapTracker multiTap in multiTapTouch){
				if(multiTap.count>0){
					if(multiTap.lastTapTime+multiTapInterval<Time.time){
						multiTap.count=0;
						multiTap.lastPos=Vector2.zero;
					}
				}
			}
			foreach(MultiTapTracker multiTap in multiTapMFTouch){
				if(multiTap.count>0){
					if(multiTap.lastTapTime+multiTapInterval<Time.time){
						multiTap.count=0;
						multiTap.lastPos=Vector2.zero;
						multiTap.fingerCount=1;
					}
				}
			}
			
			yield return null;
		}
	}
	
	IEnumerator FingerRoutine(int index){
		fingerIndex.Add(index);
		
		//init tap variables
		Touch touch=Gesture.GetTouch(index);
		float startTime=Time.time;
		Vector2 startPos=touch.position;
		Vector2 lastPos=startPos;
		bool longTap=false;
		
		//init charge variables
		_ChargeState chargeState=_ChargeState.Clear;
		int chargeDir=1;
		int chargeConst=0;
		float startTimeCharge=Time.time;
		Vector2 startPosCharge=touch.position;
		
		//yield return null;

		while(true){
			touch=Gesture.GetTouch(index);
			if(touch.position==Vector2.zero) break;
			
			Vector2 curPos=touch.position;
			
			if(Time.time-startTimeCharge>minChargeTime && chargeState==_ChargeState.Clear){
				chargeState=_ChargeState.Charging;
				float chargedValue=Mathf.Clamp(chargeConst+chargeDir*((Time.time-startTimeCharge)/maxChargeTime), 0, 1);
				ChargedInfo cInfo=new ChargedInfo(curPos, chargedValue, index, false);
				Gesture.ChargeStart(cInfo);
				
				startPosCharge=curPos;
			}
			else if(chargeState==_ChargeState.Charging){
				if(Vector3.Distance(curPos, startPosCharge)>15){
					chargeState=_ChargeState.Clear;
					float chargedValue=Mathf.Clamp(chargeConst+chargeDir*((Time.time-startTimeCharge)/maxChargeTime), 0, 1);
					ChargedInfo cInfo=new ChargedInfo(lastPos, chargedValue, index, false);
					Gesture.ChargeEnd(cInfo);
				}
				else{
					float chargedValue=Mathf.Clamp(chargeConst+chargeDir*((Time.time-startTimeCharge)/maxChargeTime), 0, 1);
					ChargedInfo cInfo=new ChargedInfo(curPos, chargedValue, index, false);
					
					if(chargeMode==_ChargeMode.PingPong){
						if(chargedValue==1 || chargedValue==0){
							chargeDir*=-1;
							if(chargeDir==1) chargeConst=0;
							else if(chargeDir==-1) chargeConst=1;
							startTimeCharge=Time.time;
						}
						
						Gesture.Charging(cInfo);
					}
					else{
						if(chargedValue<1.0f){
							Gesture.Charging(cInfo);
						}
						else{
							cInfo.percent=1.0f;
							
							if(chargeMode==_ChargeMode.Once){
								chargeState=_ChargeState.Charged;
								Gesture.ChargeEnd(cInfo);
								startTimeCharge=Mathf.Infinity;
								chargedValue=0;
							}
							else if(chargeMode==_ChargeMode.Clamp){
								chargeState=_ChargeState.Charged;
								Gesture.Charging(cInfo);
							}
							else if(chargeMode==_ChargeMode.Loop){
								chargeState=_ChargeState.Clear;
								Gesture.ChargeEnd(cInfo);
								startTimeCharge=Time.time;
							}
						}
						
					}
				}
			}
			
			if(!longTap && Time.time-startTime>longTapTime && Vector2.Distance(lastPos, startPos)<5){
				//new Tap(multiTapMFTouch[index].count, fCount, posL)
				//Gesture.LongTap(new Tap(multiTapMFTouch[index].count, fCount, posL));
				Gesture.LongTap(new Tap(curPos, 1, index, false));
				//Gesture.LongTap(startPos);
				longTap=true;
			}
			
			lastPos=curPos;
			
			yield return null;
		}
		
		//check for shortTap
		//if(Time.time-startTime<=shortTapTime && Vector2.Distance(lastPos, startPos)<5){
		if(Time.time-startTime<=shortTapTime){
			CheckMultiTapTouch(index, startPos);
		}
		
		//check for charge
		if(chargeState==_ChargeState.Charging || (chargeState==_ChargeState.Charged && chargeMode!=_ChargeMode.Once)){
			float chargedValue=Mathf.Clamp(chargeConst+chargeDir*((Time.time-startTimeCharge)/maxChargeTime), 0, 1);
			ChargedInfo cInfo=new ChargedInfo(lastPos, chargedValue, index, false);
			Gesture.ChargeEnd(cInfo);
		}
		
		fingerIndex.Remove(index);
	}
	
	
	IEnumerator MouseRoutine(int index){
		mouseIndex.Add(index);
		
		//init tap variables
		float startTime=Time.time;
		Vector2 startPos=Input.mousePosition;
		Vector2 lastPos=startPos;
		bool longTap=false;
		
		//init charge variables
		_ChargeState chargeState=_ChargeState.Clear;
		int chargeDir=1;
		float chargeConst=0;
		float startTimeCharge=Time.time;
		Vector2 startPosCharge=Input.mousePosition;
		
		yield return null;
		
		while(mouseIndex.Contains(index)){
			
			Vector2 curPos=Input.mousePosition;
			
			if(Time.time-startTimeCharge>minChargeTime && chargeState==_ChargeState.Clear){
				chargeState=_ChargeState.Charging;
				float chargedValue=Mathf.Clamp(chargeConst+chargeDir*((Time.time-startTimeCharge)/maxChargeTime), 0, 1);
				ChargedInfo cInfo=new ChargedInfo(curPos, chargedValue, index, true);
				Gesture.ChargeStart(cInfo);
				
				startPosCharge=curPos;
			}
			else if(chargeState==_ChargeState.Charging){
				if(Vector3.Distance(curPos, startPosCharge)>5){
					chargeState=_ChargeState.Clear;
					float chargedValue=Mathf.Clamp(chargeConst+chargeDir*((Time.time-startTimeCharge)/maxChargeTime), 0, 1);
					ChargedInfo cInfo=new ChargedInfo(lastPos, chargedValue, index, true);
					Gesture.ChargeEnd(cInfo);
				}
				else{
					float chargedValue=Mathf.Clamp(chargeConst+chargeDir*((Time.time-startTimeCharge)/maxChargeTime), 0, 1);
					ChargedInfo cInfo=new ChargedInfo(curPos, chargedValue, index, true);
					
					if(chargeMode==_ChargeMode.PingPong){
						if(chargedValue==1 || chargedValue==0){
							chargeDir*=-1;
							if(chargeDir==1) chargeConst=0;
							else if(chargeDir==-1) chargeConst=1;
							startTimeCharge=Time.time;
						}
						Gesture.Charging(cInfo);
					}
					else{
						if(chargedValue<1.0f){
							Gesture.Charging(cInfo);
						}
						else{
							cInfo.percent=1.0f;
							
							if(chargeMode==_ChargeMode.Once){
								chargeState=_ChargeState.Charged;
								Gesture.ChargeEnd(cInfo);
								startTimeCharge=Mathf.Infinity;
								chargedValue=0;
							}
							else if(chargeMode==_ChargeMode.Clamp){
								chargeState=_ChargeState.Charged;
								Gesture.Charging(cInfo);
							}
							else if(chargeMode==_ChargeMode.Loop){
								chargeState=_ChargeState.Clear;
								Gesture.ChargeEnd(cInfo);
								startTimeCharge=Time.time;
							}
						}
						
					}
				}
			}
			
			if(!longTap && Time.time-startTime>longTapTime && Vector2.Distance(lastPos, startPos)<5){
				Gesture.LongTap(new Tap(curPos, 1, index, true));
				longTap=true;
			}
			
			lastPos=curPos;
			
			yield return null;
		}
		
		//check for shortTap
		if(Time.time-startTime<=shortTapTime && Vector2.Distance(lastPos, startPos)<5){
			//Gesture.ShortTap(startPos);
			CheckMultiTapMouse(index, startPos);
		}
		
		//check for charge
		if(chargeState==_ChargeState.Charging || (chargeState==_ChargeState.Charged && chargeMode!=_ChargeMode.Once)){
			float chargedValue=Mathf.Clamp(chargeConst+chargeDir*((Time.time-startTimeCharge)/maxChargeTime), 0, 1);
			ChargedInfo cInfo=new ChargedInfo(lastPos, chargedValue, index, true);
			Gesture.ChargeEnd(cInfo);
		}
		
	}
	
	private List<int> indexes=new List<int>();
	
	// Update is called once per frame
	void Update () {
		
		//InputEvent inputEvent=new InputEvent();
		
		if(Input.touchCount>0){
			
			if(indexes.Count<Input.touchCount){
				foreach(Touch touch in Input.touches){
					if(!fingerIndex.Contains(touch.fingerId)){
						CheckFingerGroup(touch);
					}
				}
			}

			foreach(Touch touch in Input.touches){
				if(fingerIndex.Count==0 || !fingerIndex.Contains(touch.fingerId)){
					//StartCoroutine(swipeRoutine(new InputEvent(touch)));
					StartCoroutine(FingerRoutine(touch.fingerId));
				}
				
				
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
	
	private List<FingerGroup> fingerGroup=new List<FingerGroup>();
	IEnumerator MultiFingerRoutine(){
		while(true){
			if(fingerGroup.Count>0){
				for(int i=0; i<fingerGroup.Count; i++){
					if(fingerGroup[i].routineEnded){
						fingerGroup.RemoveAt(i);
						i--;
					}
				}
			}
			yield return null;
		}
	}
	
	public float maxFingerGroupDist=200;
	
	void CheckFingerGroup(Touch touch){
		//Debug.Log("Checking "+Time.time);
		bool match=false;
		foreach(FingerGroup group in fingerGroup){
			if(Time.time-group.triggerTime<shortTapTime/2){
				bool inRange=true;
				foreach(int index in group.indexes){
					if(Vector2.Distance(Gesture.GetTouch(index).position, touch.position)>maxFingerGroupDist)
						inRange=false;
				}
				if(inRange){
					group.indexes.Add(touch.fingerId);
					group.positions.Add(touch.position);
					match=true;
					break;
				}
			}
		}
		
		if(!match){
			fingerGroup.Add(new FingerGroup(Time.time, touch.fingerId, touch.position));
			StartCoroutine(fingerGroup[fingerGroup.Count-1].Routine(this));
		}
	}
	
	
}

public class FingerGroup{
	public List<int> indexes=new List<int>();
	public List<Vector2> positions=new List<Vector2>();
	public Vector2 posAvg;
	public float triggerTime;
	public bool routineEnded=false;
	public int count=0;
	public bool longTap=false;
	
	//charge related variable
	private enum _ChargeState{Clear, Charging, Charged}
	_ChargeState chargeState=_ChargeState.Clear;
	int chargeDir=1;
	int chargeConst=0;
	float startTimeCharge=Time.time;
	
	private int[] indexList;
	private Vector2[] posList;
	
	private TapDetector tapDetector;

	public FingerGroup(float time, int index, Vector2 pos){
		indexes.Add(index);
		positions.Add(pos);
	}
	
	public IEnumerator Routine(TapDetector tapD){
		tapDetector=tapD;
		
		triggerTime=Time.time;
		startTimeCharge=Time.time;
		
		yield return new WaitForSeconds(0.075f);
		if(indexes.Count<2){
			routineEnded=true;
			yield break;
		}
		else{
			count=indexes.Count;

			posAvg=Vector2.zero;
			foreach(Vector2 p in positions) posAvg+=p;
			posAvg/=positions.Count;
			
			posList = new Vector2[positions.Count];
  			positions.CopyTo( posList );
  			indexList = new int[indexes.Count];
  			indexes.CopyTo( indexList );		
		}
		
		bool isOn=true;
		
		float liftTime=-1;
		
		while(isOn){
			for(int i=0; i<indexes.Count; i++){
				Touch touch=Gesture.GetTouch(indexes[i]);
				if(touch.phase==TouchPhase.Moved) isOn=false;
				
				if(touch.position==Vector2.zero){
					if(indexes.Count==count){
						liftTime=Time.time;
					}
					indexes.RemoveAt(i);
					i--;
				}
			}
			
			if(Time.time-startTimeCharge>tapDetector.minChargeTime && chargeState==_ChargeState.Clear){
				chargeState=_ChargeState.Charging;
				float chargedValue=Mathf.Clamp(chargeConst+chargeDir*((Time.time-startTimeCharge)/tapDetector.maxChargeTime), 0, 1);
				ChargedInfo cInfo=new ChargedInfo(posAvg, posList, chargedValue, indexList);
				Gesture.ChargeStart(cInfo);
			}
			else if(chargeState==_ChargeState.Charging){

				float chargedValue=Mathf.Clamp(chargeConst+chargeDir*((Time.time-startTimeCharge)/tapDetector.maxChargeTime), 0, 1);
				ChargedInfo cInfo=new ChargedInfo(posAvg, posList, chargedValue, indexList);
				
				if(tapDetector.chargeMode==_ChargeMode.PingPong){
					if(chargedValue==1 || chargedValue==0){
						chargeDir*=-1;
						if(chargeDir==1) chargeConst=0;
						else if(chargeDir==-1) chargeConst=1;
						startTimeCharge=Time.time;
					}
					
					Gesture.Charging(cInfo);
				}
				else{
					if(chargedValue<1.0f){
						Gesture.Charging(cInfo);
					}
					else{
						cInfo.percent=1.0f;
						
						if(tapDetector.chargeMode==_ChargeMode.Once){
							chargeState=_ChargeState.Charged;
							Gesture.ChargeEnd(cInfo);
							startTimeCharge=Mathf.Infinity;
							chargedValue=0;
						}
						else if(tapDetector.chargeMode==_ChargeMode.Clamp){
							chargeState=_ChargeState.Charged;
							Gesture.Charging(cInfo);
						}
						else if(tapDetector.chargeMode==_ChargeMode.Loop){
							chargeState=_ChargeState.Clear;
							Gesture.ChargeEnd(cInfo);
							startTimeCharge=Time.time;
						}
					}
				}

			}
			
			if(!longTap && Time.time-triggerTime>tapDetector.longTapTime){
				if(indexes.Count==count){
					    
					Vector2[] posList = new Vector2[positions.Count];
  					positions.CopyTo( posList );
					
					Tap tap=new Tap(1, count, posList , indexList);
					Gesture.LongTap(tap);
					longTap=true;
				}
			}
			
			if(indexes.Count<count){
				if(Time.time-liftTime>0.075f || indexes.Count==0){
					if(indexes.Count==0){
						if(liftTime-triggerTime<tapDetector.shortTapTime+0.1f){
							Vector2[] posList = new Vector2[positions.Count];
		  					positions.CopyTo( posList );
							//Tap tap=new Tap(1, count, posList);
							//Gesture.MFShortTap(tap);
							tapDetector.CheckMultiTapMFTouch(count, posList, indexList);
						}
					}
					isOn=false;
					break;
				}
			}
			
			yield return null;
		}
		
		if(chargeState==_ChargeState.Charging || (chargeState==_ChargeState.Charged && tapDetector.chargeMode!=_ChargeMode.Once)){
			float chargedValue=Mathf.Clamp(chargeConst+chargeDir*((Time.time-startTimeCharge)/tapDetector.maxChargeTime), 0, 1);
			ChargedInfo cInfo=new ChargedInfo(posAvg, posList, chargedValue, indexList);
			Gesture.ChargeEnd(cInfo);
		}
		
		routineEnded=true;
	}
}