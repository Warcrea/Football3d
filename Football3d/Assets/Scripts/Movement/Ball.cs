using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

    public bool canBeDribbled = true;
    private float dribbleTimerReset = 0.5f;
    private float dribbleTimer;

    public GameObject beingDribbledBy;

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
	}

    public void beginDribbling(GameObject player) {
        beingDribbledBy = player;
    }

    public void beShot() {
        dribbleTimer = dribbleTimerReset;
        beingDribbledBy = null;
    }
}
