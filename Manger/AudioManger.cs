using UnityEngine;
using UnityEngine.UI;

public class AudioManger : MonoBehaviour
{
  public static AudioManger Instance { get; private set; }
  public Slider slider;
  private AudioSource audioSource;

  private void Awake()
  {
    audioSource = GetComponent<AudioSource>();
    Instance = this;
  }
  /// <summary>
  /// 音效
  /// </summary>
  public void PlayClip(string path, float volume = 1)
  {
    AudioClip clip = Resources.Load<AudioClip>(path);
    AudioSource.PlayClipAtPoint(clip, transform.position, volume);
  }
  //通过slider滚动条来控制音量,记得在slider上添加该方法
  public void SliderControll()
  {
    audioSource.volume = slider.value;
  }

}
