using System;
using System.Collections.Generic;
using UnityEngine;

public class CherryBomb : Plant
{
  public float radius = 0; //球形检测范围的半尺寸
  public int attack = 0;
  List<Zombie> detectedZombie = new List<Zombie>();
  public int ClickCount = 0;
  public float BaackBombTimer = 0;//回退爆炸计时器
  public List<Sprite> images;//图片数组
  //: 图片一共12张,点击一下切换3张,当到最后一张时爆炸
  //: 未点击时每5秒回退到上3张图片
  protected override void OnEnable()
  {
    base.OnEnable();
    ClickCount = 0;
    BaackBombTimer = 0;
  }
  protected override void EnableUpdate()
  {
    if (Input.GetMouseButtonDown(0))
    {
      //将屏幕上的鼠标点击位置转换成世界坐标系中的位置
      Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      //发出一条射线，并检测是否与场景中的碰撞器相交
      RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);
      //如果射线碰撞到了某个碰撞器，并且这个碰撞器所属的游戏对象是当前的 CherryBomb 对象
      if (hit.collider != null && hit.collider.gameObject == this.gameObject)
      {
        ClickCount += 1;
        if (ClickCount < 4)
          this.GetComponent<SpriteRenderer>().sprite = images[ClickCount];
      }
    }
    if (ClickCount < 4)
    {
      BaackBombTimer += Time.deltaTime;
      if (BaackBombTimer >= 5)//每5秒回退到上3张图片
      {
        if (ClickCount > 0)
          ClickCount -= 1;

        this.GetComponent<SpriteRenderer>().sprite = images[ClickCount];
        BaackBombTimer = 0;
      }
    }
    if (ClickCount == 4)
      CheckNearbyZombies(Die).Attack();//先检测,检测完毕后再销毁自己生成爆炸特效,最后再攻击僵尸
  }
  List<Cell> cells = new List<Cell>();
  //检测附近的僵尸
  CherryBomb CheckNearbyZombies(Action callBack)
  {
    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
    foreach (Collider2D collider in colliders)
    {
      if (collider.CompareTag("Zombie"))
      {
        Zombie zombie = collider.GetComponent<Zombie>();
        if (zombie != null)
        {
          detectedZombie.Add(zombie);
        }
      }
      if (collider.CompareTag("Cell"))//解冻Cell
      {
        cells.Add(collider.GetComponent<Cell>());
      }
    }
    callBack?.Invoke();
    return this;
  }
  void Attack()//攻击
  {
    //爆炸特效
    foreach (Zombie zombie in detectedZombie)
    {
      zombie.TakeDamage(attack);
    }
    foreach (Cell cell in cells)//解冻Cell
    {
      cell.EnableCell();
    }
    detectedZombie.Clear();
  }

  public override void Die()
  {
    base.Die();
    GameObject effect = BufferPoolManager.Instance.GetObj(BulletHitManger.Instance.CherryBoom);
    effect.transform.position = transform.position;
    BulletHitManger.Instance.PushEffect(BulletHitManger.Instance.CherryBoom, effect.gameObject, 0.5f);
    BufferPoolManager.Instance.PushObj(PlantManger.Instance.plantType[(int)PlantTypes.CerryBomb], this.gameObject);
  }

#if TEXTING
  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, radius);
  }
#endif
}