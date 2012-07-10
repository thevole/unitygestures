#pragma strict

function Start () {
	
}

function OnEnable(){
	Gesture.onPinchE += OnPinch;
	Gesture.onDraggingE += OnDragging;
}

function OnDisable(){
	Gesture.onPinchE += OnPinch;
	Gesture.onDraggingE -= OnDragging;
}

function Update () {

}

function OnDragging(dir:DragInfo){
	//Debug.Log(pos);
}

function OnPinch(val:float){
	//Debug.Log("Pinching, value: "+val);
}