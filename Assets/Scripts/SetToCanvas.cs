using UnityEngine;
using System.Collections;

public class SetToCanvas : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.transform.SetParent (GameObject.FindObjectOfType<Canvas>().transform, false);
	}
}
