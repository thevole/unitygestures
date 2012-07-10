using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//
//public enum _InputType{None, Mouse1, Mouse2, Touch}
//public enum _InputState{On, Down, Up}
//
//
//public class InputEvent{
//	public _InputType inputType=_InputType.None;
//	public _InputState inputState=_InputState.Down;
//	public Vector2 pos=new Vector3(-999, -999);
//	
//	public Touch touch;
//	
//	public InputEvent(){
//		
//	}
//	
//	public InputEvent(Vector3 p, _InputType type, _InputState state){
//		pos=p;
//		inputType=type;
//		inputState=state;
//	}
//	
//	public InputEvent(Touch t){
//		touch=t;
//		inputType=_InputType.Touch;
//		pos=touch.position;
//	}
//}


public class Tap{
	public Vector2 pos;
	public int count;
	
	public int fingerCount=1;
	public Vector2[] positions=new Vector2[1];
	
	public bool isMouse=false;
	public int index=0;
	public int[] indexes=new int[1];
	
	public Tap(Vector2 p, int c):this(p, c, 0, false){}
	public Tap(Vector2 p, int c, int ind):this(p, c, ind, false){}
	public Tap(Vector2 p, int c, int ind, bool im){
		pos=p;
		count=c;
		index=ind;
		isMouse=im;
		
		positions[0]=pos;
		indexes[0]=index;
	}
	public Tap(int c, int fc, Vector2[] ps, int[] inds){
		count=c;
		fingerCount=fc;
		positions=ps;
		indexes=inds;
		
		Vector2 pos=Vector2.zero;
		foreach(Vector2 p in positions){
			pos+=p;
		}
		pos/=positions.Length;
	}
}

public class ChargedInfo{
	public float percent=0;
	public Vector2 pos;
	
	public int fingerCount=1;
	public Vector2[] positions=new Vector2[1];
	
	public bool isMouse=false;
	public int index=0;
	public int[] indexes=new int[1];
	
	//obsolete member
	public Vector2 pos1;
	public Vector2 pos2;
	
	public ChargedInfo(Vector2 p, float val):this(p, val, 0, false) { }
	public ChargedInfo(Vector2 p, float val, int ind, bool im){
		pos=p;
		percent=val;
		index=ind;
		isMouse=im;
		
		positions[0]=pos;
		indexes[0]=ind;
	}
	
	public ChargedInfo(Vector2 p, Vector2[] posL, float val, int[] inds){
		pos=p;
		positions=posL;
		percent=val;
		indexes=inds;
		fingerCount=indexes.Length;
	}
	
	//obsolete constructor
	public ChargedInfo(Vector2 p, float val, Vector2 p1, Vector2 p2):this(p, val, p1, p2, 0, false){}
	public ChargedInfo(Vector2 p, float val, Vector2 p1, Vector2 p2, int ind, bool im){
		pos=p;
		percent=val;
		pos1=p1;
		pos2=p2;
		index=ind;
		isMouse=im;
	}
}

public class DragInfo{
	public Vector2 pos;
	public Vector2 delta;
	
	public int fingerCount=1;
	
	public bool isMouse;
	public int index;
	
	public DragInfo(Vector2 p, Vector2 dir, int fCount):this(p, dir, fCount, 0, false) { }
	public DragInfo(Vector2 p, Vector2 dir, int ind, bool im):this(p, dir, 1, ind, im) { }
	public DragInfo(Vector2 p, Vector2 dir, int fCount, int ind, bool im){
		pos=p;
		delta=dir;
		fingerCount=fCount;
		index=ind;
		isMouse=im;
	}
}

public class SwipeInfo{
	public Vector2 startPoint;
	public Vector2 endPoint;
	
	public Vector2 direction;
	public float angle;
	
	public float duration;
	public float speed;
	
	public int index=0;
	public bool isMouse=false;
	
	public SwipeInfo(Vector2 p1, Vector2 p2, Vector2 dir, float startT, int ind, bool im){
		startPoint=p1;
		endPoint=p2;
		direction=dir;
		angle=Gesture.VectorToAngle(dir);
		duration=Time.time-startT;
		speed=dir.magnitude/duration;
		index=ind;
		isMouse=im;
	}
}




