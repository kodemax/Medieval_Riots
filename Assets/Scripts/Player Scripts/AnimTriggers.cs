using UnityEngine;
using System.Collections;

public class AnimTriggers : MonoBehaviour {

	private Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.name == "Jump Trigger")
		{
			anim.SetBool("JumpDown", true);
			Debug.Log("Triggered");
		}
	}
	void OnCollisionEnter(Collision collision) 
	{
		//Debug.Log("The real collision with " + collision.gameObject.name);
		if(collision.gameObject.name == "Terrain")
		{
			//Debug.Log("Landed correctly");
		}
	}
}