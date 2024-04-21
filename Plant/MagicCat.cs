using UnityEngine;

public class MagicCat : PeaShooter
{
  protected override void OnEnable()
  {
    base.OnEnable();
    shootTimer = 0;
  }

  protected override void EnableUpdate()
  {
    if (ZombieEvent.Instance.SkyZombies.Count != 0)
    {
      shootTimer += Time.deltaTime;
      if (shootTimer >= firingInterval)
      {
        DetectionEnemy();
        shootTimer = 0;
      }
    }
    if (ZombieManger.Instance.zombies.Count == 0) return;//场上无僵尸
    shootTimer += Time.deltaTime;
    if (shootTimer >= firingInterval)
    {
      DetectionEnemy();
      shootTimer = 0;
    }
  }
  void DetectionEnemy()
  {
    anim.SetBool("isShoot", true);
    StartCoroutine(WaitSecondsShoot(OffestShoot, Shoot));//OffestShoot:0.4
    StartCoroutine(ChangeShootAnimation(OffestAnim, anim));//OffestAnim:1.35
  }
  protected override void Shoot()
  {
    MagicCatBullet pb = BufferPoolManager.Instance.GetObj(BulletManger.Instance.MagicCatBullet).GetComponent<MagicCatBullet>();
    pb.transform.position = firePoint.position;
    pb.SetSpeed(bulletSpeed);
    pb.SetAttack(attack);
  }
  public override void Die()
  {
    base.Die();
    AlterHP = HP;
    StopAllCoroutines();
    BufferPoolManager.Instance.PushObj(PlantManger.Instance.plantType[(int)PlantTypes.MagicCat], this.gameObject);
  }
}
