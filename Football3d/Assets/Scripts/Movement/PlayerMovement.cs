using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    private enum States {none, dribbling, turningToPass};
    private States state;

    public int playerNumber = 0;
    private GameObject teammate;
    public float angleToTeammate;

    public float acceleration = 2f;  //Force applied when running
    public float deceleration = 4f;
    private float horizSpeed = 0f;
    private float verticSpeed = 0f;

    private float turnSmoothing = 15f;
    private float passingTurnSpeed = 10f;

    public float maxSpeed = 100.0f;  //Maximum velocity
    public float shootForce = 800f;
    public float passForce = 400f;
    public float passAngle = 25f;

    //States
    public bool dribbling;

    //References
    private GameObject gameManager;
    private SoundManager soundManager;
    private GameObject ball;
    public GameObject particles;

	// Use this for initialization
	void Start () {
        state = States.none;
        ball = GameObject.Find("Ball");
        gameManager = GameObject.Find("GameManager");
        soundManager = gameManager.GetComponent<SoundManager>();

        if (playerNumber == 1) teammate = GameObject.Find("Player 2");
        else if (playerNumber == 2) teammate = GameObject.Find("Player 1");
	}

    // Update is called once per frame
    void Update() {
        float vertical = 0;
        float horizontal = 0;
        float trigger = 0;
        bool bumper = false;

        if (playerNumber == 1) {
            vertical = Input.GetAxisRaw("Team1Player1Vertical");
            horizontal = Input.GetAxisRaw("Team1Player1Horizontal");
            trigger = Input.GetAxisRaw("Team1Player1Trigger");
            bumper = Input.GetButtonDown("Team1Player1Bumper");
        }
        else if (playerNumber == 2) {
            vertical = Input.GetAxisRaw("Team1Player2Vertical");
            horizontal = Input.GetAxisRaw("Team1Player2Horizontal");
            trigger = Input.GetAxisRaw("Team1Player2Trigger");
            bumper = Input.GetButtonDown("Team1Player2Bumper");
        }
        
        MovementManagement(horizontal, vertical);
        CalculateAngleToTeammate();
        
        if (state == States.none) { CheckForBall(); }
        else if(state == States.dribbling) {

            if (trigger > 0) {
                Shoot();
            }
            else if (bumper) {
                Pass();
            }
            
        }
        else if (state == States.turningToPass) {
            if (angleToTeammate > 165 && angleToTeammate < 180) {
                Pass();
            }
            else
                RotateToFaceVector(teammate.transform.position);
        }
    }

    void MovementManagement(float horizontal, float vertical) {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        Vector3 position = gameObject.transform.position;

        //Update speeds based on input
        if (horizontal > 0) {
            horizSpeed = Mathf.Min(maxSpeed, (horizSpeed + acceleration) * horizontal);
        } else if (horizontal < 0) {
            horizSpeed = Mathf.Max(-maxSpeed, (horizSpeed - acceleration) * -horizontal);
        }
        else {
            if (horizSpeed > 0) horizSpeed = Mathf.Max(0, horizSpeed - deceleration);
            else if (horizSpeed < 0) horizSpeed = Mathf.Min(0, horizSpeed + deceleration);
        }

        if (vertical > 0) {
            verticSpeed = Mathf.Min(maxSpeed, (verticSpeed + acceleration) * vertical);
        }
        else if (vertical < 0) {
            verticSpeed = Mathf.Max(-maxSpeed, (verticSpeed - acceleration) * -vertical);
        }
        else {
            if (verticSpeed > 0) verticSpeed = Mathf.Max(0, verticSpeed - deceleration);
            else if (verticSpeed < 0) verticSpeed = Mathf.Min(0, verticSpeed + deceleration);
        }

        RotateToFaceDirection(horizontal, vertical);

        //Move to the next position
        Vector3 nextPosition = new Vector3(position.x + horizSpeed* Time.deltaTime, position.y, position.z + verticSpeed *Time.deltaTime);
        rigidbody.MovePosition(nextPosition);
    }

    void RotateToFaceDirection(float horizontal, float vertical) {
        if (!(horizontal == 0 && vertical == 0)) {
            // Create a new vector of the horizontal and vertical inputs.
            Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);

            // Create a rotation based on this new vector assuming that up is the global y axis.
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

            // Create a rotation that is an increment closer to the target rotation from the player's rotation.
            Quaternion newRotation = Quaternion.Lerp(GetComponent<Rigidbody>().rotation, targetRotation, turnSmoothing * Time.deltaTime);

            // Change the players rotation to this new rotation.
            GetComponent<Rigidbody>().MoveRotation(newRotation);
        }
    }

    void RotateToFaceVector(Vector3 target) {
        Vector3 targetDir = target - transform.position;
        float step = passingTurnSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
        Debug.DrawRay(transform.position, newDir, Color.red);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

   public void StartDribbling() {
        state = States.dribbling;
       GameObject ball = GameObject.Find("Ball");

       ball.transform.SetParent(transform);
       ball.GetComponent<Ball>().beginDribbling(gameObject);
   }

   
    void Shoot() {
        GameObject ball = GameObject.Find("Ball");
        Rigidbody ballPhysics = ball.GetComponent<Rigidbody>();
        ball.transform.SetParent(null);
        ball.GetComponent<Ball>().beShot();

        Vector3 shootVector = new Vector3();
        shootVector = gameObject.GetComponent<Rigidbody>().velocity + transform.forward * shootForce;
        shootVector.y += (float)0.5 * shootForce;

        ballPhysics.AddForce(shootVector);
        state = States.none;

        particles.GetComponent<ParticleSystem>().Play();
        soundManager.KickSound();
    }

    void Pass() {
        Vector3 target = teammate.transform.FindChild("BallCollider").transform.position;

        if (angleToTeammate > 165 && angleToTeammate < 180) {
            GameObject ball = GameObject.Find("Ball");
            Rigidbody ballPhysics = ball.GetComponent<Rigidbody>();
            ball.GetComponent<Ball>().beShot();

            ball.transform.SetParent(null);
            
            ball.GetComponent<Rigidbody>().velocity = BallisticVelocity(passAngle);
            state = States.none;
            particles.GetComponent<ParticleSystem>().Play();
            soundManager.PassSound();
        } else {
            state = States.turningToPass;
        }
    }

    Vector3 BallisticVelocity(float angle) {
        Vector3 target = teammate.transform.position;

        Vector3 dir = target - transform.position;
        float h = dir.y;
        dir.y = 0;
        float dist = dir.magnitude;
        float a = angle * Mathf.Deg2Rad;
        dir.y = dist * Mathf.Tan(a);
        dist += h / Mathf.Tan(a);
        float vel = ((Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a)) * 90) / 100);
        return vel * dir.normalized;
    }
  
    void CheckForBall() {
        Vector3 directionToBall = transform.position - GameObject.Find("Ball").transform.position;
        float angle = Vector3.Angle(transform.forward, directionToBall);
        float distance = directionToBall.magnitude;
        if (Mathf.Abs(angle) < 275 && Mathf.Abs(angle) > 200 && distance < 1) {
            StartDribbling();
        }
    }

    void DirectionRay() {
        //Debug.DrawLine(gameObject.transform.position, ball.transform.position, Color.red);
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, gameObject.transform.position);
        lineRenderer.SetPosition(1, gameObject.transform.position + gameObject.transform.forward);
    }

    void CalculateAngleToTeammate() {
        Vector3 target = teammate.transform.position;
        float angle = 10;
        angleToTeammate = (Vector3.Angle(transform.forward, transform.position - target));
    }

}
