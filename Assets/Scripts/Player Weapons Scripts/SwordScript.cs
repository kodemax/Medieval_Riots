using UnityEngine;
using System.Collections;

public class SwordScript : PlayerWeaponScript {
	
	public int damage;
	public bool collected;
	void Start () 
	{
		//FOR TESTING PURPOSES ONLY
		if(collected)
			attachToPlayer();
	}
	
	void OnTriggerEnter(Collider other)
	{
		//DURING COLLECTION BY PLAYER
		if(other.gameObject.name == "Player" && !collected)
			attachToPlayer();
	}
	void attachToPlayer()
	{
		collected = true;
		
		//DESTROYING THE PARTICLE SYSTEM
		GameObject particleSystem = GameObject.Find("SwordMagic");
		Object.Destroy(particleSystem);
		
		//PARENTING THE SWORD TO THE 
		GameObject player = GameObject.Find("Player");
		Component[] children;
		children = player.GetComponentsInChildren<Transform>();
		foreach(Component child in children)
		{
			if(child.name == "SwordHolder")
			{
				transform.parent = child.transform;
				break;
			}
		}
			
		transform.localRotation = Quaternion.identity;
		transform.localPosition = Vector3.zero;
	}
	public override int GetDamage()
	{
		return damage;
	}
	
}
