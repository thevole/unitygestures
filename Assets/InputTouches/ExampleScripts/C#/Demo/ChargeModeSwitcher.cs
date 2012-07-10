using UnityEngine;
using System.Collections;

public class ChargeModeSwitcher : MonoBehaviour {

	public TapDetector tapDetector;

//public enum _ChargeMode{Once, Clamp, Loop, PingPong}

	// Use this for initialization
	void Start () {
		if(tapDetector.chargeMode==_ChargeMode.Once) type=0;
		else if(tapDetector.chargeMode==_ChargeMode.Clamp) type=1;
		else if(tapDetector.chargeMode==_ChargeMode.Loop) type=2;
		else if(tapDetector.chargeMode==_ChargeMode.PingPong) type=3;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private int type=0;
	void OnGUI(){
		
		GUI.Label(new Rect(Screen.width-240, 50, 130, 40), "ChargeMode:");
		
		if(GUI.Button(new Rect(Screen.width-150, 40, 130, 40), tapDetector.chargeMode.ToString())){
			type+=1;
			if(type>3)type=0;
			
			if(type==0) tapDetector.chargeMode=_ChargeMode.Once;
			else if(type==1) tapDetector.chargeMode=_ChargeMode.Clamp;
			else if(type==2) tapDetector.chargeMode=_ChargeMode.Loop;
			else if(type==3) tapDetector.chargeMode=_ChargeMode.PingPong;
		}
	}
	
}
