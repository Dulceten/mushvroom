using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
	public GameObject gnatPrefab;
	public GameObject squirrelPrefab;
	public GameObject treePrefab;
	public GameObject nutrientPrefab;

	private GameObject mainTree;

	private const int radius = 100;
	private const int treeCount = 120;
	public int treeHealth = 5;

    private Vector3 MAIN_TREE = new Vector3(0, 1, 0);
    private Vector3 NUTRIENT_TREE = new Vector3(0, 1, 500);

	private int maxEnemies = 5;
    public List<GameObject> enemiesArray;

	private const int treeLossInterval = 30;
	
	public GameObject rig;
    public int nutrientCount = 0;
    public bool startFalseCafeteriaTrue = false;
	public int nutrientsInCafe = 0;
	private int maxNutrients = 4;

    public static Game game;

	void spawnTrees () {
		for (int i = 0; i < treeCount; i++) {
			float angle = Random.Range (0, Mathf.PI * 2);
			Vector3 pos =  MAIN_TREE + new Vector3 (Mathf.Cos (angle), 0, Mathf.Sin (angle)) * (Random.Range (10, radius));
			Instantiate (treePrefab, pos, Quaternion.identity);
		}
	}

	void spawnNutrients() {
		Vector3 pos = Game.game.rig.GetComponent<Transform> ().position;
		while (nutrientsInCafe < maxNutrients) {
			float angle = Random.Range (0, Mathf.PI * 2);
			Vector3 post =  pos + new Vector3 (Mathf.Cos (angle), 0.5f, Mathf.Sin (angle)) * (Random.Range (0.5f,1.0f));
			Instantiate(nutrientPrefab, post, Quaternion.identity);
			nutrientsInCafe += 1;
		}
	}
	
	public void respawnEnemies() {
		for(int i = 0; i < enemiesArray.Count; i++)
        {
            Object.Destroy(enemiesArray[i]); 
        }
        spawnEnemies();
	}
		
	// Use this for initialization
	void Start () {
		game = this;
		
		spawnTrees ();
		
		// mainTree = world.find<Tree>();
		// or
		//Instantiate (mainTree, new Vector3 (0, 0, 0), Quaternion.identity);
	}

	void spawnEnemies () {
		while (enemiesArray.Count < maxEnemies) {
			float angle = Random.Range (0, Mathf.PI * 2);
			Vector3 pos = new Vector3 (Mathf.Cos (angle), 0, Mathf.Sin (angle)) * (radius + 5);
			int squirrelOneGnatZero = Random.Range (0, 1);
			if (squirrelOneGnatZero == 1) {
                enemiesArray.Add(Instantiate (squirrelPrefab, pos, Quaternion.identity));

            } else {
				pos.y = Random.Range (5, 10);
                enemiesArray.Add(Instantiate (gnatPrefab, pos, Quaternion.identity));
			}
		}
	}

	void decreaseTree() {
		if (Time.realtimeSinceStartup % treeLossInterval == (treeLossInterval - 1)) {
            treeHealth -= 1;
		}
	}

	void winOrLose() {
		if (treeHealth >= 20) {
			Debug.Log("you won");
		}
		if (treeHealth < 0) {
			Debug.Log ("tree ded :<");
		}
	}

	// Update is called once per frame
	void Update () {
		spawnEnemies ();

		spawnNutrients ();

		decreaseTree ();

		winOrLose ();

        if (nutrientCount < 0)
        {
            // Go back to main tree if we die.
            rig.GetComponent<Transform>().position = new Vector3(0, 0, 2);
            nutrientCount = 0;

            // Respawn enemies outside the forest to avoid spawnkill.
            Game.game.respawnEnemies();
        }
    }
}
