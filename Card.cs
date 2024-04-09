using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
# region 卡牌状态和植物类型的枚举
enum CardState
{
  /// <summary>
  /// 禁用状态
  /// </summary>
  Disable,
  /// <summary>
  /// 冷却中状态
  /// </summary>
  Cooling,
  /// <summary>
  /// 正常状态
  /// </summary>
  Ready,
  /// <summary>
  /// 等待状态
  /// </summary>
  Waiting,
}
/// <summary>
/// 植物类型设置为pubic是为了在编辑器中修改
/// </summary>
public enum PlantType
{
  /// <summary>
  /// 向日葵
  /// </summary>
  Sunflower,
  /// <summary>
  /// 单发豌豆射手
  /// </summary>
  PeashooterOne,
  /// <summary>
  /// 双发豌豆射手
  /// </summary>
  Peashooter,
  /// <summary>
  /// 矮土豆
  /// </summary>
  WallNut,
  /// <summary>
  /// 火焰树桩
  /// </summary>
  TorchWood,
  /// <summary>
  /// 窝瓜
  /// </summary>
  Squash,
  /// <summary>
  /// 寒冰豌豆射手
  /// </summary>
  SnowPeaShooter,
  /// <summary>
  /// 机枪豌豆射手
  /// </summary>
  MachineGunPeaShooter,
  /// <summary>
  /// 卷心菜投手
  /// </summary>
  CabbagePitcher,
}
#endregion
public class Card : MonoBehaviour
{
  private CardState cardState = CardState.Disable;//默认禁用
  public PlantType plantType;
  public GameObject cardlight;
  public GameObject cardgray;
  public Image cardmask;
  /// <summary>
  /// 总冷却时间
  /// </summary>
  [SerializeField]
  private float coolingtime = 0f;
  /// <summary>
  /// 冷却计时时间
  /// </summary>
  [SerializeField]
  private float coolingtimer = 0;
  /// <summary>
  /// 需要阳光数量
  /// </summary>
  [SerializeField]
  public int needsumpoint = 50;

  private void FixedUpdate()
  {
    switch (cardState)
    {
      case CardState.Cooling:
        CoolingUpdate(coolingtime);
        break;
      case CardState.Ready:
        ReadyUpdate();
        break;
      case CardState.Waiting:
        WaitingUpdate();
        break;
      default:
        break;
    }

  }
  /// <summary>
  /// 冷却状态,计算卡牌冷却时间
  /// </summary>
  void CoolingUpdate(float coolingtime)
  {
    coolingtimer += Time.deltaTime;
    cardmask.fillAmount = (coolingtime - coolingtimer) / coolingtime;//计算剩余时间比例
    if (coolingtimer >= coolingtime)
    {
      TransToWaiting();
      coolingtimer = 0;
    }
  }

  /// <summary>
  /// 等待阳光状态,阳光不足时触发
  /// </summary>
  void WaitingUpdate()
  {
    if (needsumpoint <= SunManger.Insance.SunPoint)
    {
      TransToReady();
    }
  }
  /// <summary>
  /// 正常状态,阳光足够时触发
  /// </summary>
  void ReadyUpdate()
  {
    if (needsumpoint > SunManger.Insance.SunPoint)//阳光不足,转换为等待阳光状态
    {
      TransToWaiting();
    }
  }
  /// <summary>
  /// 转换为等待阳光状态
  /// </summary>
  void TransToWaiting()
  {
    cardState = CardState.Waiting;
    cardlight.SetActive(false);
    cardgray.SetActive(true);
    cardmask.gameObject.SetActive(false);
  }
  /// <summary>
  /// 转换为正常状态
  /// </summary>
  void TransToReady()
  {
    cardState = CardState.Ready;
    cardlight.SetActive(true);
    cardgray.SetActive(false);
    cardmask.gameObject.SetActive(false);
  }
  /// <summary>
  /// 转换为冷却状态
  /// </summary>
  public void TransToCooling()
  {
    cardState = CardState.Cooling;
    coolingtimer = 0;
    cardlight.SetActive(false);
    cardgray.SetActive(true);
    cardmask.gameObject.SetActive(true);
  }

  /// <summary>
  /// 点击事件,阳光足够时触发
  /// </summary> 
  public void Onclick()
  {
    if (needsumpoint > SunManger.Insance.SunPoint) return;//如果阳光不足,则不执行
    if (HandManger.Instance.currentPlant == null)//没有植物在手上,添加植物到手上并return
    {
      HandManger.Instance.AddPlant(plantType);
      return;
    }
    if (HandManger.Instance.currentPlant != null)//如果当前有植物
    {
      if (HandManger.Instance.currentPlant.plantType == this.plantType)//如果是相同植物,则置空鼠标的选择并return
      {
        HandManger.Instance.RemovePlant();
        return;
      }
      //? 这里销毁不知道为什么会在原地留下一个对象
      //Destroy(HandManger.Instance.currentPlant);
      HandManger.Instance.RemovePlant();//点击其他植物,先销毁原有植物,再添加新植物到手上
      HandManger.Instance.AddPlant(plantType);
    }

    AudioManger.Instance.PlayClip(Config.ButtonOnClickTap);//播放点击音效
  }

  //用于外部使用的方法
  public void DisableCard()
  {
    cardState = CardState.Disable;
  }
  public void EnableCard()
  {
    TransToCooling();
  }
}
