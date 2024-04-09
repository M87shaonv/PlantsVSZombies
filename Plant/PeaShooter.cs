using System;
using System.Collections;
using UnityEngine;

public class PeaShooter : Plant
{
  public float firingInterval = 2;//射击间隔
  public float shootTimer = 0;//射击计时器
  public Transform firePoint;//射击点
  public float bulletSpeed = 5;//子弹速度
  public int attack = 20;//子弹攻击力
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
    AlterHP = HP;
    TransToDisable();//默认禁用状态
    shootTimer = 0;
  }
  protected override void EnableUpdate()
  {
    if (ZombieEvent.Instance.zombieRows[row].Count == 1) return;//@如果该行没有僵尸，则不再射击
    shootTimer += Time.deltaTime;
    if (shootTimer >= firingInterval)
    {
      anim.SetBool("isShoot", true);
      StartCoroutine(WaitSecondsShoot(OffestShoot, Shoot));//OffestShoot:1
      StartCoroutine(WaitSecondsShoot(OffestShoot + 0.2f, Shoot));
      StartCoroutine(ChangeShootAnimation(OffestAnim, anim));//OffestAnim:1.85
      shootTimer = 0;
    }
  }

  /// <summary>
  /// 发射子弹
  /// </summary>
  protected virtual void Shoot()
  {
    PeaBullet pb = BufferPoolManager.Instance.GetObj(BulletManger.Instance.PeaBullet).GetComponent<PeaBullet>();
    pb.transform.position = firePoint.position;
    pb.SetSpeed(bulletSpeed);
    pb.SetAttack(attack);
  }
  public override void Die()
  {
    base.Die();
    shootTimer = 0;//重置射击计时器
    AlterHP = HP;//死亡时恢复生命值
    StopAllCoroutines();
    BufferPoolManager.Instance.PushObj(PlantManger.Instance.plantType[2], this.gameObject);
  }
  /// <summary>
  /// 等待指定时间后执行指定动作
  /// </summary>
  public IEnumerator WaitSecondsShoot(float time, Action action)
  {
    yield return new WaitForSeconds(time);
    action();
  }
  /// <summary>
  /// 改变射击动画
  /// </summary>
  public IEnumerator ChangeShootAnimation(float time, Animator anim)
  {
    yield return new WaitForSeconds(time);
    anim.SetBool("isShoot", false);
  }
}

