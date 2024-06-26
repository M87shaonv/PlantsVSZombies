using System.Collections;
using UnityEngine;

public class Cell : MonoBehaviour
{
  public static Cell Instance;
  public Plant currentPlant;//当前cell种植的植物
  public int Row;//当前cell的行号
  bool CanGrow = true;//当前cell是否可以生长植物
  public GameObject Ice;//冰车僵尸走过显示的冰层
  void Awake()
  {
    Instance = this;
  }
  /// <summary>
  /// 点击cell时种植植物
  /// </summary>
  private void OnMouseDown()
  {
    if (CanGrow)
      HandManger.Instance.OncCellClick(this);
  }
  public bool AddPlant(Plant plant, float offsetX = 0, float offsetY = 0)
  {
    if (currentPlant != null)
    {
      return false;//当前位置已有植物
    }
    SetTipPlant();//置空提示植物
    currentPlant = plant;
    currentPlant.transform.position = transform.position + new Vector3(offsetX, offsetY, 0);
    currentPlant.row = Row;//设置植物的行号
    currentPlant.thisCell = this;
    plant.TransToEnable();
    return true;
  }
  Plant tipPlant;
  void SetTipPlant()
  {
    if (tipPlant != null)
    {
      tipPlant.GetComponent<SpriteRenderer>().color = new Color32(220, 220, 220, 250);
      tipPlant.GetComponent<SpriteRenderer>().sortingOrder = 0;
      BufferPoolManager.Instance.PushObj(PlantManger.Instance.plantType[(int)HandManger.Instance.currentPlant.plantType], tipPlant.gameObject);//回收到缓冲池中
      tipPlant = null;//将当前植物置空
    }
  }
  void OnMouseEnter()
  {
    if (HandManger.Instance.currentPlant != null)
    {
      tipPlant = BufferPoolManager.Instance.GetObj(PlantManger.Instance.plantType[(int)HandManger.Instance.currentPlant.plantType]).GetComponent<Plant>();
      tipPlant.transform.position = transform.position;
      //! 为什么这里改变颜色后再重新赋值,颜色还是没有改变
      //tipPlant.GetComponent<SpriteRenderer>().color = new Color32(180, 180, 180, 220);
      tipPlant.GetComponent<SpriteRenderer>().sortingOrder = -2;
    }
  }

  void OnMouseExit()
  {
    if (HandManger.Instance.currentPlant != null)
    {
      tipPlant.GetComponent<SpriteRenderer>().sortingOrder = 0;
      BufferPoolManager.Instance.PushObj(PlantManger.Instance.plantType[(int)HandManger.Instance.currentPlant.plantType], tipPlant.gameObject);//回收到缓冲池中
      tipPlant = null;//将当前植物置空
    }
  }
  public void RemovePlant()
  {
    if (currentPlant != null)
    {
      BufferPoolManager.Instance.PushObj(PlantManger.Instance.plantType[(int)currentPlant.plantType], currentPlant.gameObject);//回收到缓冲池中
      currentPlant = null;
    }
  }
  public void DisableCell()
  {
    CanGrow = false;
    StartCoroutine(IEEnableCell(60));
  }
  public void EnableCell()
  {
    CanGrow = true;
    this.GetComponent<SpriteRenderer>().sprite = null;
  }
  IEnumerator IEEnableCell(float delay)
  {
    yield return new WaitForSeconds(delay);
    EnableCell();
  }
}
