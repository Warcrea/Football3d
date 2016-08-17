using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    private GameManager gameManager;

    public Text leftScore;
    public Text rightScore;

    public void Start() {
        gameManager = GetComponent<GameManager>();
    }

	public void UpdateScores() {
        leftScore.text = gameManager.leftScore.ToString();
        rightScore.text = gameManager.rightScore.ToString();
    }
}
