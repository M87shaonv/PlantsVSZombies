using UnityEngine;
using UnityEngine.UI;

public class BgMusicManger : MonoBehaviour
{
  public static BgMusicManger Instance { get; private set; }
  public Slider slider;
  public AudioSource musicSource;
  void Awake()
  {
    Instance = this;
    musicSource = GetComponent<AudioSource>();
  }
  void Start()
  {
    StopMusic();
  }

  public void StartMusic(string path)
  {
    AudioClip bgm = Resources.Load<AudioClip>(path);
    musicSource.clip = bgm;
    musicSource.Play();
  }
  public void StopMusic()
  {
    musicSource.Stop();
  }
  public void ResumeBackgroundMusic()
  {
    musicSource.UnPause();
  }
  public void SliderControll()
  {
    musicSource.volume = slider.value;
  }
}