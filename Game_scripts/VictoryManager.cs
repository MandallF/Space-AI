using UnityEngine;
using TMPro; // Metinler için TextMeshPro kullanıyoruz
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager Instance;

    [Header("UI Panelleri")]
    public GameObject victoryPanel; // Inspector'dan VictoryPanel objesini sürükle

    [Header("Skor Metinleri")]
    public TextMeshProUGUI scoreText;     // "Score: 0" yazan text
    public TextMeshProUGUI bestScoreText; // "Best Score: 0" yazan text

    private bool isVictory = false;

    void Awake()
    {
        // Singleton yapısı: Diğer scriptlerden kolayca erişmek için
        if (Instance == null) Instance = this;
    }

    void Update()
    {
        // Zafer ekranı açıkken R'ye basılırsa menüye dön
        if (isVictory && Input.GetKeyDown(KeyCode.R))
        {
            // Menüye dönmeden önce zamanı normale döndür
            Time.timeScale = 1f; 
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void OnBossKilled(int finalScore)
    {
        isVictory = true;
        
        // Paneli görünür yap
        if (victoryPanel != null) victoryPanel.SetActive(true);

        // Mevcut skoru yazdır
        if (scoreText != null) scoreText.text = "Score: " + finalScore;

        // En yüksek skoru PlayerPrefs ile kontrol et ve kaydet
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        if (finalScore > bestScore)
        {
            bestScore = finalScore;
            PlayerPrefs.SetInt("BestScore", bestScore);
            PlayerPrefs.Save();
        }

        // En yüksek skoru yazdır
        if (bestScoreText != null) bestScoreText.text = "Best Score: " + bestScore;

        // Oyunu durdur (AI ve hareketler dursun)
        Time.timeScale = 0f; 
    }
}