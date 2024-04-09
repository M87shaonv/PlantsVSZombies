using System.Collections;
using UnityEngine;

public class CabbagePitcher : PeaShooter
{
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
      StartCoroutine(WaitSecondsShoot(OffestShoot, Shoot));//OffestShoot:0.4
      StartCoroutine(ChangeShootAnimation(OffestAnim, anim));//OffestAnim:1.35
      shootTimer = 0;
    }
  }

  protected override void Shoot()
  {
    CabbageBullet pb = BufferPoolManager.Instance.GetObj(BulletManger.Instance.CabbageBullet).GetComponent<CabbageBullet>();
    pb.transform.position = firePoint.position;
    //:通过方法可以不使用单例或消息系统即可获得其他脚本的变量
    pb.SetRowNumber(row);//设置子弹所属行号
    pb.SetSpeed(bulletSpeed);
    pb.SetAttack(attack);
  }
  public override void Die()
  {
    base.Die();
    shootTimer = 0;//重置射击计时器
    AlterHP = HP;//死亡时恢复生命值
    StopAllCoroutines();
    BufferPoolManager.Instance.PushObj(PlantManger.Instance.plantType[7], this.gameObject);
  }
}
