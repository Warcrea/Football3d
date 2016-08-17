using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    GameObject ball;

    public bool linkedControls; //when true both teams are controlled by the same controller, useful for testing
    public int leftScore;
    public int rightScore;

    private UIManager ui;

    void Start() {
        ball = GameObject.Find("Ball");
        leftScore = rightScore = 0;

        ui = GetComponent<UIManager>();
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Select")) {
            ball.GetComponent<Rigidbody>().isKinematic = true;
            ball.transform.position = GameObject.Find("BallSpawner").transform.position;
            ball.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    public void Goal(Team team) {
        if ((int)team == 0) leftScore++;
        else if ((int)team == 1) rightScore++;

        ui.UpdateScores();
    }
}
