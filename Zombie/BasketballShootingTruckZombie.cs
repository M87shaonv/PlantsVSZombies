using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BasketballShootingTruckZombie : Zombie
{
  /*动画状态机Shoot参数
  0:shoot
  1:idle
  2.walk
  HPpercent<0时:blast
  */
  public Transform ShootPoint;
  /// <summary>
  /// 射击动画发射子弹偏移量
  /// </summary>
  public float OffestShoot = 0.5f;
  /// <summary>
  /// 射击动画切换偏移量
  /// </summary>
  public float OffestAnim = 0.5f;
  void OnEnable()
  {
    this.GetComponent<Collider2D>().enabled = true;
    isPush = true;
    zombieState = ZombieState.Move;
    currentHP = HP;
    attackTimer = 0;
    AlterMoveSpeed = moveSpeed;
  }

  protected override void FixedUpdate()
  {
    switch (zombieState)
    {
      case ZombieState.Move:
        MoveUpdate();
        break;
      case ZombieState.Eat:
        ShootBasketballUpdate();
        break;
      case ZombieState.Die:
        break;
    }
  }
  protected override void MoveUpdate()
  {
    anim.SetInteger("Shoot", 2);
    base.MoveUpdate();
    if (ZombieEvent.Instance.plantRows[Row].Count > 1)
    {
      zombieState = ZombieState.Eat;//当前行有植物便可以射击
      anim.SetInteger("Shoot", 1);
    }
  }
  void ShootBasketballUpdate()
  {
    attackTimer += Time.deltaTime;
    if (attackTimer > attackInterval)
    {
      anim.SetInteger("Shoot", 0);
      //ShootBasketball();
      StartCoroutine(WaitSecondsShoot(OffestShoot, ShootBasketball));
      StartCoroutine(ChangeShootAnimation(OffestAnim, anim));
      attackTimer = 0;
    }

    if (ZombieEvent.Instance.plantRows[Row].Count <= 1)
    {
      zombieState = ZombieState.Move;
    }
  }
  public override void TakeDamage(int damage)
  {
    if (currentHP <= 0) return;
    this.currentHP -= damage;
    if (damage != 15)
    {
      GetComponent<SpriteRenderer>().color = Color.white;
      StartCoroutine(ChangeColor());
    }
    if (currentHP <= 0)
    {
      currentHP = -1;
      Dead();
    }

    float hppercent = (float)currentHP / HP;//计算当前生命值百分比
    anim.SetFloat("HPpercent", hppercent);
  }
  void ShootBasketball()//射击
  {
    BasketballBullet pb = BufferPoolManager.Instance.GetObj(BulletManger.Instance.BasketballBullet).GetComponent<BasketballBullet>();
    pb.transform.position = ShootPoint.position;
    pb.SetRowNumber(Row);
  }
  /// <summary>
  /// 等待指定时间后执行指定动作
  /// </summary>
  IEnumerator WaitSecondsShoot(float time, Action action)
  {
    yield return new WaitForSeconds(time);
    action();
  }
  /// <summary>
  /// 改变射击动画
  /// </summary>
  IEnumerator ChangeShootAnimation(float time, Animator anim)
  {
    yield return new WaitForSeconds(time);
    anim.SetInteger("Shoot", 1);
  }
  public override void Dead()//爆炸
  {
    base.Dead();
  }
  
  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Plant"))
    {
      other.GetComponent<Plant>().Die();
    }
  }
}
