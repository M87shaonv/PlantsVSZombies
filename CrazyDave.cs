using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class CrazyDave : MonoBehaviour
{
  Animator anim;
  public GameObject dialog;//对话框
  public Text text;//说话文本
  public JsonDataList jsonDataList;
  public String[] speaks;//说话内容
  public static CrazyDave Instance;
  bool isSpeak = false;
  bool hasAppeared = false;  // 添加一个表示是否已经触发过 Appear 的标志
  int index = 0;//文本索引
  void Awake()
  {
    PlayerPrefs.DeleteKey("OwnedCard");
    PlayerPrefs.SetInt("Gold", 0);
    PlayerPrefs.SetInt("Level", 1);//初始化关卡数
    anim = GetComponent<Animator>();
    jsonDataList = Resources.Load<JsonDataList>("JsonDataList");
    Instance = this;
    isSpeak = false;
    index = 0;
  }
  void OnEnable()
  {
    dialog.SetActive(false);
    ReadSpeak();
    hasAppeared = false;
  }
  public void PlayEnter(int direction)//0从左边进入，1从下面进入
  {
    switch (direction)
    {
      case 0:
        anim.Play("EnterLeft");
        break;
      case 1:
        anim.Play("EnterUnder");
        break;
    }
  }
  public void PlayLeave(int direction)//0从左边离开，1从下面离开
  {
    switch (direction)
    {
      case 0:
        anim.Play("LeaveLeft");
        break;
      case 1:
        anim.Play("LeaveUnder");
        break;
    }
  }
  void PlaySpeak()//:帧事件中调用
  {
    anim.Play("Speak");
    dialog.SetActive(true);
    isSpeak = true;
  }
  void Update()
  {

    if (isSpeak)
    {
      dialog.SetActive(true);

      if (Input.GetMouseButtonDown(0))
      {
        ++index;
      }
      if (index < speaks.Length)
        text.text = speaks[index];
      else
      {
        dialog.SetActive(false);
        int direction = UnityEngine.Random.Range(0, 2);
        PlayLeave(direction);
        isSpeak = false;
        StartCoroutine(GameInit());
      }
    }
  }
  IEnumerator GameInit()
  {
    yield return new WaitForSeconds(1);
    GameManger.Instance.GameInit();
  }
  void PlayCrazy()
  {
    anim.Play("Crazy");
  }

  void ReadSpeak()//读取说话内容
  {
    foreach (var JsonData in jsonDataList.JsonData)
    {
      if (JsonData.LevelID > PlayerPrefs.GetInt("Level"))
      {
        GameManger.Instance.GameInit();
        return;
      }
      if (!hasAppeared && JsonData.LevelID == PlayerPrefs.GetInt("Level"))
      {
        if (JsonData.Showed == 1)
        {
          GameManger.Instance.GameInit();
          return;//已经显示过
        }
        else
        {
          speaks = JsonData.text;
          JsonData.Showed = 1;//标记为已经显示
          Appear();
          hasAppeared = true;
          return;
        }
      }
    }
  }
  public void Appear()
  {
    int direction = UnityEngine.Random.Range(0, 2);
    PlayEnter(direction);
  }
}