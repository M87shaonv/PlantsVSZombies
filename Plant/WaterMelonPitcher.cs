using UnityEngine;

public class WaterMelonPitcher : PeaShooter
{
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
    WatermelonBullet pb = BufferPoolManager.Instance.GetObj(BulletManger.Instance.WatermelonBullet).GetComponent<WatermelonBullet>();
    pb.transform.position = firePoint.position;
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
    BufferPoolManager.Instance.PushObj(PlantManger.Instance.plantType[(int)PlantTypes.WatermelonPitcher], this.gameObject);
  }
}