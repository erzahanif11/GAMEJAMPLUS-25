using UnityEngine;
using System.Collections;
using TMPro; 
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; } 

    [Header("UI & Display")]
    public TextMeshProUGUI scoreText;

    [Header("Survival Settings")]
    public float survivalInterval = 5f; 
    public int survivalScore = 3;    
    
    private int currentScore = 0;
    private Coroutine survivalCoroutine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateScoreDisplay();
        survivalCoroutine = StartCoroutine(SurvivalScoring());
    }

    IEnumerator SurvivalScoring()
    {
        while (true)
        {
            yield return new WaitForSeconds(survivalInterval); 
            
            AddScore(survivalScore); 
        }
    }

    public void AddScore(int amount)
    {
        if (amount > 0) 
        {
            currentScore += amount;
            UpdateScoreDisplay();
            
            Debug.Log($"Score bertambah: +{amount}. Total: {currentScore}");
        }
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "SCORE: " + currentScore.ToString();
        }
        else
        {
            Debug.LogError("scoreText (TextMeshProUGUI) belum diset di Inspector!");
        }
    }
    
    public void StopScoring()
    {
        if(survivalCoroutine != null)
        {
            StopCoroutine(survivalCoroutine);
        }
    }
}