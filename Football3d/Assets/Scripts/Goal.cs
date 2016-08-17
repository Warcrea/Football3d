using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

    public Team opposition;
    private GameManager gameManager;

	// Use this for initialization
	void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}

    void OnCollisionEnter(Collision c) {
        if (c.gameObject.transform.name == "Ball") {
            Debug.Log("Goal for " + opposition );
            gameManager.Goal(opposition);
        }
    }
}
