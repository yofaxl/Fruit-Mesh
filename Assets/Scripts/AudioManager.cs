using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip deathSound;
    public AudioClip fruitSound;
    public AudioClip jumpSound;
    public AudioClip damageSound;
    public AudioClip uiButtonSound;
    public AudioClip winSound;
    public AudioClip teleportSound;

    private AudioSource musicSource;
    private AudioSource sfxSource; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 

           
            musicSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();

            
            musicSource.loop = true; 
            musicSource.volume = 0.5f; 

            
            sfxSource.volume = 1.0f; 

          
            PlayBackgroundMusic();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void PlayBackgroundMusic()
    {
        if (musicSource != null && backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Music source or background music clip is not assigned.");
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip); 
        }
        else
        {
            Debug.LogWarning("SFX source or audio clip is not assigned.");
        }
    }


    public void PlayDeathSound() { PlaySFX(deathSound); }
    public void PlayFruitSound() { PlaySFX(fruitSound); }
    public void PlayJumpSound() { PlaySFX(jumpSound); }
    public void PlayDamageSound() { PlaySFX(damageSound); }
    public void PlayUIButtonSound() { PlaySFX(uiButtonSound); }
    public void PlayWinSound() { PlaySFX(winSound); }
    public void PlayTeleportSound() { PlaySFX(teleportSound); }

    
    public void StopMusic() { if (musicSource != null) musicSource.Stop(); }
    public void StopSFX() { if (sfxSource != null) sfxSource.Stop(); }
} 