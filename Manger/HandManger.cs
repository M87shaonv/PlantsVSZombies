using System;
using System.Collections.Generic;
using UnityEngine;

public class HandManger : MonoBehaviour
{
  public static HandManger Instance { get; private set; }
  void Awake()
  {
    Instance = this;
  }

  void Update()
  {
    FollowCursor();
  }
  /// <summary>
  /// 玩家手中的植物预制体
  /// </summary>
  public List<Plant> plantPerfabList;
  /// <summary>
  /// 当前需要种植的植物
  /// </summary>
  public Plant currentPlant;

  /// <summary>
  /// 添加植物到玩家的手中,获取植物类型来选择添加哪种植物
  /// </summary>
  public bool AddPlant(PlantType plantType)
  {
    if (currentPlant != null) return false;//如果当前有植物在手上，则不进行添加

    currentPlant = BufferPoolManager.Instance.GetObj(PlantManger.Instance.plantType[(int)plantType]).GetComponent<Plant>();//获取缓冲池中的植物
    return true;
  }

  public void RemovePlant()
  {
    if (currentPlant != null)
      //Destroy(currentPlant.gameObject);
      BufferPoolManager.Instance.PushObj(PlantManger.Instance.plantType[(int)currentPlant.plantType], currentPlant.gameObject);//回收到缓冲池中
    currentPlant = null;//将当前植物置空
  }
  Plant tipPlant;
  /// <summary>
  /// 预制体实例跟随鼠标移动
  /// </summary>
  void FollowCursor()
  {

    if (currentPlant == null) return;
    Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);//将屏幕坐标转换为世界坐标
    mouseWorldPosition.z = 0;//! 不设置z轴会和camera保持一致
    currentPlant.transform.position = mouseWorldPosition;//设置当前植物的位置为鼠标的位置
    // RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);//射线检测是否有障碍物

    // if (hit.collider != null && hit.collider.CompareTag("Cell"))
    // {
    //   var cell = hit.collider.GetComponent<Cell>();

    //   if (tipPlant == null)
    //   {
    //     tipPlant = BufferPoolManager.Instance.GetObj(PlantManger.Instance.plantType[(int)currentPlant.plantType]).GetComponent<Plant>();
    //     //tipPlant.transform.position = hit.collider.transform.position;//设置提示植物的位置为射线碰撞点
    //     tipPlant.transform.position = cell.transform.position;
    //     tipPlant.GetComponent<SpriteRenderer>().color = new Color32(180, 180, 180, 220);
    //   }

    // }
    // else
    // {
    //   if (tipPlant != null)
    //   {
    //     BufferPoolManager.Instance.PushObj(PlantManger.Instance.plantType[(int)currentPlant.plantType], tipPlant.gameObject);//回收到缓冲池中
    //     tipPlant = null;//将当前植物置空}
    //   }
    // }
  }
  /// <summary>
  /// $多个Card实例,根据植物类型来选择不同的Card实例,负责扣除对应阳光和转换为冷却状态
  /// </summary>
  public List<Card> cardInstances;
  public void OncCellClick(Cell cell)
  {
    if (currentPlant == null) return;//手上没有植物则不进行操作
    bool isSuccess = cell.AddPlant(currentPlant, currentPlant.offsetX, currentPlant.offsetY);//将当前植物添加到cell中
    if (isSuccess)
    {
      SunManger.Insance.SubtractSunPoint(cardInstances.Find(Card => Card.plantType == currentPlant.plantType).needsumpoint);//扣除阳光
      cardInstances.Find(Card => Card.plantType == currentPlant.plantType).TransToCooling();//转换为冷却状态
      AudioManger.Instance.PlayClip(Config.PlantGrowers);//播放植物种植音效
      currentPlant = null;//将当前植物置空
      if (tipPlant != null)
        Destroy(tipPlant.gameObject);//回收到缓冲池中
      tipPlant = null;
    }
  }
}


