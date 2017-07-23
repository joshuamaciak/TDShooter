using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	public float maxSecondsAlive; // the amount of time the bullet will live
	private float secondsAliveCounter; // the amount of time the bullet has been alive 

	void Start () {
		
	}

	void OnCollisionEnter(Collision collision) {
		GetComponent<Poolable>().objectPool.ReturnObject(gameObject);
	}
	void Update() {
		secondsAliveCounter += Time.deltaTime;
		if(secondsAliveCounter >= maxSecondsAlive) {
			secondsAliveCounter = 0;	// reset object state
			GetComponent<Poolable>().objectPool.ReturnObject(gameObject);
		}
	}
}
