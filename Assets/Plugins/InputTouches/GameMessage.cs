using UnityEngine;
using System.Collections;

public class GameMessage : MonoBehaviour {

	public GUIText displayText;
	[HideInInspector] public GUIText guiText2;
	
	public static GameMessage gameMessage;
	
	private bool displayed=false;
	private float timeDisplay;
	
	private bool displayed2=false;
	private float timeDisplay2;
	
	public static bool init=false;
	private GameObject messageObj;
	
	void Awake () {
		gameMessage=this;
		messageObj=gameObject;
		Init();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnDisable(){
		init=false;
	}
	
	static public void Init(){
		//Debug.Log("init");
		
		if(gameMessage==null){
			GameObject obj=new GameObject();
			obj.name="GameMessage";
			gameMessage=obj.AddComponent<GameMessage>();
			
			gameMessage.messageObj=obj;
		}
		
		init=true;
		
		if(gameMessage.displayText==null){
			GameObject obj=new GameObject();
			obj.name="guiText1";
			
			Transform t=obj.transform;
			t.parent=gameMessage.messageObj.transform;
			t.position=new Vector3(1-(10f/Screen.width), 0, 1);
			
			gameMessage.displayText=obj.AddComponent<GUIText>();
			
			gameMessage.displayText.alignment = TextAlignment.Right;
			gameMessage.displayText.anchor = TextAnchor.LowerRight;
		}
		
		if(gameMessage.guiText2==null){
//			GameObject obj=new GameObject();
//			obj.name="guiText2";
//			
//			Transform t=obj.transform;
//			t.parent=gameMessage.messageObj.transform;
//			t.position=new Vector3(1-(10f/Screen.width), 0, 1);
//			
//			gameMessage.guiText2=obj.AddComponent<GUIText>();
//			
//			gameMessage.guiText2.alignment = TextAlignment.Right;
//			gameMessage.guiText2.anchor = TextAnchor.LowerRight;
		}
	}
	
	static public void DisplayMessage(string str){
		if(!init){
			GameObject obj=new GameObject();
			obj.name="GameMessage";
			gameMessage=obj.AddComponent<GameMessage>();
			gameMessage.messageObj=obj;
		
			Init();
		}
		
		gameMessage.DisplayMsg(str);
	}
	
	void DisplayMsg(string str){
		timeDisplay=Time.time;
		displayText.text=displayText.text+str+"\n";
		//displayText.text=str;
		if(!displayed){
			displayed=true;
			StartCoroutine(DisplayRoutine());
		}
	}
	
	IEnumerator DisplayRoutine(){
		while(Time.time-timeDisplay<3){
			yield return null;
		}
		displayed=false;
		displayText.text="";
	}
	
	static public void DisplayMessage2(string str){
		//if(gameMessage==null) Init();
		gameMessage.DisplayMsg2(str);
	}
	
	void DisplayMsg2(string str){
		timeDisplay2=Time.time;
		guiText2.text=str;
		if(!displayed2){
			displayed2=true;
			StartCoroutine(DisplayRoutine2());
		}
	}
	
	IEnumerator DisplayRoutine2(){
		while(Time.time-timeDisplay2<2){
			yield return null;
		}
		displayed2=false;
		guiText2.text="";
	}
	
}
