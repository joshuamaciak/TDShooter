using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Any gameobject that is used in the ObjectPool should
 * have a Poolable script that contains a reference to 
 * the pool it came from.
**/
public class Poolable : MonoBehaviour {
	public ObjectPool objectPool; // the ObjectPool that this object belongs to
}
