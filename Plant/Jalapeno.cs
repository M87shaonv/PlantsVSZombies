using System.Collections.Generic;
using UnityEngine;

public class Jalapeno : Plant
{
  public Vector2 boxSize = new Vector2(1, 1); // 长方形的大小
  List<Zombie> nearbyZombies = new List<Zombie>();
  public Transform[] FireTransform = new Transform[2];
  public Sprite initImgae;
  public int damage = 100;
  bool isCheck = false;
  Vector3 pos;
  protected override void OnEnable()
  {
    base.OnEnable();
    isCheck = false;
    GetComponent<SpriteRenderer>().sprite = initImgae;
  }
  protected override void EnableUpdate()
  {
    pos = transform.position;
    pos.y -= 0.3f;
    if (!isCheck)
      CheckNearbyZombies();
  }
  void CheckNearbyZombies()
  {
    Collider2D[] colliders = Physics2D.OverlapBoxAll(pos, boxSize, 0f); // 执行长方形范围检测
    foreach (Collider2D collider in colliders)
    {
      if (collider.CompareTag("Zombie"))
      {
        Zombie zombie = collider.GetComponent<Zombie>();
        nearbyZombies.Add(zombie); // 添加到附近的僵尸列表
      }
    }
    isCheck = true;
  }

  //:在帧事件中调用
  public override void Die()
  {
    foreach (Zombie zombie in nearbyZombies)
    {
      zombie.TakeDamage(damage); // 造成伤害
    }
    nearbyZombies.Clear();
    base.Die();
    //TODO 爆炸特效
    for (int i = 0; i < FireTransform.Length; i++)
    {
      GameObject Fire = BufferPoolManager.Instance.GetObj(BulletHitManger.Instance.JalapenoBlast);
      var pos = FireTransform[i].position;
      pos.x = 6.78f;
      Fire.transform.position = pos;
      BulletHitManger.Instance.PushEffect(BulletHitManger.Instance.JalapenoBlast, Fire, 0.7f);
    }
    BufferPoolManager.Instance.PushObj(PlantManger.Instance.plantType[(int)PlantTypes.Jajapeno], this.gameObject);
  }
#if TEXTING
  void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(pos, boxSize); // 在Scene视图中绘制长方形范围
  }
#endif
}
