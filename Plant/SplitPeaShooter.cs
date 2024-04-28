using UnityEngine;

public class SplitPeaShooter : PeaShooter
{
  public Transform[] transforms;

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
    for (int i = 0; i < transforms.Length; i++)
    {
      PeaBullet pb = BufferPoolManager.Instance.GetObj(BulletManger.Instance.PeaBullet).GetComponent<PeaBullet>();
      pb.transform.position = transforms[i].position;
      pb.SetSpeed(bulletSpeed);
      pb.SetAttack(attack);

      if (i == 0)
        pb.SetSpeed(-bulletSpeed);
    }
  }
  public override void Die()
  {
    base.Die();
    BufferPoolManager.Instance.PushObj(PlantManger.Instance.plantType[(int)PlantTypes.SplitPeaShooter], this.gameObject);
  }
}