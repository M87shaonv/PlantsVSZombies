using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

# region 卡牌状态和植物类型的枚举
public enum CardState
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
/// 植物类型枚举
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
  /// <summary>
  /// 老壁灯
  /// </summary>
  OldWallLamp,
  /// <summary>
  /// 杨桃
  /// </summary>
  Carambola,
  /// <summary>
  /// 魔法猫咪
  /// </summary>
  MagicCat,
  /// <summary>
  /// 樱桃炸弹
  /// </summary>
  CherryBomb,
  /// <summary>
  /// 玉米加农炮
  /// </summary>
  CornCannon,
  /// <summary>
  /// 火爆辣椒
  /// </summary>
  Jajapeno,
  /// <summary>
  /// 幸运花
  /// </summary>
  LuckyFlower,
  /// <summary>
  /// 土豆地雷
  /// </summary>
  PotatoMine,
  /// <summary>
  /// 地刺
  /// </summary>
  Spikeweed,
  /// <summary>
  /// 裂荚豌豆射手
  /// </summary>
  SplitPeaShooter,
  /// <summary>
  /// 孪生向日葵
  /// </summary>
  SunflowerTwin,
  /// <summary>
  /// 高坚果
  /// </summary>
  TallWallNut,
  /// <summary>
  /// 三发豌豆射手
  /// </summary>
  ThreeShotPeashooter,
  /// <summary>
  /// 胆小菇
  /// </summary>
  TimidMushroom,
  /// <summary>
  /// 西瓜投手
  /// </summary>
  WatermelonPitcher,
  /// <summary>
  /// 冰西瓜投手
  /// </summary>
  WinterWatermelonPitcher,
  /// <summary>
  /// 冰冻蘑菇
  /// </summary>
  FrozenMushroom,
  /// <summary>
  /// 模仿者
  /// </summary>
  Imitater,
}
#endregion
public class Card : MonoBehaviour, IPointerClickHandler
{
  public CardState cardState = CardState.Disable;//默认禁用
  public PlantType plantType;
  public GameObject cardlight;
  public GameObject cardgray;
  public Image cardmask;
  public int CardId;

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
  void OnEnable()
  {
    CardId = (int)plantType;
  }
  private void FixedUpdate()
  {
    if (!GameManger.gameStarted) return;
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
  public void TransToWaiting()
  {
    cardState = CardState.Waiting;
    cardlight.SetActive(false);
    cardgray.SetActive(true);
    cardmask.gameObject.SetActive(false);
  }
  /// <summary>
  /// 转换为正常状态
  /// </summary>
  public void TransToReady()
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
  public bool hasUsed = false;//是否已经使用
  public bool hasLock = false;//是否锁定
  public bool isMoving = false;//是否正在移动

  public void OnPointerClick(PointerEventData eventData)
  {
    if (UIManger.Instance.selectCardBarUI.gameStart) return;//游戏已经开始,则不执行
    if (isMoving || hasLock) return;//如果正在移动或锁定,则不执行
    if (hasUsed)//如果已经使用,则移除卡牌
    {
      RemoveCard(gameObject);
    }
    else
    {

      AddCard();
    }
  }
  public void RemoveCard(GameObject UseCard)
  {
    if (UseCard == null)
    {
      Debug.Log("UseCard is null");
      return;
    }
    if (UIManger.Instance.cardListUI.ChooseCardsList.Contains(UseCard.GetComponent<Card>()))
    {
      GameObject card = UIManger.Instance.selectCardBarUI.Bg.transform.Find("Card" + CardId.ToString()).Find(CardId.ToString()).gameObject;
      card.GetComponent<Card>().hasLock = false;
      hasLock = false;//解锁卡牌
      UIManger.Instance.cardListUI.ChooseCardsList.Remove(UseCard.GetComponent<Card>());//从选择卡牌列表移除


      UIManger.Instance.cardListUI.currentIndex--;//当前选择卡牌索引减一
      UIManger.Instance.cardListUI.UpdateCardPosition();//更新卡牌位置
      UseCard.GetComponent<Card>().isMoving = true;
      Transform cardTransform = UIManger.Instance.selectCardBarUI.Bg.transform.Find("Card" + CardId.ToString());
      UseCard.transform.DOMove(cardTransform.position, 0.3f).OnComplete(
        () =>
        {
          UIManger.Instance.selectCardBarUI.UnHightLightCard(CardId);//取消高亮选择卡牌
          UseCard.GetComponent<Card>().isMoving = false;
          Destroy(UseCard);//销毁卡牌
        }
      );
    }
  }

  public void AddCard()
  {
    if (hasLock) return;//如果锁定,则不执行
    if (UIManger.Instance.cardListUI.currentIndex > UIManger.Instance.cardListUI.Count - 1)
    {
      print("卡牌数量已达上限");
      return;
    }

    GameObject useCard = Instantiate(UIManger.Instance.cardListUI.cardList[CardId].gameObject);//克隆卡牌
    useCard.transform.Find("cardLight").GetComponent<Button>().enabled = false;//禁用卡牌按钮防止冲突
    useCard.transform.SetParent(UIManger.Instance.cardListUI.transform);
    useCard.transform.position = transform.position;
    useCard.name = "useCard" + UIManger.Instance.cardListUI.currentIndex.ToString();
    useCard.GetComponent<Card>().TransToReady();
    Transform TragetTransform = UIManger.Instance.cardListUI.ChooseBar.transform.Find("Card" + UIManger.Instance.cardListUI.currentIndex);//找到添加卡牌的对应位置

    UIManger.Instance.cardListUI.ChooseCardsList.Add(useCard.GetComponent<Card>());//添加到选择卡牌列表
    UIManger.Instance.cardListUI.currentIndex++;//当前选择卡牌索引加一
    UIManger.Instance.selectCardBarUI.HightLightCard(CardId);//高亮选择卡牌
    useCard.GetComponent<Card>().hasUsed = true;//正在使用
    useCard.GetComponent<Card>().isMoving = true;//正在移动
    hasLock = true; //锁定卡牌
    useCard.transform.DOMove(TragetTransform.position, 0.3f).OnComplete(
      () =>
      {
        useCard.GetComponent<Card>().cardState = CardState.Ready;//设置卡牌状态为正常
        useCard.GetComponent<Card>().isMoving = false;//动画结束后设置isMoving为false
        useCard.transform.localPosition = Vector3.zero;//重置位置 
        useCard.transform.SetParent(TragetTransform, false);//设置父物体为对应位置
      }
    );
  }
}