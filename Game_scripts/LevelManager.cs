using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // SAHNE YÖNETİMİ İÇİN ŞART!

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("Level Ayarları")]
    public int currentLevel = 1;
    public int currentXP = 0;
    public int requiredXP = 100;
    public int xpIncreasePerLevel = 20;
    public int totalScore = 0; // Hiç sıfırlanmayan toplam puan

    [Header("UI Referansları")]
    public GameObject bonusMenuPanel;
    public GameObject gameOverPanel; // YENİ: Game Over Paneli
    public Slider healthSlider;
    public Slider xpSlider;
    public TMP_Text levelText;

    [Header("Sesler")]
    public AudioClip gameMusic;

    public Player_sc playerScript;
    
    // Oyunun bitip bitmediğini kontrol eden değişken
    private bool isGameOver = false; 

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Sahne yeniden yüklendiğinde zamanın durmadığından emin ol
        Time.timeScale = 1f; 

        if (playerScript == null) playerScript = FindFirstObjectByType<Player_sc>();
        
        UpdateLevelText();
        UpdateXPUI();

        if (SoundManager.Instance != null && gameMusic != null)
        {
            SoundManager.Instance.PlayMusic(gameMusic);
        }
    }

    private void Update()
    {
        // Eğer oyun bittiyse ve R tuşuna basılırsa
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    // --- OYUNU BİTİREN FONKSİYON ---
    public void GameOver()
    {
        isGameOver = true;
        Debug.Log("OYUN BİTTİ!");

        // Zamanı durdur
        Time.timeScale = 0f;

        // Paneli aç
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    // --- YENİDEN BAŞLATMA ---
    public void RestartGame()
    {
        // Zamanı tekrar normal akışına al (Yoksa yeni oyun donuk başlar)
        Time.timeScale = 1f;

        // Şu anki sahneyi baştan yükle
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ... Diğer AddXP, LevelUp, Button fonksiyonların aynen kalıyor ...
    // (Aşağıdaki kısımlara dokunmana gerek yok, sadece yukarıdakileri ekle/güncelle)

    public void AddXP(int amount)
    {
        currentXP += amount;
        totalScore += amount; // Her XP kazandığında toplam skoru da artır
        UpdateXPUI();
        if (currentXP >= requiredXP) LevelUp();
    }

    void LevelUp()
    {
        currentLevel++;
        currentXP -= requiredXP;
        requiredXP += xpIncreasePerLevel;
        
        if (playerScript != null)
        {
        playerScript.Heal(10); 
        }
        
        UpdateLevelText();
        UpdateXPUI();
        Time.timeScale = 0f;
        if (bonusMenuPanel != null) bonusMenuPanel.SetActive(true);
    }
    
    public void UpdateHealthUI(int current, int max)
    {
        if (healthSlider != null) { healthSlider.maxValue = max; healthSlider.value = current; }
    }
    
    void UpdateXPUI() { if (xpSlider != null) { xpSlider.maxValue = requiredXP; xpSlider.value = currentXP; } }
    
    void UpdateLevelText() { if (levelText != null) levelText.text = "Level: " + currentLevel; }

    public void Button_UpgradeSpeed() { if (playerScript != null) playerScript.UpgradeSpeed(1f); ResumeGame(); }
    public void Button_UpgradeFireRate() { if (playerScript != null) playerScript.UpgradeFireRate(0.2f); ResumeGame(); }

    public void ResumeGame() { Time.timeScale = 1f; if (bonusMenuPanel != null) bonusMenuPanel.SetActive(false); }
}