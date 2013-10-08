#pragma strict
private var anim : Animator;
function Start () {

}

function Update () 
{
	/*anim = GetComponent(Animator);
	var speed = Input.GetAxis("Vertical");
	anim.SetFloat("Speed", speed);
	 rigidbody.AddForce(transform.forward * speed, ForceMode.VelocityChange);VelocityChange*/
	 //var v :float = Input.GetAxis("Vertical")
	 if(Input.GetAxis("Vertical")>0)
	 {
	 	Debug.Log("Pressed up");
	 }
	 anim = GetComponent(Animator);
	 //anim.SetFloat("Speed",v);
}