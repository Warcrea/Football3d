using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

    public bool canBeDribbled = true;
    private float dribbleTimerReset = 0.5f;
    private float dribbleTimer;

    public GameObject beingDribbledBy;
    public float floorYCoord = 0.28f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (dribbleTimer != 0) {
            canBeDribbled = false;
            dribbleTimer = Mathf.Max(dribbleTimer - Time.deltaTime, 0);
        }
        else canBeDribbled = true;

        if (beingDribbledBy != null) {
            Vector3 correctedYVector = transform.position;
            correctedYVector.y = Mathf.Max(transform.position.y - 0.02f, floorYCoord);
            transform.position = correctedYVector;
        }
	}

    public void beginDribbling(GameObject player) {
        beingDribbledBy = player;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void beShot() {
        dribbleTimer = dribbleTimerReset;
        beingDribbledBy = null;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }

}
