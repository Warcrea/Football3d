using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    GameObject ball;

    public bool linkedControls; //when true both teams are controlled by the same controller, useful for testing

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
