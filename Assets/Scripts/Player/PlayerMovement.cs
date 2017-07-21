using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This script controls the player's movement.
 * The player's state has two primary attributes related to movement: 
 * the direction it is moving and the direction it is facing. This implies that a player
 * can move one way while independently changing its direction (i.e. the player can walk north while facing south, etc)
 **/
public class PlayerMovement : MonoBehaviour {
	private Rigidbody rigidBody; 	 // the rigidbody of the player. this is what we will apply velocities & forces to.
	public float speed;		 	 	 // the speed at which the player can move. this will be constant, but needs to be tinkered with
	public Direction direction; 	 // the direction that the player is currently moving in
	public bool moving;				 // flag that specifies whether or not the player is currently moving

	// defines the four cardinal directions (this probably won't be needed when we eventually use polar coordinates for finer movement)
	public enum Direction		
	{
		NORTH, SOUTH, EAST, WEST
	};

	void Start () {
		rigidBody = GetComponent<Rigidbody>();
	}
	/**
	 *  Method that starts motion in a given direction. 
	 **/
	private void StartMovingDirection(Direction dir) {
		this.direction = dir;
		this.moving = true;
	}
	/**
	 *  Method that starts motion north. 
	 **/
	public void StartMovingNorth() {
		this.direction = Direction.NORTH;
		this.moving = true;
	}
	/**
	 *  Method that starts motion south. 
	 **/
	public void StartMovingSouth() {
		this.direction = Direction.SOUTH;
		this.moving = true;
	}
	/**
	 *  Method that starts motion east. 
	 **/
	public void StartMovingEast() {
		this.direction = Direction.EAST;
		this.moving = true;
	}
	/**
	 *  Method that starts motion west. 
	 **/
	public void StartMovingWest() {
		this.direction = Direction.WEST;
		this.moving = true;
	}
	public void StopMoving() {
		this.moving = false;
		rigidBody.velocity = Vector3.zero;
	}
	/**
	* Applies a force in the opposite direction the player is moving.
	* (used for gun recoil)
	**/
	public void ApplyBackwardsForce(float force) {
		rigidBody.AddForce(transform.forward * -1 * force);
	}
	/**
	 *  Takes the angle of the joystick in degrees & converts it to a unit vector representing velocity 
	 **/
	public void ApplyVelocityFromJoystickAngle(float degrees) {
		float rads = degrees * Mathf.Deg2Rad;
		moving = true;
		rigidBody.velocity = new Vector3 (Mathf.Cos (rads), 0, Mathf.Sin (rads)) * speed;
	}
	public void ApplyLookRotationFromJoystickAngle(float degreesToBeAt) {
		// W  -> N
		// 90 -> 0
		// 0 -> 90
		// 180 -> -90
		// 270 -> 180
		transform.rotation = Quaternion.Euler( 0, 90 - degreesToBeAt, 0);
	}
	/**
	 *  Takes in a cardinal direction & returns a velocity vector corresponding to that direction.
	 **/
	private Vector3 GetVelocityFromDirection(Direction dir) {
		switch (direction) {
			case Direction.NORTH:
				return new Vector3(0, 0, -speed);
			case Direction.SOUTH:
				return new Vector3(0, 0, +speed);
			case Direction.EAST:
				return new Vector3(-speed, 0, 0);
			case Direction.WEST:
				return new Vector3(+speed, 0, 0);
		}		
		return Vector3.zero;
	}
	void FixedUpdate() {

	}
}
