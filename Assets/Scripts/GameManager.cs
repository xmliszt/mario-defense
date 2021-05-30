using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject winScreen;
    public TMP_Text levelText;
    public TMP_Text scoreText;
    public TMP_Text finalScoreText;
    public int totalLevel;

    public bool isGameover;
    private int score;
    private int currentLevel;
    private AudioManager audioManager;

    void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        winScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        isGameover = false;
        score = 0;
        currentLevel = 1;
        SetLevel(currentLevel);
    }

    public void AddScore(int val)
    {
        if (!isGameover)
        {
            score += val;
            scoreText.text = score.ToString();
        }
    }

    public void GameOver()
    {
        if (!isGameover)
        {
            audioManager.PlayGameover();
            isGameover = true;
            gameOverScreen.SetActive(true);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
        gameOverScreen.SetActive(false);
        winScreen.SetActive(false);
        score = 0;
        currentLevel = 1;
        isGameover = false;
        audioManager.PlayTheme();
    }

    public void NextLevel()
    {
        if (!isGameover)
        {
            currentLevel++;
            if (currentLevel > totalLevel)
            {
                isGameover = true;
                winScreen.SetActive(true);
                finalScoreText.text = string.Format("Final Score: {0}", score);
                audioManager.PlayWin();
            }
            else
            {
                SetLevel(currentLevel);
                audioManager.PlayTheme();
            }
        }
    }

    private void SetLevel(int level)
    {
        levelText.text = string.Format("Level {0}", level);
        if (level > 1 && level <= totalLevel)
        {
            SceneManager.UnloadSceneAsync(level - 1);
        }
        SceneManager.LoadScene(level, LoadSceneMode.Additive);
        StartCoroutine(ActivateScene(level));
    }

    IEnumerator ActivateScene(int level)
    {
        yield return new WaitForSeconds(2);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(level));
    }
}
