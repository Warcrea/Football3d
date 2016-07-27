using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    GameObject ball;

    void Start() {
        ball = GameObject.Find("Ball");
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Select")) {
            ball.GetComponent<Rigidbody>().isKinematic = true;
            ball.transform.position = GameObject.Find("BallSpawner").transform.position;
            ball.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
