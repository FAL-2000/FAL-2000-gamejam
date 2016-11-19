#pragma strict

//Valtter Taipale 2013 IGP

class RotateController extends MonoBehaviour
{

	public var Rotate_Object : Transform;
	public var rotate_speed : double = 1;
	public var rotate_forward : boolean = false;
	public var rotate_x : boolean = false;	//x?
	public var rotate_up : boolean = false;
	 
	 //enpäs muista miten nuo menee akseleissa joten tälläne :P

	function Start () {
	
	}
	
	function Update () {
	
		if (rotate_forward == true){
			Rotate_Object.Rotate(Vector3.forward * Time.deltaTime*rotate_speed);
		}
		if (rotate_x == true){
			Rotate_Object.Rotate(Vector3.right * Time.deltaTime*rotate_speed);
		}
		if (rotate_up == true){
			Rotate_Object.Rotate(Vector3.up * Time.deltaTime*rotate_speed);
		}

	}
}