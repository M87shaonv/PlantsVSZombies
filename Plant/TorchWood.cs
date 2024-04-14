using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchWood : Plant
{
  protected override void OnEnable()
  {
    base.OnEnable();
  }
  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.GetComponent<PeaBullet>() != null || other.GetComponent<SnowPeaBullet>() != null)//!我真tm服了,这里两行代码就能解决,我偏偏多写了,导致各种问题,铭记,代码还是越简洁越好
    {
      if (other.GetComponent<PeaBullet>().isfire) return;//表示是火焰树桩创建的子弹,就不做任何操作

      BufferPoolManager.Instance.PushObj(BulletManger.Instance.PeaBullet, other.gameObject);

      FirePeaBullet firePeaBullet = BufferPoolManager.Instance.GetObj(BulletManger.Instance.FirePeaBullet).GetComponent<FirePeaBullet>();
      firePeaBullet.transform.position = other.transform.position;
      firePeaBullet.GetComponent<PeaBullet>().isfire = true;//表示是火焰树桩创建的子弹
    }
  }
  public override void Die()
  {
    base.Die();
    AlterHP = HP;//死亡时恢复生命值
    BufferPoolManager.Instance.PushObj(PlantManger.Instance.plantType[(int)PlantType.TorchWood], this.gameObject);
  }
}
