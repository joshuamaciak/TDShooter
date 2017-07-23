using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * This class is used to implement pooling. Objects will
 * be reused when they become inactive. If there are not enough objects
 * at the time GetObject() is called, a new object will be instantiated.
 * Every call to GetObject() should be paired with a ReturnObject() call, otherwise
 * this will behave no different than normal instantiation when initialSize is reached
**/
public class ObjectPool : MonoBehaviour {
	public GameObject modelObject; // the gameobject that will be instantiated (preferably a prefab)
	private List<PoolEntry> pool;	   // the objects within the pool
	public int initialSize;			   // the number of objects initialize in the pool at start.
	/**
	 * A wrapper struct that represents a poolable object. Contains
	 * only the gameobject & flag representing whether that game object is active.
	 **/
	private class PoolEntry {
		public GameObject gameObject;
		public bool isActive;
	}
	// Use this for initialization
	void Start () {
		pool = new List<PoolEntry>();
		// initialize the bool with inactive game objects
		for(int i = 0; i < initialSize; ++i) {
			JLogger.Log("Initialization");
			GameObject instance = Instantiate(modelObject) as GameObject;
			instance.GetComponent<Poolable>().objectPool = this;
			PoolEntry poolEntry = new PoolEntry();
			poolEntry.gameObject = instance;
			poolEntry.gameObject.SetActive(false);
			poolEntry.isActive   = false;
			pool.Add(poolEntry);
		}
	}
	/**
	 * Called when object becomes active.
	 * Place things that need to happen when the object becomes active here.
	**/
	private void OnBecameActive(PoolEntry poolEntry) {
		poolEntry.gameObject.SetActive(true);
	}
	/**
	 * Called when object becomes inactive.
	 * Place things that need to happen when the object becomes inactive here.
	**/
	private void OnBecameInactive(PoolEntry poolEntry) {
		poolEntry.gameObject.SetActive(false);
	}
	/**
	 * Returns a gameobject from the pool & calls it's SetActive() method.
	 * If all gameobjects are in use, this method will instantiate a new one.
	**/
	public GameObject GetObject() {
		JLogger.Log("ObjectPool::GetObject()");
		for(int i = 0; i < pool.Count; ++i) {
			PoolEntry poolEntry = pool[i];
			if(!poolEntry.isActive) {
				poolEntry.isActive = true;
				OnBecameActive(poolEntry);
				return poolEntry.gameObject;
			}
		}
		// otherwise we need to instantiate a new game object & add it too the pool.
		JLogger.Log("Pool empty! Need to instantiate new object.");
		GameObject instance = Instantiate(modelObject) as GameObject;
		instance.transform.localPosition = Vector3.zero;
		instance.GetComponent<Poolable>().objectPool = this;
		PoolEntry newPoolEntry = new PoolEntry();
		newPoolEntry.gameObject = instance;
		newPoolEntry.isActive   = true;
		pool.Add(newPoolEntry);
		return newPoolEntry.gameObject;
	}
	/**
	 * This method should be called when an object is returned. Will deactivate
	 * the object & place it back in the pool.
	**/
	public void ReturnObject(GameObject returnedObject) {
		JLogger.Log("ObjectPool::ReturnObject()");
		for(int i = 0; i < pool.Count; ++i) {
			PoolEntry poolEntry = pool[i];
			if(poolEntry.gameObject == returnedObject) {
				poolEntry.isActive = false;
				OnBecameInactive(poolEntry);
			}
		}
	}
}
