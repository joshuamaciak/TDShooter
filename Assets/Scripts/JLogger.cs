using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  A wrapper for Unity's Debug.Log(...). Since Unity provides no simple
 *  way to silence logs (e.g. for release) we need this class.
**/
public class JLogger {
	public static bool ENABLED = true; // set to true if you want to log to the console
	public static void Log(object message) {
		if (ENABLED) {
			Debug.Log (message);
		}
	}
}
