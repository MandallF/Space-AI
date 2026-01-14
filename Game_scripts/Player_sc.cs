using System.Collections;
using UnityEngine;

public class Player_sc : MonoBehaviour
{
    [Header("Hareket Ayarlari")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float maxSpeedLimit = 15f; 

    [Header("Ateşleme Ayarlari")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireInterval = 2f;
    [SerializeField] private float minFireInterval = 0.1f; 
    private float fireTimer; 

    [SerializeField] private Transform muzzlePosition; 

    [Header("Can Ayarlari")]
    [SerializeField] private int maxHealth = 100; 
    private int currentHealth;

    [Header("Ses Efektleri")]
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioClip deathSound;

    void Start()
    {
        transform.position = Vector3.zero;
        currentHealth = maxHealth; 

        if (LevelManager.instance != null)
        {
            LevelManager.instance.UpdateHealthUI(currentHealth, maxHealth);
        }
    }

    void Update()
    {
        AgentController ai = GetComponent<AgentController>();

        // 1. HAREKET KONTROLÜ
        // Eğer AI modu kapalıysa veya Eğitimdeysek kontrol sende (Klavye)
        // Not: AI aktifse zaten kendi Update'inden cubeMovement'ı çağıracak.
        if (ai == null || !ai.aiActive || ai.trainingMode)
        {
            cubeMovement(); 
        }

        HandleShooting(); // Ateşleme tamamen otomatik, Update içinde.
    }

    // --- DEĞİŞİKLİK BURADA BAŞLIYOR ---
    // Fonksiyonu PUBLIC yaptık ve içine parametreler ekledik.
    // customHorizontal = -99f demek: "Eğer kimse bir sayı göndermezse -99 kabul et"
    public void cubeMovement(float customHorizontal = -99f, float customVertical = -99f)
    {
        float horizontalInput;
        float verticalInput;

        // Eğer dışarıdan (AI'dan) veri gelmediyse (-99 ise), KLAVYEYİ oku
        if (customHorizontal == -99f)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
        }
        else // Eğer AI bir veri gönderdiyse ONU KULLAN
        {
            horizontalInput = customHorizontal;
            verticalInput = customVertical;
        }

        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f);

        if (movement.magnitude > 1f)
            movement.Normalize();

        transform.position += movement * speed * Time.deltaTime;

        // Ekran sınırları (Teleport)
        if (transform.position.x <= -8.9f)
            transform.position = new Vector3(8.9f, transform.position.y, 0);
        else if (transform.position.x >= 8.9f)
            transform.position = new Vector3(-8.9f, transform.position.y, 0);

        // Y ekseni sınırlama
        float clampedY = Mathf.Clamp(transform.position.y, -4.5f, 0f);
        transform.position = new Vector3(transform.position.x, clampedY, 0f);
    }
    // --- DEĞİŞİKLİK BURADA BİTİYOR ---

    void HandleShooting()
    {
        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0f)
        {
            FireBullet();
            fireTimer = fireInterval;
        }
    }

    void FireBullet()
    {
        if(bulletPrefab != null)
        {
            Instantiate(bulletPrefab, muzzlePosition.position, Quaternion.identity);
        }

        if (SoundManager.Instance != null && shootSound != null)
        {
            SoundManager.Instance.PlaySFX(shootSound);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (LevelManager.instance != null)
        {
            LevelManager.instance.UpdateHealthUI(currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            Debug.Log("Player öldü!");
            Die();
        }
    }
    
    private void Die()
    {
        if (SoundManager.Instance != null && deathSound != null)
        {
            SoundManager.Instance.PlaySFX(deathSound);
        }

        if (LevelManager.instance != null)
        {
            LevelManager.instance.GameOver();
        }

        Destroy(gameObject);
    }

    // Bonus fonksiyonları aynen kalabilir
    public void UpgradeSpeed(float amount)
    {
        speed += amount;
        if (speed > maxSpeedLimit) speed = maxSpeedLimit;
    }

    public void UpgradeFireRate(float amount)
    {
        fireInterval -= amount;
        if (fireInterval < minFireInterval) fireInterval = minFireInterval;
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        
        // Can, maksimum canı geçemesin
        if (currentHealth > maxHealth) 
            currentHealth = maxHealth;

        // Can barını (UI) güncelle
        if (LevelManager.instance != null)
        {
            LevelManager.instance.UpdateHealthUI(currentHealth, maxHealth);
        }
    }
}