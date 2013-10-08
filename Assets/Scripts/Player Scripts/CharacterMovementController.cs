using UnityEngine;
using System.Collections;

[RequireComponent(typeof (CapsuleCollider))]
public class CharacterMovementController : MonoBehaviour 
{	
	private CharacterController characterController;
	
	
	//WEAPONS STUFF
	public bool hasBasicSword;
	
	//ANIMATION STUFF
	private Animator anim;
	static int idleState = Animator.StringToHash("Base Layer.Idle");
	static int locoState = Animator.StringToHash("Base Layer.Locomotion");
	static int jumpState = Animator.StringToHash("Base Layer.Jump");
	static int jumpDownState = Animator.StringToHash("Base Layer.Jump Down");
	static int fallState = Animator.StringToHash("Base Layer.Fall");
	static int rollState = Animator.StringToHash("Base Layer.Roll");
	static int AttackSword1State = Animator.StringToHash("Base Layer.Attack Sword 1");
	private AnimatorStateInfo currentBaseState;
	
	//ENEMY STUFF
	public Transform enemy;
	public float lookWeight;
	
	//TEMP VARIABLES
	float directionTemp;
	//COLLIDER STUFF
	private CapsuleCollider col;
	void Start () 
	{
		anim = GetComponent<Animator>();
		characterController = GetComponent<CharacterController>();
		
		col = GetComponent<CapsuleCollider>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		float speed = Input.GetAxis("Vertical");
		float direction = Input.GetAxis("Horizontal");
		
		if(direction > 0)
		{
			directionTemp = Mathf.Lerp(directionTemp, 1, Time.deltaTime*4);
		}
		else if(direction < 0)
		{
			directionTemp = Mathf.Lerp(directionTemp, -1, Time.deltaTime*4);
		}
		else
		{
			directionTemp = Mathf.Lerp(directionTemp, 0, Time.deltaTime*4);
		}
		
		anim.SetFloat("Speed",speed);
		anim.SetFloat("Direction", directionTemp);
		currentBaseState = anim.GetCurrentAnimatorStateInfo(0);
		
		
		//LOOKATSTUFF
		anim.SetLookAtWeight(lookWeight);
		if(Input.GetButton("Fire2"))
		{
			// ...set a position to look at with the head, and use Lerp to smooth the look weight from animation to IK (see line 54)
			anim.SetLookAtPosition(enemy.position);
			lookWeight = Mathf.Lerp(lookWeight,1f,Time.deltaTime);
		}
		// else, return to using animation for the head by lerping back to 0 for look at weight
		else
		{
			lookWeight = Mathf.Lerp(lookWeight,0f,Time.deltaTime);
		}
		
		//JUMP STUFF
		if(currentBaseState.nameHash == locoState)
		{
			if(Input.GetButtonDown("Jump"))
			{				
				anim.SetBool("Jump", true);
			}
		}
		else if(currentBaseState.nameHash == jumpState)
		{
			if(!anim.IsInTransition(0))
			{	
				//*6 -> SINCE THE VALUE IN THE ANIMATION AND THE MESH SCALES ARE DIFFERENT
				characterController.height = anim.GetFloat("ColliderHeight")*6;
				anim.SetBool("Jump", false);
			}
			// Raycast down from the center of the character.. 
			Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
			RaycastHit hitInfo = new RaycastHit();
			
			if (Physics.Raycast(ray, out hitInfo))
				if (hitInfo.distance > 6f)
					anim.MatchTarget(hitInfo.point, Quaternion.identity, AvatarTarget.Root, new MatchTargetWeightMask(new Vector3(0, 1, 0), 0), 0.45f, 0.5f);
		}
		
		//FALLING STUFF
		else if (currentBaseState.nameHash == jumpDownState)
		{
			//characterController.center = new Vector3(0, anim.GetFloat("ColliderY"), 0);
		}
		
		// if we are falling, set our Grounded boolean to true when our character's root 
		// position is less that 0.6, this allows us to transition from fall into roll and run
		// we then set the Collider's Height equal to the float curve from the animation clip
		else if (currentBaseState.nameHash == fallState)
		{
			characterController.height = anim.GetFloat("ColliderHeight")*6;
		}
		
		// if we are in the roll state and not in transition, set Collider Height to the float curve from the animation clip 
		// this ensures we are in a short spherical capsule height during the roll, so we can smash through the lower
		// boxes, and then extends the collider as we come out of the roll
		// we also moderate the Y position of the collider using another of these curves on line 128
		else if (currentBaseState.nameHash == rollState)
		{
			if(!anim.IsInTransition(0))
			{
				Debug.Log("I am done");		
				anim.SetBool("JumpDown", false);
				characterController.height = anim.GetFloat("ColliderHeight")*6;
				characterController.center = new Vector3(0, 0+6, 0);	
			}
		}
		
		//COMBAT STUFF
		if ( Input.GetKeyDown(KeyCode.LeftControl) == true )
		{
			anim.SetBool("Attack_Sword_1", true);
		}
		else if (currentBaseState.nameHash == AttackSword1State)
		{
			if(!anim.IsInTransition(0))
			{
				anim.SetBool("Attack_Sword_1", false);
			}
		}
	}
}
