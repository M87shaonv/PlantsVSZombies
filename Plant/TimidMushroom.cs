using UnityEngine;

public class TimidMushroom : PeaShooter
{
  protected override void EnableUpdate()
  {
    if (ZombieEvent.Instance.zombieRows[row].Count == 1) return;//@如果该行没有僵尸，则不再射击
    shootTimer += Time.deltaTime;
    if (shootTimer >= firingInterval)
    {
      anim.SetBool("isShoot", true);
      StartCoroutine(WaitSecondsShoot(OffestShoot, Shoot));//OffestShoot:1
      StartCoroutine(ChangeShootAnimation(OffestAnim, anim));//OffestAnim:1.85
      shootTimer = 0;
    }
  }
  protected override void Shoot()
  {
    TimidMushroomBullet pb = BufferPoolManager.Instance.GetObj(BulletManger.Instance.TimidMushroomBullet).GetComponent<TimidMushroomBullet>();
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
    BufferPoolManager.Instance.PushObj(PlantManger.Instance.plantType[(int)PlantTypes.TimidMushroom], this.gameObject);
  }
}