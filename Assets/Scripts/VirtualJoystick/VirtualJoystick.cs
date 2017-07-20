using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualJoystick : MonoBehaviour {
	public GameObject pointer; 		// the joystick's pointer
	private bool clickInProgress;	// flag that shows whether a click is in progress
	private bool touchInProgress;   // flag that shows whether a touch is in progress
	private int activeTouchId;			// the id of touch that is controlling joystick movement
	public float pointerAngle; 		// the angle of the pointer relative to the base (base is the unit circle)
	public Ray ray;
	void Start () {

	}

	/**
	 * Checks to see if a touch started inside the VirtualJoystick 
	 **/
	public bool DidTouchStart() {
		if (Application.isMobilePlatform) {
			RaycastHit hit;
			foreach (Touch t in Input.touches) {
				JLogger.Log ("Touch:" + t.fingerId + " Phase:" + t.phase);
				if (t.phase == TouchPhase.Began) {
					activeTouchId = t.fingerId;
					JLogger.Log ("Touch id = " + activeTouchId);
					Vector3 adjPosition = new Vector3(t.position.x, Screen.height - t.position.y);
					JLogger.Log ("normal: " + t.position + " adj:" + adjPosition);
					ray = Camera.main.ScreenPointToRay (t.position);
					JLogger.Log ("ray:" + ray);
					Debug.DrawRay (ray.origin, ray.direction);
					if (Physics.Raycast (ray, out hit) && hit.collider.gameObject.name.Equals (this.gameObject.name)) {
						JLogger.Log ("Touch started inside joystick");
						return true;
					} 
				}
			}
		}
		return false;
	}
	/**
	 * For editor.
	 * Checks to see if a click started inside the VirtualJoystick.
	 **/
	#if UNITY_EDITOR
	public bool DidClickStart() {
		if (!Application.isMobilePlatform) {
			if (Input.GetMouseButtonDown (0)) {
				RaycastHit hit;
				ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				Debug.DrawRay (ray.origin, ray.direction * 10000, Color.green);
				JLogger.Log ("ray:" + ray);

				if (Physics.Raycast (ray, out hit) && hit.collider.gameObject.name.Equals (this.gameObject.name)) {
					JLogger.Log ("Click started inside joystick");
					return true;
				}
			}
		}
		return false;
	}
	void OnDrawGizmosSelected() {
		Gizmos.color = Color.green;
		Gizmos.DrawRay (ray.origin, ray.direction);
	}
	private void OnClickEnded() {
		JLogger.Log ("OnClickEnded()");
		JLogger.Log ("Click ended.");
		clickInProgress = false;
		pointer.transform.localPosition = Vector3.zero;
	}
	#endif
	private void OnTouchEnded() {
		JLogger.Log ("OnTouchEnded()");
		JLogger.Log ("Touch ended.");
		touchInProgress = false;
		pointer.transform.localPosition = Vector3.zero;
	}
	/**
	 * Returns the value of the joystick pointer (angle in degrees)
	 **/
	public float GetValueDegrees() {
		JLogger.Log ("Joystick value (degrees): " + pointerAngle);
		return pointerAngle;
	}
	/**
	 *  Returns true is the joystick is currently being used
	 **/
	public bool IsJoystickActive() {
		return (clickInProgress || touchInProgress);
	}
	private float CalculatePointerAngleOffset() {
		Vector3 dir = pointer.transform.localPosition;
		float rads = Mathf.Atan2 (dir.y , dir.x);
		return (rads < 0) ? 360 + Mathf.Rad2Deg * rads : Mathf.Rad2Deg * rads; // make degrees out of 360 instead of negative
	}
	/**
	 *  Returns the touch with the given id. Used to keep track of one touch between frames.
	 **/ 
	private Touch GetTouchById(int touchId) {
		foreach(Touch t in Input.touches) {
			if (t.fingerId == touchId) {
				JLogger.Log ("Found touch: " + touchId);
				return t;
			}
		}
		Touch invalidTouch = new Touch();
		invalidTouch.fingerId = -1;
		return invalidTouch;
	}
	void Update () {
		if (!touchInProgress) {
			if (DidTouchStart ()) {
				touchInProgress = true;
			}
		}
		#if UNITY_EDITOR
		if(!clickInProgress) {
			if (DidClickStart()) {
				clickInProgress = true;
			}
		}
		#endif

		if (touchInProgress) {
			Touch activeTouch = GetTouchById (activeTouchId);
			if (activeTouch.fingerId == -1) { // couldn't find touch. this should never be executed.
				JLogger.Log ("Critical error: uh oh! Couldn't find active touch. Ending touch.");
				OnTouchEnded ();
			} else {
				if (activeTouch.phase == TouchPhase.Ended) {
					JLogger.Log ("TouchPhase is ended. Ending touch.");
					OnTouchEnded ();
				} else {
					Ray ray = Camera.main.ScreenPointToRay (activeTouch.position);
					Debug.DrawRay (ray.origin, ray.direction * 10000, Color.red);
					RaycastHit hit;

					// this moves the pointer to a point on the ray & ensures that  localPosition.z is 0
					Vector3 pointOnRay = ray.GetPoint (7.5f); // don't know why but 7.5 is a magic number, not transform.localPosition.z
					pointer.transform.position = pointOnRay;
					pointer.transform.localPosition = new Vector3(pointer.transform.localPosition.x, pointer.transform.localPosition.y, 0);

					// this ensures that the pointer stays within the base... clamps the magnitude of the localPosition 
					float localRadius = .06f;
					pointer.transform.localPosition = Vector3.ClampMagnitude (pointer.transform.localPosition, localRadius);
				}
			}
		}
		#if UNITY_EDITOR
		if (clickInProgress) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			Debug.DrawRay (ray.origin, ray.direction * 10000, Color.red);
			RaycastHit hit;

			// this moves the pointer to a point on the ray & ensures that  localPosition.z is 0
			Vector3 pointOnRay = ray.GetPoint (7.5f); // don't know why but 7.5 is a magic number, not transform.localPosition.z
			pointer.transform.position = pointOnRay;
			pointer.transform.localPosition = new Vector3(pointer.transform.localPosition.x, pointer.transform.localPosition.y, 0);

			// this ensures that the pointer stays within the base... clamps the magnitude of the localPosition 
			float localRadius = .06f;
			pointer.transform.localPosition = Vector3.ClampMagnitude (pointer.transform.localPosition, localRadius);
		}

		if (clickInProgress && Input.GetMouseButtonUp (0)) {
			OnClickEnded ();
		}
		#endif
		pointerAngle = CalculatePointerAngleOffset ();
	}
}
