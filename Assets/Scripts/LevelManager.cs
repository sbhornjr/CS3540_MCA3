using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public string nextLevel;
    public int numSkeletons = 10;
    public static int numSkeletonsRemaining = 10;
    public static bool isGameOver = false;
    public Text scoreText;
    public Text gameText;

    private void Awake()
    {
        numSkeletonsRemaining = numSkeletons;
        isGameOver = false;
        gameText.gameObject.SetActive(false);
        SetScoreText();
    }

    public void SetScoreText()
    {
        scoreText.text = "Skeletons Remaining: " + numSkeletonsRemaining.ToString();
        if (numSkeletonsRemaining <= 0) LevelWon();
    }


    public void LevelLost()
    {
        isGameOver = true;
        gameText.text = "LEVEL FAILED";
        gameText.gameObject.SetActive(true);

        Camera.main.GetComponent<AudioSource>().pitch = 1;
        //AudioSource.PlayClipAtPoint(gameOverSFX, Camera.main.transform.position);

        Invoke("LoadCurrentLevel", 2);
    }

    public void LevelWon()
    {
        isGameOver = true;
        gameText.text = "LEVEL COMPLETE";
        gameText.gameObject.SetActive(true);

        Camera.main.GetComponent<AudioSource>().pitch = 3;
        //AudioSource.PlayClipAtPoint(gameWonSFX, Camera.main.transform.position);

        if (!string.IsNullOrEmpty(nextLevel)) Invoke("LoadNextLevel", 2);
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }

    void LoadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
