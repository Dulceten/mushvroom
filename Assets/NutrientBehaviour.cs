using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NutrientBehaviour : MonoBehaviour {
	private GameObject attachedTo = null;

	// Use this for initialization
	void Start () {
		
	}
	
	public bool attachTo(GameObject to) {
		if (attachedTo == null) {
			attachedTo = to;
			return true;
		}
		
		return false;
	}
	
	public void unattach() {
		attachedTo = null;
	}
	
	// Update is called once per frame
	void Update () {
		if (attachedTo != null) {
			// TODO maybe offset?
			gameObject.GetComponent<Transform>().position = attachedTo.GetComponent<Transform>().position;
		}
	}
}
