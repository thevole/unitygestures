using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BasicDetector))]
[RequireComponent (typeof (DragDetector))]
[RequireComponent (typeof (TapDetector))]
[RequireComponent (typeof (SwipeDetector))]
[RequireComponent (typeof (DualFingerDetector))]


public class Gesture : MonoBehaviour {

	public static Gesture gesture;
	
	//*****************************************************************************//
	//standard tap event
	public delegate void MultiTapHandler(Tap tap); 
	public static event MultiTapHandler onMultiTapE;
	
	
	public delegate void LongTapHandler(Tap tap); 
	public static event LongTapHandler onLongTapE;
	
	public delegate void ChargeStartHandler(ChargedInfo cInfo); 
	public static event ChargeStartHandler onChargeStartE;
	
	public delegate void ChargingHandler(ChargedInfo cInfo); 
	public static event ChargingHandler onChargingE;
	
	public delegate void ChargeEndHandler(ChargedInfo cInfo); 
	public static event ChargeEndHandler onChargeEndE;
	
	
	//Multi-Finger Standard tap event
	public delegate void MFMultiTapHandler(Tap tap); 
	public static event MFMultiTapHandler onMFMultiTapE;
	
	public delegate void MFLongTapHandler(Tap tap); 
	public static event MFLongTapHandler onMFLongTapE;
	
	public delegate void MFChargeStartHandler(ChargedInfo cInfo); 
	public static event MFChargeStartHandler onMFChargeStartE;
	
	public delegate void MFChargingHandler(ChargedInfo cInfo); 
	public static event MFChargingHandler onMFChargingE;
	
	public delegate void MFChargeEndHandler(ChargedInfo cInfo); 
	public static event MFChargeEndHandler onMFChargeEndE;
	
	
	//*****************************************************************************//
	//dragging
	public delegate void DraggingStartHandler(DragInfo dragInfo);
	public static event DraggingStartHandler onDraggingStartE;
	
	public delegate void DraggingHandler(DragInfo dragInfo);
	public static event DraggingHandler onDraggingE;
	
	public delegate void DraggingEndHandler(DragInfo dragInfo); 
	public static event DraggingEndHandler onDraggingEndE;
	
	public delegate void MFDraggingStartHandler(DragInfo dragInfo); 
	public static event MFDraggingStartHandler onMFDraggingStartE;
	
	public delegate void MFDraggingHandler(DragInfo dragInfo); 
	public static event MFDraggingHandler onMFDraggingE;
	
	public delegate void MFDraggingEndHandler(DragInfo dragInf); 
	public static event MFDraggingEndHandler onMFDraggingEndE;
	
	
	//*****************************************************************************//
	//special event swipe/pinch/rotate
	public delegate void SwipeStartHandler(SwipeInfo sw); 
	public static event SwipeStartHandler onSwipeStartE;
	
	public delegate void SwipingHandler(SwipeInfo sw); 
	public static event SwipingHandler onSwipingE;
	
	public delegate void SwipeEndHandler(SwipeInfo sw); 
	public static event SwipeEndHandler onSwipeEndE;
	
	public delegate void SwipeHandler(SwipeInfo sw); 
	public static event SwipeHandler onSwipeE;
	
	public delegate void PinchHandler(float val); 
	public static event PinchHandler onPinchE;
	
	public delegate void RotateHandler(float val); 
	public static event RotateHandler onRotateE;
	
	
	
	//*****************************************************************************//
	//native input down/on/up event
	public delegate void TouchDownHandler(Vector2 pos); 
	public static event TouchDownHandler onTouchDownE;
	
	public delegate void TouchUpHandler(Vector2 pos); 
	public static event TouchUpHandler onTouchUpE;
	
	public delegate void TouchHandler(Vector2 pos); 
	public static event TouchHandler onTouchE;
	
	public delegate void Mouse1DownHandler(Vector2 pos); 
	public static event Mouse1DownHandler onMouse1DownE;
	
	public delegate void Mouse1UpHandler(Vector2 pos); 
	public static event Mouse1UpHandler onMouse1UpE;
	
	public delegate void Mouse1Handler(Vector2 pos); 
	public static event Mouse1Handler onMouse1E;
	
	public delegate void Mouse2DownHandler(Vector2 pos); 
	public static event Mouse2DownHandler onMouse2DownE;
	
	public delegate void Mouse2UpHandler(Vector2 pos); 
	public static event Mouse2UpHandler onMouse2UpE;
	
	public delegate void Mouse2Handler(Vector2 pos); 
	public static event Mouse2Handler onMouse2E;
	
	
	
	
	
	
	void Awake(){
		gesture=this;
	}
	
	
	//*****************************************************************************//
	//standard tap event
	public static void MultiTap(Tap tap){
		
		//Debug.Log("multitap "+tap.count);
		
		if(tap.fingerCount==1){
			if(tap.count==1){
				if(onShortTapE!=null) onShortTapE(tap.pos);
			}
			else if(tap.count==2){
				if(onDoubleTapE!=null) onDoubleTapE(tap.pos);
			}
			
			if(onMultiTapE!=null) onMultiTapE(tap);
		}
		else{
			if(tap.fingerCount==2){
				if(tap.count==1){
					DFShortTap(tap.pos);
				}
				else if(tap.count==2){
					DFDoubleTap(tap.pos);
				}
			}
			
			if(onMFMultiTapE!=null) onMFMultiTapE(tap);
		}
		
	}
	
	public static void LongTap(Tap tap){
		if(tap.fingerCount>1){
			if(tap.fingerCount==2){
				if(onDFLongTapE!=null) onDFLongTapE(tap.pos);
			}
			if(onMFLongTapE!=null) onMFLongTapE(tap);
		}
		else{
			if(onLongTapE!=null) onLongTapE(tap);
		}
	}
	
	
	//*****************************************************************************//
	//charge
	public static void ChargeStart(ChargedInfo cInfo){
		if(cInfo.fingerCount>1){
			if(onMFChargeStartE!=null) onMFChargeStartE(cInfo);
		}
		else{
			if(onChargeStartE!=null) onChargeStartE(cInfo);
		}
	}
	
	public static void Charging(ChargedInfo cInfo){
		if(cInfo.fingerCount>1){
			if(cInfo.fingerCount==2) DFCharging(cInfo);
			if(onMFChargingE!=null) onMFChargingE(cInfo);
		}
		else{
			if(onChargingE!=null) onChargingE(cInfo);
		}
	}
	
	public static void ChargeEnd(ChargedInfo cInfo){
		if(cInfo.fingerCount>1){
			if(cInfo.fingerCount==2) DFChargingEnd(cInfo);
			if(onMFChargeEndE!=null) onMFChargeEndE(cInfo);
		}
		else{
			if(onChargeEndE!=null) onChargeEndE(cInfo);
		}
	}
	


	
	//*****************************************************************************//
	//dragging
	public static void DraggingStart(DragInfo dragInfo){
		if(dragInfo.fingerCount>1){
			if(onMFDraggingStartE!=null) onMFDraggingStartE(dragInfo);
		}
		else{
			if(onDraggingStartE!=null) onDraggingStartE(dragInfo);
		}
	}
	
	public static void Dragging(DragInfo dragInfo){
		if(dragInfo.fingerCount>1){
			if(dragInfo.fingerCount==2) DFDragging(dragInfo); //obsolete function call
			if(onMFDraggingE!=null) onMFDraggingE(dragInfo);
		}
		else{
			if(onDraggingE!=null) onDraggingE(dragInfo);
		}
	}
	
	public static void DraggingEnd(DragInfo dragInfo){
		if(dragInfo.fingerCount>1){
			if(dragInfo.fingerCount==2) DFDraggingEnd(dragInfo); //obsolete function call
			if(onMFDraggingEndE!=null) onMFDraggingEndE(dragInfo);
		}
		else{
			if(onDraggingEndE!=null) onDraggingEndE(dragInfo);
		}
	}
	
	
	
	
	

	//*****************************************************************************//
	//special event swipe/pinch/rotate
	public static void SwipeStart(SwipeInfo sw){
		if(onSwipeStartE!=null) onSwipeStartE(sw);
	}
	
	public static void Swiping(SwipeInfo sw){
		if(onSwipingE!=null) onSwipingE(sw);
	}
	
	public static void SwipeEnd(SwipeInfo sw){
		if(onSwipeEndE!=null) onSwipeEndE(sw);
	}
	
	public static void Swipe(SwipeInfo sw){
		Debug.Log("In Gesture.Swipe - handler: " + (onSwipeE != null));
		if(onSwipeE!=null) onSwipeE(sw);
	}

	public static void Pinch(float val){
		if(onPinchE!=null) onPinchE(val);
	}
	
	public static void Rotate(float val){
		if(onRotateE!=null) onRotateE(val);
	}
	
	
	//*****************************************************************************//
	//native input down/on/up event
	public static void OnTouchDown(Vector2 pos){
		if(onTouchDownE!=null) onTouchDownE(pos);
	}
	public static void OnTouchUp(Vector2 pos){
		if(onTouchUpE!=null) onTouchUpE(pos);
	}
	public static void OnTouch(Vector2 pos){
		if(onTouchE!=null) onTouchE(pos);
	}
	
	public static void OnMouse1Down(Vector2 pos){
		if(onMouse1DownE!=null) onMouse1DownE(pos);
	}
	public static void OnMouse1Up(Vector2 pos){
		if(onMouse1UpE!=null) onMouse1UpE(pos);
	}
	public static void OnMouse1(Vector2 pos){
		if(onMouse1E!=null) onMouse1E(pos);
	}
	
	public static void OnMouse2Down(Vector2 pos){
		if(onMouse2DownE!=null) onMouse2DownE(pos);
	}
	public static void OnMouse2Up(Vector2 pos){
		if(onMouse2UpE!=null) onMouse2UpE(pos);
	}
	public static void OnMouse2(Vector2 pos){
		if(onMouse2E!=null) onMouse2E(pos);
	}
	
	
	
	//utility for converting vector to angle
	public static float VectorToAngle(Vector2 dir){
		
		if(dir.x==0){
			if(dir.y>0) return 90;
			else if(dir.y<0) return 270;
			else return 0;
		}
		else if(dir.y==0){
			if(dir.x>0) return 0;
			else if(dir.x<0) return 180;
			else return 0;
		}
		
		float h=Mathf.Sqrt(dir.x*dir.x+dir.y*dir.y);
		float angle=Mathf.Asin(dir.y/h)*Mathf.Rad2Deg;
		
		if(dir.y>0){
			if(dir.x<0)  angle=180-angle;
		}
		else{
			if(dir.x>0)  angle=360+angle;
			if(dir.x<0)  angle=180-angle;
		}
		
		//Debug.Log(angle);
		return angle;
	}
	
	
	//utility for tracking a finger input based on fingerId
	public static Touch GetTouch(int ID){
		Touch touch=new Touch();
		if(Input.touchCount>0){
			foreach(Touch touchTemp in Input.touches){
				if(touchTemp.fingerId==ID){
					touch=touchTemp;
					break;
				}
			}
		}
		
		return touch;
	}
	
	
	
	//***************************************************************************//
	//obsolete but still in use, possibly remove by the next update
	//standard tap event
	public delegate void ShortTapHandler(Vector2 pos); 
	public static event ShortTapHandler onShortTapE;
	
	//public delegate void LongTapHandler(Vector2 pos); 
	//public static event LongTapHandler onLongTapE;
	
	public delegate void DoubleTapHandler(Vector2 pos); 
	public static event DoubleTapHandler onDoubleTapE;
	
	
	//Dual-Finger Standard tap event 
	public delegate void DFShortTapHandler(Vector2 pos); 
	public static event DFShortTapHandler onDFShortTapE;
	
	public delegate void DFLongTapHandler(Vector2 pos); 
	public static event DFLongTapHandler onDFLongTapE;
	
	public delegate void DFDoubleTapHandler(Vector2 pos); 
	public static event DFDoubleTapHandler onDFDoubleTapE;
	
	public delegate void DFChargingHandler(ChargedInfo cInfo); 
	public static event DFChargingHandler onDFChargingE;
	
	public delegate void DFChargeEndHandler(ChargedInfo cInfo); 
	public static event DFChargeEndHandler onDFChargeEndE;
	
	
	//Dual-Finger dragging
	public delegate void DFDraggingHandler(DragInfo dragInfo);
	public static event DFDraggingHandler onDualFDraggingE;
	
	public delegate void DFDraggingEndHandler(Vector2 pos); 
	public static event DFDraggingEndHandler onDualFDraggingEndE;
	
	
	//*****************************************************************************//
	//standard
	public static void ShortTap(Vector2 pos){
		if(onShortTapE!=null) onShortTapE(pos);
	}
	
	//public static void LongTap(Vector2 pos){
	//	if(onLongTapE!=null) onLongTapE(pos);
	//}
	
	public static void DoubleTap(Vector2 pos){
		if(onDoubleTapE!=null) onDoubleTapE(pos);
	}
	
	
	//Dual Finger standard tap event
	public static void DFShortTap(Vector2 pos){
		if(onDFShortTapE!=null) onDFShortTapE(pos);
	}
	
	public static void DFLongTap(Vector2 pos){
		if(onDFLongTapE!=null) onDFLongTapE(pos);
	}
	
	public static void DFDoubleTap(Vector2 pos){
		if(onDFDoubleTapE!=null) onDFDoubleTapE(pos);
	}
	
	public static void DFCharging(ChargedInfo cInfo){
		if(onDFChargingE!=null) onDFChargingE(cInfo);
	}
	
	public static void DFChargingEnd(ChargedInfo cInfo){
		if(onDFChargeEndE!=null) onDFChargeEndE(cInfo);
	}
	
	//dual finger drag event
	public static void DFDragging(DragInfo dragInfo){
		if(onDualFDraggingE!=null) onDualFDraggingE(dragInfo);
	}
	
	public static void DFDraggingEnd(DragInfo dragInfo){
		if(onDualFDraggingEndE!=null) onDualFDraggingEndE(dragInfo.pos);
	}
	
	
}


