using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnCollisionEnter(Collision col) {
        print("Head collides with " + col.gameObject.tag);
		if (col.gameObject.tag == "Nutrient") {
			Game.game.nutrientCount += 1;
			Game.game.nutrientsInCafe -= 1;
			Object.Destroy(col.gameObject);
		}
		else if (col.gameObject.tag == "Enemy") {
			Game.game.nutrientCount -= 1;
			Object.Destroy(col.gameObject);
		}
	}	
}
