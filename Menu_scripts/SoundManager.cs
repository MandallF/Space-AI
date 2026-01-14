using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource; // Loop işaretli olmalı
    public AudioSource sfxSource;   // Loop işaretsiz olmalı

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Sahneler arası yok olmasın
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Kayıtlı ses ayarlarını yükle
        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 1f);

        SetMusicVolume(musicVol);
        SetSFXVolume(sfxVol);
    }

    public void SetMusicVolume(float value)
    {
        musicSource.volume = value;
    }

    public void SetSFXVolume(float value)
    {
        sfxSource.volume = value;
    }

    // --- YENİ EKLENEN FONKSİYONLAR ---

    // Arkaplan müziğini değiştirmek için bunu kullanacağız
    public void PlayMusic(AudioClip musicClip)
    {
        // Eğer zaten aynı müzik çalıyorsa baştan başlatma
        if (musicSource.clip == musicClip) return;

        musicSource.Stop();
        musicSource.clip = musicClip;
        musicSource.Play();
    }

    // Tek seferlik ses efektleri (Ateş, Tıklama vb.) için
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}