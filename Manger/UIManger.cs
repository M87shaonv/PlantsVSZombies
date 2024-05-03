using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManger : MonoBehaviour
{
  public static UIManger Instance { get; private set; }
  public PrepareUI prepareUI;//准备UI
  public CardListUI cardListUI;//卡片列表UI
  public FailUI failUI;//失败UI
  public SuccessUI successUI;//成功UI
  public ProgressUI progressUI;//进度UI
  public SettingUI settingsUI;//设置UI
  public Transform cameraTransform;//主相机位置
  public Camera Main;//主摄像机
  public GameObject shovel;//铲子
  public GameObject bigwave;//大波僵尸
  public GameObject lastwave;//最后一波僵尸
  public GameObject SuccessMenu;//成功菜单
  public GameObject FailMenu;//失败菜单
  public SelectCardBarUI selectCardBarUI;//选择卡牌栏UI
  public GameObject GoldBar;//金币栏
  public GameObject Introduce;//卡牌介绍界面
  private void Awake()
  {
    Instance = this;//? Instance这个单例模式究竟怎么实现的
    cameraTransform = Camera.main.transform;
  }
  /// <summary>
  /// 显示准备UI,然后调用回调函数StartGame
  /// </summary>
  public void ShowPrepareUI()
  {
    AudioManger.Instance.PlayClip(Config.prepare);
    prepareUI.Show(Initialize);//调用回调函数
  }
  /// <summary>
  /// 开始游戏,显示对应UI
  /// </summary>
  void Initialize()
  {
    //Instance.cardListUI.ShowCardList();//显示卡片列表
    GameManger.gameStarted = true;//游戏开始
    InitializeChooseCardsList();//初始化选择卡牌列表
    progressUI.Show();//显示进度UI
    shovel.SetActive(true);//显示铲子
    SunManger.Insance.StartProduceSun();//开始生产阳光
    ZombieManger.Instance.TableCreateZombie();//开始创建僵尸
    BgMusicManger.Instance.StartMusic(Config.BgminGameQucik);//播放游戏中背景音乐
    GoldBar.SetActive(true);
  }
  #region 大波僵尸\最后一波僵尸\成功\失败UI的显示与隐藏

  public void ShowBigWaveUI()
  {
    AudioManger.Instance.PlayClip(Config.bigwave, 0.2f);
    bigwave.SetActive(true);
    bigwave.GetComponent<Animator>().enabled = true;
    StartCoroutine(GameManger.Instance.WaitForSeconds(HideBigWaveUI, 3));

  }
  public void ShowLastWaveUI()
  {
    AudioManger.Instance.PlayClip(Config.lastwave, 0.2f);
    lastwave.SetActive(true);
    lastwave.GetComponent<Animator>().enabled = true;
    StartCoroutine(GameManger.Instance.WaitForSeconds(HideLastWaveUI, 3));
  }
  public void HideBigWaveUI()
  {
    bigwave.SetActive(false);
  }
  public void HideLastWaveUI()
  {
    lastwave.SetActive(false);
  }

  public void SuccessShow()
  {
    SuccessMenu.SetActive(true);
  }
  public void SuccessHide()
  {
    SuccessMenu.SetActive(false);
  }
  public void FailShow()
  {
    FailMenu.SetActive(true);
  }
  public void FailHide()
  {
    FailMenu.SetActive(false);
  }
  #endregion

  void InitializeChooseCardsList()
  {
    HandManger.Instance.cardInstances = cardListUI.ChooseCardsList;//初始化选择卡牌列表
    for (int i = 0; i < cardListUI.ChooseCardsList.Count; i++)
    {
      cardListUI.ChooseCardsList[i].cardlight.GetComponent<Button>().enabled = true;
    }
  }
  public void ShowCardIntroduce(String Text)//显示卡牌介绍
  {
    Introduce.SetActive(true);
    Introduce.transform.Find("text").GetComponent<Text>().text = Text;
  }
  public void HideCardIntroduce()//隐藏卡牌介绍
  {
    Introduce.SetActive(false);
  }
}