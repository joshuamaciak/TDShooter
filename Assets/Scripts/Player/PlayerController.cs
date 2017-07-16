using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	private PlayerMovement playerMovement;
	void Start () {
		playerMovement = GetComponent<PlayerMovement>();
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.W)) {
			playerMovement.StartMovingNorth ();
		} else if (Input.GetKeyDown(KeyCode.S)) {
			playerMovement.StartMovingSouth();
		} else if (Input.GetKeyDown(KeyCode.D)) {
			playerMovement.StartMovingEast();
		} else if (Input.GetKeyDown(KeyCode.A)) {
			playerMovement.StartMovingWest();
		} else {
			playerMovement.StopMoving();
		}
	}
}
