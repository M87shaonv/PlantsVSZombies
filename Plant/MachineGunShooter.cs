using System.Collections;
using UnityEngine;

public class MachineGunShooter : PeaShooter
{
  public GameObject bigPeaBullet;

  protected override void OnEnable()
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
      //! 使用Invoke会导致无法正常回收,不要混用Invoke和StartCoroutine
      anim.SetBool("isShoot", true);
      StartCoroutine(WaitSecondsShoot(OffestShoot, Shoot));
      StartCoroutine(WaitSecondsShoot(OffestShoot + 0.2f, Shoot));
      StartCoroutine(WaitSecondsShoot(OffestShoot + 0.4f, Shoot));
      StartCoroutine(WaitSecondsShoot(OffestShoot + 0.6f, Shoot));
      StartCoroutine(WaitSecondsShoot(OffestShoot + 0.8f, Shoot));
      StartCoroutine(ChangeShootAnimation(OffestAnim, anim));
      shootTimer = 0;
    }
  }

  /// <summary>
  /// 机枪豌豆射手有1/40的概率发射攻击力*5和速度*2的大子弹
  /// </summary>
  protected override void Shoot()
  {
    int luck = Random.Range(0, 41);//TODO 应该设为散射,如果能实现的话
    if (luck == 17)
    {
      GameObject big = BufferPoolManager.Instance.GetObj(BulletManger.Instance.BigPeaBullet);
      big.transform.position = firePoint.position;
      big.GetComponent<PeaBullet>().SetSpeed(bulletSpeed * 2);
      big.GetComponent<PeaBullet>().SetAttack(attack * 5);

      StartCoroutine(BufferPoolManager.Instance.WaitAndPush(BulletManger.Instance.BigPeaBullet, big, 6));
      return;
    }
    GameObject pb = BufferPoolManager.Instance.GetObj(BulletManger.Instance.PeaBullet);
    pb.transform.position = firePoint.position;
    pb.GetComponent<PeaBullet>().SetSpeed(bulletSpeed);
    pb.GetComponent<PeaBullet>().SetAttack(attack);

    StartCoroutine(BufferPoolManager.Instance.WaitAndPush(BulletManger.Instance.PeaBullet, pb, 6));
  }
  public override void Die()
  {
    base.Die();
    StopAllCoroutines();
    BufferPoolManager.Instance.PushObj(PlantManger.Instance.plantType[7], this.gameObject);
  }
}
