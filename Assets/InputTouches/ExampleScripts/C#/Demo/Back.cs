using UnityEngine;
using System.Collections;

public class Back : MonoBehaviour {

	void OnGUI(){
		if(GUI.Button(new Rect(10, 10, 130, 35), "Back to Menu")){
			Application.LoadLevel("Menu");
		}
	}
	
}
