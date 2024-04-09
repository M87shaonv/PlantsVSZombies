using UnityEngine;
using UnityEngine.EventSystems;

public class Shovel : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
  private Transform shovel;
  private bool isUseShovel = false;
  public LayerMask plantLayer;
  public bool IsUseShovel
  {
    get { return isUseShovel; }
    set { isUseShovel = value; }
  }

  public void OnPointerClick(PointerEventData eventData)//鼠标点击事件
  {
    isUseShovel = true;
    Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero, Mathf.Infinity, plantLayer);
    if (hit.collider != null && hit.collider.CompareTag("Plant"))
    {
      Plant plant = hit.collider.GetComponent<Plant>(); // 假设植物的脚本为Plant
      if (plant != null)
      {
        // 进行相关铲除植物的逻辑
        plant.Die();
      }
    }
    if (hit.collider != null && hit.collider.CompareTag("Squash"))
    {
      Plant plant = hit.collider.GetComponent<Plant>();
      if (plant != null)
      {
        plant.Die();
      }
    }
  }

  public void OnPointerEnter(PointerEventData eventData)//鼠标进入事件
  {
    shovel.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);//鼠标进入铲子时，铲子变大
  }

  public void OnPointerExit(PointerEventData eventData)//鼠标退出事件
  {
    shovel.transform.localScale = new Vector3(1f, 1f, 1f);//鼠标退出铲子时，铲子变小
  }

  void Awake()
  {
    shovel = transform.Find("Shovel");//用于在游戏对象的子对象中查找特定名称的子对象的方法
    plantLayer = LayerMask.GetMask("Plant");
  }

  void Update()
  {
    if (IsUseShovel)//使用铲子
    {
      shovel.transform.position = Input.mousePosition;//将铲子的位置设置为鼠标的位置
      shovel.localRotation = Quaternion.Euler(0, 0, 45);//铲子的旋转角度
      // if (Input.GetMouseButtonDown(0))//铲取植物
      // {

      // }
      if (Input.GetMouseButtonDown(1))//放回铲子
      {
        IsUseShovel = false;
        shovel.localRotation = Quaternion.Euler(0, 0, 0);
        shovel.transform.position = transform.position;//将铲子的位置设置为铲子的初始位置
      }
    }
  }

}

