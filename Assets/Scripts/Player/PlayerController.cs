using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	private PlayerMovement playerMovement; 		   // the PlayerMovement object attached to the player
	public VirtualJoystick movementJoystick; 	   // the virtual joystick that controls movement
	public VirtualJoystick lookJoystick;		   // the virtual joystick that controls look rotation
	#if UNITY_EDITOR
	private KeyCode lastKeyDown; // the last key that was pressed down
	#endif

	void Start () {
		playerMovement = GetComponent<PlayerMovement>();
	}
	
	void Update () {
		if(movementJoystick.IsJoystickActive()) {
			float joystickValue = movementJoystick.GetValueDegrees();
			playerMovement.ApplyVelocityFromJoystickAngle (joystickValue);
		} else {
			playerMovement.StopMoving();
		}
		if(lookJoystick.IsJoystickActive()) {
			float joystickValue = lookJoystick.GetValueDegrees();
			playerMovement.ApplyLookRotationFromJoystickAngle(joystickValue);
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
