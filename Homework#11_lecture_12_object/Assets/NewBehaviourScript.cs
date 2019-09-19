using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate(0,5 * Time.deltaTime,0);
		//transform.Translate(1 * Time.deltaTime,0,0);
	
	}
}
