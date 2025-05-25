using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject gameOverScreen; 
    public GameObject pauseScreen;
    public GameCode gameCode; 
    
    private void Awake()
    {
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
        
        // Make sure to assign the GameCode reference in the inspector
        if (gameCode == null)
            gameCode = FindObjectOfType<GameCode>();
    }

    #region Pause
    public void PauseGame(bool status)
    {
        pauseScreen.SetActive(status);

        if(status){
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    #endregion

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(pauseScreen.activeInHierarchy)
            {
                PauseGame(false);
            }
            else
            {
                PauseGame(true);
            }
        }
    }

    #region Restart
    public void RestartGame()
    {
        Time.timeScale = 1;
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
        

        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
        // saat gamecode mulai, score juga di load
    }
    #endregion
    
    #region Quit
    public void QuitGame()
    {

        if (gameCode != null)
            gameCode.ResetScores();
            //hnaya reset score ketika quit game
            
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    #endregion
}