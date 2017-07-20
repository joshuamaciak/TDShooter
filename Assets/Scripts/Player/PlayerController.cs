using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	private PlayerMovement playerMovement; // the PlayerMovement object attached to the player
	public VirtualJoystick joystick; 	   // the virtual joystick
	#if UNITY_EDITOR
	private KeyCode lastKeyDown; // the last key that was pressed down
	#endif

	void Start () {
		playerMovement = GetComponent<PlayerMovement>();
	}
	
	void Update () {
		if(joystick.IsJoystickActive()) {
			float joystickValue = joystick.GetValueDegrees();
			playerMovement.ApplyVelocityFromJoystickAngle (joystickValue);
			/*if(joystickValue > 315 || joystickValue < 45) { // needs to be or because it modulus (wrap around bounds)
				playerMovement.StartMovingEast();
			}
			if(joystickValue > 45 && joystickValue < 135) {
				playerMovement.StartMovingNorth();
			}
			if(joystickValue > 135 && joystickValue < 225) {
				playerMovement.StartMovingWest();
			}
			if(joystickValue > 225 && joystickValue < 315)  {
				playerMovement.StartMovingSouth();
			}*/
		} else {
			playerMovement.StopMoving();
		}
		#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.W)) {
			lastKeyDown = KeyCode.W;
			playerMovement.StartMovingNorth ();
		}
		if (Input.GetKeyDown(KeyCode.S)) {
			lastKeyDown = KeyCode.S;
			playerMovement.StartMovingSouth();
		} 
		if (Input.GetKeyDown(KeyCode.D)) {
			lastKeyDown = KeyCode.D;
			playerMovement.StartMovingEast();
		} 
		if (Input.GetKeyDown(KeyCode.A)) {
			lastKeyDown = KeyCode.A;
			playerMovement.StartMovingWest();
		} 

		if (Input.GetKeyUp (lastKeyDown)) {
			playerMovement.StopMoving ();
		}
		#endif
	}
}
