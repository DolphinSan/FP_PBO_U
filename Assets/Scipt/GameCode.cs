using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameCode : MonoBehaviour
{

    public Button[] cells;
    public Text[] cellTexts;
    public TMP_Text statusText;
    public Text scoreXText, scoreOText;
    public GameObject gameOverScreen;
    public LineRenderer winLine; //cacat fisik

    private int winStartIndex = -1;
    private int winEndIndex = -1;

    private string currentPlayer = "X";
    private string[] board = new string[9];
    private int scoreX = 0, scoreO = 0;
    private bool gameOver = false;
    private int[] winningCells = new int[3];

    private void Start()
    {
        LoadScores();
        ResetBoard();
    }

    private void LoadScores()
    {
        scoreX = PlayerPrefs.GetInt("ScoreX", 0);
        scoreO = PlayerPrefs.GetInt("ScoreO", 0);
        scoreXText.text = scoreX.ToString();
        scoreOText.text = scoreO.ToString();
    }

    private void SaveScores()
    {
        PlayerPrefs.SetInt("ScoreX", scoreX);
        PlayerPrefs.SetInt("ScoreO", scoreO);
        PlayerPrefs.Save();
    }

    private bool CheckWin()
    {
        int[,] winConditions = new int[,] {
            {0,1,2}, {3,4,5}, {6,7,8},
            {0,3,6}, {1,4,7}, {2,5,8},
            {0,4,8}, {2,4,6}
        };

        for (int i = 0; i < winConditions.GetLength(0); i++)
        {
            int a = winConditions[i, 0], b = winConditions[i, 1], c = winConditions[i, 2];
            if (board[a] == currentPlayer && board[b] == currentPlayer && board[c] == currentPlayer)
            {
                winStartIndex = a;
                winEndIndex = c;
                winningCells[0] = a;
                winningCells[1] = b;
                winningCells[2] = c;
                return true;
            }
        }
        return false;
    }

    private bool CheckDraw()
    {
        foreach (string s in board)
        {
            if (s == "") return false;
        }
        return true;
    }

    private void UpdateScore()
    {
        if (currentPlayer == "X")
        {
            scoreX++;
            scoreXText.text = scoreX.ToString();
        }
        else
        {
            scoreO++;
            scoreOText.text = scoreO.ToString();
        }
    }

    public void CellClicked(int index)
    {
        if (board[index] != "" || gameOver) return;

        board[index] = currentPlayer;
        cellTexts[index].text = currentPlayer;

        if (CheckWin())
        {
            statusText.text = $"Player {currentPlayer} Wins!";
            gameOver = true;
            UpdateScore();
            SaveScores();
            gameOverScreen.SetActive(true);

            Vector3 startPos = cells[winStartIndex].transform.position;
            Vector3 endPos = cells[winEndIndex].transform.position;
            DrawWinLine(startPos, endPos);

            foreach (int i in winningCells)
            {
                cells[i].GetComponent<Image>().color = Color.red;
            }
        }
        else if (CheckDraw())
        {
            statusText.text = "Draw!";
            gameOver = true;
            gameOverScreen.SetActive(true);
        }
        else
        {
            currentPlayer = (currentPlayer == "X") ? "O" : "X";
        }
    }

    public void ResetBoard()
    {
        for (int i = 0; i < 9; i++)
        {
            board[i] = "";
            cellTexts[i].text = "";
            cellTexts[i].color = Color.black;
            cells[i].GetComponent<Image>().color = Color.white; 
        }
        currentPlayer = "X";
        gameOver = false;
        statusText.text = "Tic Tak Toe";
        winLine.gameObject.SetActive(false);
    }

    public void ResetScores()
    {
        scoreX = 0;
        scoreO = 0;
        scoreXText.text = "0";
        scoreOText.text = "0";
        PlayerPrefs.DeleteKey("ScoreX");
        PlayerPrefs.DeleteKey("ScoreO");
        PlayerPrefs.Save();
    }

    void DrawWinLine(Vector3 startPos, Vector3 endPos)
    {
        //cacat fisik
        startPos.z = -1;
        endPos.z = -1;
        winLine.gameObject.SetActive(true);
        winLine.positionCount = 2;
        winLine.SetPosition(0, startPos);
        winLine.SetPosition(1, endPos);
        Debug.Log($"Drawing line from {startPos} to {endPos}");
    }
}
