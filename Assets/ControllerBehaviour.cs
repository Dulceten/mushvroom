    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class ControllerBehaviour : MonoBehaviour {
        private int teleportDelay = 5;
        private float buttonPressDuration = 0;
        private SteamVR_TrackedObject trackedObj;
        private GameObject collidingObj;
        private GameObject objInHand;
        private SteamVR_Controller.Device Controller
        {
            get { return SteamVR_Controller.Input((int)trackedObj.index); }
        }

        void Awake()
        {
            trackedObj = GetComponent<SteamVR_TrackedObject>();
        }

        void TeleportTimer()
        {
            if (Controller.GetAxis() != Vector2.zero)
            {
                buttonPressDuration += Time.deltaTime;
                if (buttonPressDuration > teleportDelay)
                {
                    Teleport();
                }
            }
        }

        void Teleport()
        {
            if (Game.game.startFalseCafeteriaTrue)
            {
                Game.game.rig.GetComponent<Transform>().position = new Vector3(0, 1, 2);
            }
            else
            {
                Game.game.rig.GetComponent<Transform>().position = new Vector3(0, 1, 500);
            }
            Game.game.startFalseCafeteriaTrue = !Game.game.startFalseCafeteriaTrue;
            buttonPressDuration = 0;
        }



        // Update is called once per frame
        void Update()
        {
            TeleportTimer();
            if (Controller.GetHairTriggerDown())
            {
                if (collidingObj && collidingObj.tag == "Nutrient")
                {
                    GrabObject();
                }
            }

            if (Controller.GetHairTriggerUp())
            {
                if (objInHand)
                {
                    ReleaseObject();
                }
            }
           
        }

    void SetCollidingObject(Collider col)
    {
        if (collidingObj || !col.GetComponent<Rigidbody>())
        {
            return;
        }

        collidingObj = col.gameObject;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            print("controllerCOllidesWithEnemy");
            //Destroy(other.gameObject);
            print(Controller.velocity);
            other.gameObject.GetComponent<Rigidbody>().velocity = Controller.velocity * 10.0f;
            //other.gameObject.GetComponent<Rigidbody>().angularVelocity = gameObject.GetComponent<Rigidbody>().angularVelocity;
        }
        else if (other.gameObject.tag == "MainTree")
        {
            Game.game.treeHealth += Game.game.nutrientCount;
            Game.game.nutrientCount = 0;
            print(Game.game.nutrientCount);
        }
        SetCollidingObject(other);
    }

    void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    void OnTriggerExit(Collider other)
    {
        if (!collidingObj)
        {
            return;
        }

        collidingObj = null;
    }

    private void GrabObject()
    {
        objInHand = collidingObj;
        collidingObj = null;
        var joint = AddFixedJoint();
        joint.connectedBody = objInHand.GetComponent<Rigidbody>();
    }

    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    private void ReleaseObject()
    {
        if (GetComponent<FixedJoint>())
        {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());

            objInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
            objInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
        }

        objInHand = null;
    }
}