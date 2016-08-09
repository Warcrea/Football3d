using UnityEngine;
using System.Collections;

public class BallCollider : MonoBehaviour {

    private PlayerMovement player;

    void Start() {
        Debug.Log("Start");
        player = gameObject.GetComponentInParent<PlayerMovement>();
    }

    void OnTriggerEnter(Collider c) {
        Debug.Log("Hit something");
        if (c.gameObject.transform.name == "Ball") {
            Debug.Log("Hit ball");
            if (!player.dribbling) {
                if (c.gameObject.GetComponent<Ball>().canBeDribbled) {
                    player.StartDribbling();
                }
            }
        }
        
    }

}
