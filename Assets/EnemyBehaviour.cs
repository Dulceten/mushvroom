using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {
    public float minY = 0.0f;
    public float acceleration = 1.0f;
    public float maxVel = 5.0f;

    void Update()
    {
        // Find the direction to the player.
        Vector3 pos = gameObject.GetComponent<Transform>().position;
        var children = Game.game.rig.GetComponentsInChildren<Transform>();
        Vector3 dir = new Vector3(0, 0, 0);
        foreach (Transform child in children)
        {
            if (child.gameObject.tag == "MainCamera")
            {
                dir = child.position - pos;
                break;
            }

        }
        dir /= dir.magnitude;

        gameObject.GetComponent<Rigidbody>().velocity += dir * acceleration * Time.deltaTime;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(gameObject.GetComponent<Rigidbody>().velocity, maxVel);
        //if (pos.y < minY)
        //{
        // gameObject.GetComponent<Transform>().position.y = 2;
        // }

    }

    void OnDestroy()
    {
        Game.game.enemiesArray.Remove(gameObject);
    }
}
