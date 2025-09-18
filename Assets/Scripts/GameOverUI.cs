using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [Header("Elementos da UI")]
    public Text gameOverText;
    public Button restartButton;
    public Button quitButton;
    
    void Start()
    {
        // Configura o texto de game over
        if (gameOverText != null)
        {
            gameOverText.text = "GAME OVER";
        }
        
        // Configura os botões
        if (restartButton != null)
        {
            restartButton.GetComponentInChildren<Text>().text = "Reiniciar";
        }
        
        if (quitButton != null)
        {
            quitButton.GetComponentInChildren<Text>().text = "Sair";
        }
    }
}
