using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Slider için gerekli
using System.IO;      // Dosya okuma için gerekli

public class MainMenu : MonoBehaviour
{
    [Header("UI Ayarları")]
    public GameObject gameModePanel; // Sürükleyip bırakacağın Panel (Seçim Ekranı)

    [Header("UI Sliderları")]
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Ses Dosyaları")]
    public AudioClip menuMusic; 
    public AudioClip clickSFX;  

    private void Start()
    {
        // --- SLIDER VE SES BAŞLANGIÇ AYARLARI (Senin kodun aynen korundu) ---
        if (PlayerPrefs.HasKey("MusicVolume")) musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        else musicSlider.value = 1f;

        if (PlayerPrefs.HasKey("SFXVolume")) sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        else sfxSlider.value = 1f;

        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);

        if (menuMusic != null && SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayMusic(menuMusic);
        }

        // Başlangıçta panel kapalı olsun
        if(gameModePanel != null) 
            gameModePanel.SetActive(false);
    }

    // --- BUTON FONKSİYONLARI ---

    // 1. Ana Menüdeki "OYNA" (Play) butonuna bunu bağla
    public void OpenModeSelection()
    {
        PlayClickSound(); // Tık sesi
        if (gameModePanel != null)
            gameModePanel.SetActive(true); // Paneli aç
    }

    // 2. Paneldeki "KENDİM OYNA" (Human) butonuna bunu bağla
    public void StartAsHuman()
    {
        GameData.isTrainingMode = true;  // Sen oynarken öğrenmeye devam etsin
        GameData.isAILoaded = false;     // AI kontrolü kapalı
        
        PlayClickSound();
        LoadGameScene();
    }

    // 3. Paneldeki "AI BAŞLAT" (Robot) butonuna bunu bağla
        public void StartAsAI()
    {
        // WebGL uyumlu yükleme: Resources klasöründen okur
        // Dosya uzantısını (.txt veya .json) yazmana gerek yok, sadece ismini yaz.
        TextAsset brainFile = Resources.Load<TextAsset>("ai_weights");

        if (brainFile != null)
        {
            GameData.jsonBrainData = brainFile.text; // Dosyanın içindeki metni al
            GameData.isAILoaded = true;
            Debug.Log("AI Dosyası Resources Klasöründen Yüklendi!");
        }
        else
        {
            GameData.isAILoaded = false; 
            Debug.Log("HATA: Resources klasöründe 'ai_weights' dosyası bulunamadı!");
        }

        GameData.isTrainingMode = false; // AI oynarken eğitim kapalı
        
        PlayClickSound();
        LoadGameScene();
    }

    // 4. Paneldeki Çarpı (X) veya İptal butonuna bunu bağla
    public void CloseModeSelection()
    {
        PlayClickSound();
        if (gameModePanel != null)
            gameModePanel.SetActive(false);
    }

    // --- YARDIMCI FONKSİYONLAR ---

    void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene"); 
    }

    void PlayClickSound()
    {
        if (clickSFX != null && SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX(clickSFX);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // --- SES AYARLARI (Senin kodun aynen korundu) ---
    public void OnMusicVolumeChanged(float value)
    {
        if (SoundManager.Instance != null) SoundManager.Instance.SetMusicVolume(value);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void OnSFXVolumeChanged(float value)
    {
        if (SoundManager.Instance != null) SoundManager.Instance.SetSFXVolume(value);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }
}