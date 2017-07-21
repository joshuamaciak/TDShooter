using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {
	public GameObject weaponObject;			// the weapon game object (should be instantiated from prefab)
	private Weapon weapon;
	private bool firing;
	private float secondsSinceLastShot;
	// Use this for initialization
	void Start () {
		if(weapon == null) {
			weaponObject = Instantiate(Resources.Load("Prefabs/Weapons/GUN0"), transform) as GameObject;
			weaponObject.transform.localPosition = new Vector3(0,0,1);
		}
		weapon = weaponObject.GetComponent<Weapon>();
		firing = true;
	}
	
	void FixedUpdate () {
		if(firing) {
			if(secondsSinceLastShot >= 1/weapon.rateOfFire) {
				weapon.Fire();
				this.GetComponent<PlayerMovement>().ApplyBackwardsForce(200);
			}
		}
		if(secondsSinceLastShot >= 1/weapon.rateOfFire) secondsSinceLastShot = 0; // reset timer if we shot in this update
		else secondsSinceLastShot += Time.deltaTime;
	}
	void StartFiring() {
		this.firing = true;
	}
	void StopFiring() {
		this.firing = false;
	}
	void SetWeapon(GameObject weaponObject) {
		this.weaponObject = weaponObject;
		this.weapon = weaponObject.GetComponent<Weapon>();
	}
}
