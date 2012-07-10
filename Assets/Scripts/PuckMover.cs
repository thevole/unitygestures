using UnityEngine;
using System.Collections;

public class PuckMover : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Move(float speed, float direction) {
		float rads = Mathf.Deg2Rad * direction;
		Vector3 force = new Vector3(Mathf.Cos(rads), 0.0f, Mathf.Sin(rads));
		
		force = force.normalized * speed;
		rigidbody.AddForce(force);		
	}
}
