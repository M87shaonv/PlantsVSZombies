using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManger : MonoBehaviour
{
  public static GameManger Instance { get; private set; }
  public static bool gameStarted = false;// 游戏是否开始
  public GameObject CrazyDave;//戴夫
  private void Awake()
  {
    Instance = this;
  }
  public void GameInit()
  {
    GameStart();
    // 移动完成后暂停1秒返回
    StartCoroutine(WaitForSeconds(ReturnToOrigin, 2));
  }
  /// <summary>
  /// 游戏开始时的相机移动和初始化
  /// </summary>
  void GameStart()
  {
    // 向右移动相机
    DOTween.To(() => UIManger.Instance.cameraTransform.position, x => UIManger.Instance.cameraTransform.position = x,
        UIManger.Instance.cameraTransform.position + Vector3.right * 7.5f, 1).SetEase(Ease.Linear);
  }
  void ReturnToOrigin()
  {
    // 返回原点
    DOTween.To(() => UIManger.Instance.cameraTransform.position, x => UIManger.Instance.cameraTransform.position = x,
        UIManger.Instance.cameraTransform.position + Vector3.left * 7.5f, 1).SetEase(Ease.Linear)
        .OnComplete(UIManger.Instance.selectCardBarUI.Show);// 显示选择卡牌栏
    //.OnComplete(UIManger.Instance.ShowPrepareUI);
  }

  public bool isGameOverend = false;//使用标志位防止游戏多次失败
  /// <summary>
  /// 游戏失败
  /// </summary>
  public void GameOverFail()
  {
    if (isGameOverend) return;
    isGameOverend = true;
    UIManger.Instance.Main.GetComponent<Animator>().enabled = true;// 游戏失败的摄像机移动
    StartCoroutine(WaitForSeconds(UIManger.Instance.failUI.Show, 3));//3秒后显示失败UI
    AudioManger.Instance.PlayClip(Config.loseSound);// 播放失败音乐
    StartCoroutine(WaitForSeconds(Pause, 5));//5秒后暂停僵尸移动和植物攻击
    UIManger.Instance.cardListUI.HideCardList();
    SunManger.Insance.StopProduceSun();// 停止阳光的生成
    StartCoroutine(WaitForSeconds(UIManger.Instance.FailShow, 8));//3秒后显示失败UI
  }
  public void GameOverSuccess()
  {
    if (isGameOverend) return;//无论游戏失败还是胜利,都只执行一次
    isGameOverend = true;
    UIManger.Instance.successUI.Show();
    AudioManger.Instance.PlayClip(Config.winSound);
    SunManger.Insance.StopProduceSun();
    StartCoroutine(WaitForSeconds(UIManger.Instance.SuccessShow, 3));
  }
  /// <summary>
  /// 继续下一关
  /// </summary>
  public void nextLevel()
  {
    PlayerPrefs.SetInt("Level", ZombieManger.Instance.currentLevel + 1);//更新关卡数
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
  }
  /// <summary>
  /// 等待一段时间后执行方法
  /// </summary>
  /// <param name="method">执行的方法</param>
  /// <param name="delay">等待时间</param>
  public IEnumerator WaitForSeconds(Action method, float delay)
  {
    yield return new WaitForSeconds(delay);
    method();
  }
  /// <summary>
  /// 暂停
  /// </summary>
  public void Pause()
  {
    foreach (Zombie zombie in ZombieManger.Instance.zombies)
    {
      zombie.TransToPause();
    }
    foreach (KeyValuePair<int, List<Plant>> pair in ZombieEvent.Instance.plantRows)
    {
      List<Plant> plants = pair.Value;//获取对应的值列表

      foreach (Plant plant in plants)
      {
        plant.TransToPause();
      }
    }
  }
}
