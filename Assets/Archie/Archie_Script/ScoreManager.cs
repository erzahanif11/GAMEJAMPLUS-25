using UnityEngine;
using System.Collections;
using TMPro; 

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; } 

    [Header("UI & Display")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText; // NEW: UI untuk menampilkan High Score

    [Header("Survival Settings")]
    public float survivalInterval = 5f; 
    public int survivalScore = 3;    
    
    // NEW: Pengaturan High Score
    [Header("High Score Settings")]
    private const string HIGHSCORE_KEY = "HighScore"; // Kunci penyimpanan di PlayerPrefs
    
    private int currentScore = 0;
    private int highScore = 0; // NEW: Variabel untuk menyimpan High Score saat ini
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
        // NEW: Muat High Score saat game dimulai
        LoadHighScore();
        
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
            
            // NEW: Cek High Score setiap kali skor bertambah (opsional, tapi bagus untuk feedback instan)
            CheckForNewHighScore(false); 
            AudioManager.instance.PlaySFX(AudioManager.instance.blockJatuh);
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
        
        // NEW: Update tampilan High Score
        if (highScoreText != null)
        {
            highScoreText.text = "HIGH SCORE: " + highScore.ToString();
        }
    }
    
    // NEW: Fungsi untuk memuat High Score dari PlayerPrefs
    private void LoadHighScore()
    {
        // PlayerPrefs.GetInt(key, defaultValue)
        highScore = PlayerPrefs.GetInt(HIGHSCORE_KEY, 0); 
    }

    // NEW: Fungsi utama untuk mengecek dan menyimpan High Score baru
    // Parameter 'isGameOver' digunakan untuk memastikan penyimpanan hanya terjadi saat game over
    public void CheckForNewHighScore(bool isGameOver = true)
    {
        if (currentScore > highScore)
        {
            // Skor saat ini lebih tinggi dari High Score yang tersimpan
            highScore = currentScore;
            
            // Simpan ke PlayerPrefs secara permanen
            PlayerPrefs.SetInt(HIGHSCORE_KEY, highScore);
            PlayerPrefs.Save(); // Memastikan data ditulis ke disk
            
            Debug.Log("NEW HIGH SCORE! " + highScore);
            UpdateScoreDisplay(); // Update UI High Score
        } 
        // Jika isGameOver TRUE dan skor tidak mencapai High Score baru, 
        // kita tetap memanggil UpdateScoreDisplay untuk memastikan UI terakhir terupdate.
        else if (isGameOver)
        {
            UpdateScoreDisplay();
        }
    }

    public void StopScoring()
    {
        if(survivalCoroutine != null)
        {
            StopCoroutine(survivalCoroutine);
        }
        
        // NEW: Pastikan High Score dicek dan disimpan saat game berakhir
        CheckForNewHighScore(); 
    }
}