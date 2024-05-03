using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 选择卡牌栏UI
/// </summary>
public class SelectCardBarUI : MonoBehaviour
{
  public GameObject Bg;
  public GameObject StartGameButton;
  public GameObject CardPrefab;
  int cardNum = 0;
  public void InitiCardBarUI()
  {
    cardNum = UIManger.Instance.cardListUI.cardList.Count;
    for (int i = 0; i < cardNum; i++)//根据已有卡牌数量生成
    {
      GameObject card = Instantiate(CardPrefab);
      card.transform.SetParent(Bg.transform, false);
      card.name = "Card" + i.ToString();
    }
    InitCards();
  }
  int i = 0;
  public void InitCards()
  {
    //生成卡片,一一对应,
    foreach (Card card in UIManger.Instance.cardListUI.cardList)//遍历已有卡牌
    {
      Transform cardParent = Bg.transform.Find("Card" + i.ToString());//找到对应卡牌的父物体
      GameObject cardObj = Instantiate(card.gameObject) as GameObject;
      cardObj.transform.SetParent(cardParent, false);//设置卡片的父物体,确保相对位置不变
      cardObj.transform.localPosition = Vector3.zero;//设置卡片的位置为原点位置
      cardObj.name = card.CardId.ToString();//设置卡片的名字
      cardObj.GetComponent<Card>().DisableCard();//初始化卡片UI
      cardObj.transform.Find("cardGray").gameObject.SetActive(false);//设置卡片灰色蒙版的激活状态
      cardObj.transform.Find("cardLight").gameObject.SetActive(true);//设置卡片选中蒙版的激活状态
      cardObj.transform.Find("cardLight").GetComponent<Button>().enabled = false;//禁用卡牌按钮防止冲突
      i++;
    }
    i = 0;
  }
  public void Show()
  {
    UIManger.Instance.cardListUI.ShowCardList();//显示卡牌列表
    GetComponent<RectTransform>().DOLocalMoveY(-59, 1);
  }
  public void Hide()
  {
    GetComponent<RectTransform>().DOLocalMoveY(-1626, 1).OnComplete(UIManger.Instance.ShowPrepareUI);
    UIManger.Instance.HideCardIntroduce();//隐藏卡牌介绍UI
  }
  public void HightLightCard(int index)//高亮卡牌
  {
    Transform card = Bg.transform.Find("Card" + index.ToString()).Find(index.ToString());
    card.GetComponent<Card>().cardmask.enabled = false;
  }
  public void UnHightLightCard(int index)//取消高亮卡牌
  {
    Transform card = Bg.transform.Find("Card" + index.ToString()).Find(index.ToString());
    card.GetComponent<Card>().cardmask.enabled = true;
  }
  public bool gameStart = false;
  public void StartGame()//$开始游戏
  {
    gameStart = true;

    Hide();//隐藏选择卡牌栏UI,回调显示准备UI,然后再回调初始化函数
  }
}
