using UnityEngine;
using System.Collections;
using UnityEngine.UI; // UI işlemleri için şart!

public class Enemy_sc : MonoBehaviour
{
    [Header("Hareket Ayarlari")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float leftLimit = -7.5f;
    [SerializeField] private float rightLimit = 7.5f;
    private bool movingRight = true;

    [Header("Spawner Ayarlari")]
    [SerializeField] private GameObject minionPrefab;
    [SerializeField] private float baseSpawnInterval = 2f;
    private float spawnTimer;

    [Header("Boss Can ve UI Ayarlari")]
    [SerializeField] private int health = 1000;
    [SerializeField] private Slider bossHealthSlider; // Inspector'dan atayacağız
    
    private int maxHealth;
    private bool isInvulnerable = true; 
    private bool vulnerabilityTriggered = false; 

    [Header("Ses Efektleri")]
    [SerializeField] private AudioClip bossFightMusic; // Heyecanlı Boss müziği
    private AudioClip normalGameMusic; // Eski müziği hafızada tutmak için
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        transform.position = new Vector3(0f, 3.33f, 0f);
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        maxHealth = health;
        
        // --- UI BAŞLANGIÇ AYARLARI ---
        if (bossHealthSlider != null)
        {
            bossHealthSlider.maxValue = maxHealth;
            bossHealthSlider.value = health;
            bossHealthSlider.gameObject.SetActive(false); // Başlangıçta gizle
        }

        UpdateBossColor();

        if (SoundManager.Instance != null)
        {
        normalGameMusic = SoundManager.Instance.musicSource.clip;
        }
    }

    void Update()
    {
        BossMovement();
        HandleSpawning();
        CheckVulnerability();
    }

    void BossMovement()
    {
        if (movingRight)
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            if (transform.position.x >= rightLimit) movingRight = false;
        }
        else
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            if (transform.position.x <= leftLimit) movingRight = true;
        }
    }

    void HandleSpawning()
    {
        float currentLevel = LevelManager.instance != null ? LevelManager.instance.currentLevel : 1;
        float currentSpawnRate = Mathf.Max(0.5f, baseSpawnInterval - (currentLevel * 0.1f));

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            SpawnMinion();
            spawnTimer = currentSpawnRate;
        }
    }

    void SpawnMinion()
    {
        if (minionPrefab != null) Instantiate(minionPrefab, transform.position, Quaternion.identity);
    }

    void CheckVulnerability()
    {
        if (LevelManager.instance == null) return;

        int currentLevel = LevelManager.instance.currentLevel;

        if (currentLevel % 10 == 0 && currentLevel > 0 && !vulnerabilityTriggered)
        {
            StartCoroutine(VulnerablePhase());
        }
        
        if (currentLevel % 10 != 0)
        {
            vulnerabilityTriggered = false;
        }
    }

    IEnumerator VulnerablePhase()
    {
        vulnerabilityTriggered = true;
        isInvulnerable = false;
        UpdateBossColor(); 
        
        // --- BAR GÖRÜNSÜN ---
        if (bossHealthSlider != null) 
            bossHealthSlider.gameObject.SetActive(true);
        
        if (SoundManager.Instance != null && bossFightMusic != null)
        {
        SoundManager.Instance.PlayMusic(bossFightMusic);
        }

        Debug.Log("BOSS SAVUNMASIZ! SALDIR!");

        yield return new WaitForSeconds(30f); 

        // --- SÜRE DOLDU ---
        isInvulnerable = true;
        UpdateBossColor(); 
        
        if (health > 0)
        {
            health = maxHealth;
            // --- BARI DOLDUR VE GİZLE ---
            if (bossHealthSlider != null)
            {
                bossHealthSlider.value = health;
                bossHealthSlider.gameObject.SetActive(false);
            }

        if (SoundManager.Instance != null && normalGameMusic != null)
        {
            SoundManager.Instance.PlayMusic(normalGameMusic);
        }

            Debug.Log("SÜRE DOLDU! Boss iyileşti.");
        }
    }

    void UpdateBossColor()
    {
        if (spriteRenderer == null) return;
        if (isInvulnerable) spriteRenderer.color = Color.white; 
        else spriteRenderer.color = Color.red;   
    }

    public void TakeDamage(int amount)
    {
        if (isInvulnerable) return; 

        if (LevelManager.instance != null) LevelManager.instance.AddXP(1);

        health -= amount;
        
        if (bossHealthSlider != null) bossHealthSlider.value = health;

        if (health <= 0)
        {
            Debug.Log("BOSS ÖLDÜ!");
            if (bossHealthSlider != null) bossHealthSlider.gameObject.SetActive(false);

            // --- ZAFER EKRANINI TETİKLE ---
        if (VictoryManager.Instance != null)
        {
            int scoreToSend = 0;
            
            if (LevelManager.instance != null)
            {
                
                scoreToSend = LevelManager.instance.totalScore;
                
            }
            
            VictoryManager.Instance.OnBossKilled(scoreToSend);
        }

            Destroy(gameObject);
        }
    }
}