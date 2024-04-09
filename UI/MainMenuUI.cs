using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenuUI : MonoBehaviour
{
  /// <summary>
  /// 更改用户名界面
  /// </summary>
  public GameObject alterUserName;
  /// <summary>
  /// 更改用户名输入框
  /// </summary>
  public InputField nameInputField;
  /// <summary>
  /// 显示用户名文本框
  /// </summary>
  public Text userNameText;
  /// <summary>
  /// 冒险模式关卡数
  /// </summary>
  public Text level;
  void Start()
  {
    level.text = PlayerPrefs.GetInt("Level").ToString();
    UpdateName();
  }
  /// <summary>
  /// 显示更改用户名界面
  /// </summary>
  public void OnNameButtonClick()
  {
    AudioManger.Instance.PlayClip(Config.ButtonOnClick);
    alterUserName.SetActive(true);
  }
  /// <summary>
  /// 确认更改用户名
  /// </summary>
  public void OnConfirmButtonClick()
  {
    AudioManger.Instance.PlayClip(Config.ButtonOnClickTap);
    PlayerPrefs.SetString("username", nameInputField.text);
    alterUserName.SetActive(false);
    UpdateName();
  }
  void UpdateName()//更新用户名
  {
    userNameText.text = PlayerPrefs.GetString("username");
  }
  /// <summary>
  /// 进入冒险模式
  /// </summary>
  public void OnAdventureButtonClick()
  {
    AudioManger.Instance.PlayClip(Config.ButtonOnClick);
    SceneManager.LoadScene(2);
  }
}
