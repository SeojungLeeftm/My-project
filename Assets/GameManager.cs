using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameoverText;
    public GameObject victoryText; // 승리 UI 추가
    public Text timeText;
    public Text recordText;
    public float maxTime = 180f; // 3분(기본값), Inspector에서 수정 가능
    private float remainingTime;
    private float surviveTime;
    private bool isGameOver;
    public bool isVictory;
    public Slider healthSlider; // 체력 슬라이더 참조 추가
    private PlayerController player;

    void Start()
    {
        surviveTime = 0;
        remainingTime = maxTime;
        isGameOver = false;
        isVictory = false;
        
        if (gameoverText != null) gameoverText.SetActive(false);
        if (victoryText != null) victoryText.SetActive(false);

        player = FindObjectOfType<PlayerController>();
        if (healthSlider != null && player != null)
        {
            healthSlider.maxValue = player.GetMaxHealth();
            healthSlider.value = player.GetCurrentHealth();
        }
    }

    void Update()
    {
        if(!isGameOver && !isVictory)
        {
            surviveTime += Time.deltaTime;
            remainingTime -= Time.deltaTime;
            timeText.text = "남은 시간: " + FormatTime(remainingTime);

            if(remainingTime <= 0)
            {
                Victory();
            }

            if (healthSlider != null && player != null)
            {
                healthSlider.value = player.GetCurrentHealth();
            }
        }
        else if(isGameOver || isVictory)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene("SampleScene");
            }
        }
    }

    public void EndGame()
    {
        isGameOver = true;
        gameoverText.SetActive(true);
        float bestTime = PlayerPrefs.GetFloat("BestTime");
        if (surviveTime > bestTime)
        {
            bestTime = surviveTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
        }
        recordText.text = "최고 기록: " + FormatTime(bestTime);
    }

    private void Victory()
    {
        isVictory = true;
        if(victoryText != null) victoryText.SetActive(true);
        float bestTime = PlayerPrefs.GetFloat("BestTime");
        if (surviveTime > bestTime)
        {
            bestTime = surviveTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
        }
        recordText.text = "최고 기록: " + FormatTime(bestTime);
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = (int)(timeInSeconds / 60);
        int seconds = (int)(timeInSeconds % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public bool IsVictory()
    {
        return isVictory;
    }
}
