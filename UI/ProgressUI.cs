using UnityEngine.UI;
using UnityEngine;

public class ProgressUI : MonoBehaviour
{
  GameObject bg;
  GameObject progress;
  GameObject flag;
  GameObject head;
  GameObject leveltext;

  void Awake()
  {
    bg = transform.Find("bg").gameObject;
    progress = transform.Find("progress").gameObject;
    flag = transform.Find("flag").gameObject;
    head = transform.Find("head").gameObject;
    leveltext = transform.Find("leveltext").gameObject;
    leveltext.GetComponent<Text>().text = $"关卡{PlayerPrefs.GetInt("Level")}";
  }

  /// <summary>
  /// 设置进度条进度百分比
  /// </summary>
  /// <param name="percent">进度百分比<,1是完全填充/param>
  public void SetProgress(float percent)
  {
    progress.GetComponent<Image>().fillAmount = percent;
    //获取进度条中心点的x坐标
    float originPosX = bg.GetComponent<RectTransform>().position.x + bg.GetComponent<RectTransform>().sizeDelta.x / 2;
    float width = bg.GetComponent<RectTransform>().sizeDelta.x;//获取进度条背景组件宽度
    head.GetComponent<RectTransform>().position = new Vector2(originPosX - percent * width, head.GetComponent<RectTransform>().position.y);
    if (percent == 1)//进度条完全填充,禁用脑子旗帜
    {
      flag.SetActive(false);
    }
  }
  public void Show()
  {
    this.gameObject.SetActive(true);
  }
}
