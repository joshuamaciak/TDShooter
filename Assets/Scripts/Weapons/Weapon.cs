using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
	public int id;				// unique id
	public string name; 		// name
	public float damage;		// amount of damage one bullet inflicts
	public float rateOfFire; 	// the rate of fire of the weapon (in bullets/second)
	public float recoilAmount;  // the amount of force a bullet inflicts on the player
	public float range;			// the distance the bullet will travel
	public float bulletSpeed; 	// the speed of the bullet
	public int weight;			// the weight of the weapon [1,3] (heavier = slower player)
	
	//private GameObject bullet;
	private ObjectPool bulletPool;
	void Start() {
		bulletPool = GetComponent<ObjectPool>();
	}
	public void Fire() {
		JLogger.Log("bang!");
		GameObject newBullet = bulletPool.GetObject();
		newBullet.transform.position    = transform.position;
		newBullet.transform.rotation    = transform.rotation;
		newBullet.GetComponent<Rigidbody>().velocity = newBullet.transform.forward * bulletSpeed;
		// todo implement code that spawns bullet, etc...

	}
}
