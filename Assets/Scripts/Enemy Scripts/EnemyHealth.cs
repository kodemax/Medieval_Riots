//#################################################################
//THIS SCRIPT IS RESPONSIBLE FOR MAINTINING THE HEALTH OF THE ENEMY
//,DETECT WHEN THE PLAYER IS HITTING ENEMY AND TO TAKE DAMAGE
//#################################################################
using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

	public int health;
	Animator anim;
	
	private bool swordEnteredZone;
	private bool swordEnteredBody;
	void Start () 
	{
		swordEnteredBody = false;
		swordEnteredZone = false;
		
		anim = GetComponent<Animator>();
	}
	void OnTriggerEnter(Collider collider)
	{
		if(collider.gameObject.tag == "PlayerSword")
		{
			if(swordEnteredZone == false)
				swordEnteredZone = true;
			else
			{
				//GETTING THE DAMAGE OF THE SWORD THAT HIT THE ENEMY
				PlayerWeaponScript script = collider.GetComponent<PlayerWeaponScript>();
				int damage = script.GetDamage();
				health -= 10;
				swordEnteredBody = true;
				if(health < 0)
					anim.SetBool("Dead", true);
			}
		
		}
	}
	void OnTriggerExit(Collider collider)
	{
		if(collider.gameObject.tag == "PlayerSword")
		{
			if(swordEnteredBody == true)
				swordEnteredBody = false;
			else
				swordEnteredZone = false;
		
		}
	}
}
