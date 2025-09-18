using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int totalScore;
    public Text scoreText;
    public GameObject gameOver;
    public static GameController instance;
    
    void Start()
    {
        instance = this;
        // Start is called before the first frame update
    }
    
    public void UpdateScoreText()
    {
        scoreText.text = totalScore.ToString();
    }
    
    public void ShowGameOver()
    {
        gameOver.SetActive(true);
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
