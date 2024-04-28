using UnityEngine;

public class ThreeShotPeashooter : PeaShooter
{
  public Transform[] transforms;
  protected override void Shoot()
  {
    for (int i = 0; i < transforms.Length; ++i)
    {
      PeaBullet pb = BufferPoolManager.Instance.GetObj(BulletManger.Instance.PeaBullet).GetComponent<PeaBullet>();
      pb.transform.position = transforms[i].position;
      if (i == 0)
        StartCoroutine(pb.Move(0.5f, new Vector3(transforms[i].position.x + 2.1f, transforms[i].position.y + pb.UpwardDistance, transforms[i].position.z), transforms[i].position));
      else if (i == 2)
        StartCoroutine(pb.Move(0.5f, new Vector3(transforms[i].position.x + 2.1f, transforms[i].position.y + pb.DownwardDistance, transforms[i].position.z), transforms[i].position));

      pb.SetSpeed(bulletSpeed);
      pb.SetAttack(attack);
    }
  }

  protected override void EnableUpdate()
  {
    if (ZombieEvent.Instance.zombieRows[row].Count == 1) return;//@如果三行没有僵尸，则不再射击
    if (row == 0)
    {
      if (ZombieEvent.Instance.zombieRows[0].Count == 1 && ZombieEvent.Instance.zombieRows[1].Count == 1)
        return;
    }
    else if (row == 1)
    {
      if (ZombieEvent.Instance.zombieRows[0].Count == 1 && ZombieEvent.Instance.zombieRows[1].Count == 1 && ZombieEvent.Instance.zombieRows[2].Count == 1)
        return;
    }
    else if (row == 2)
    {
      if (ZombieEvent.Instance.zombieRows[1].Count == 1 && ZombieEvent.Instance.zombieRows[2].Count == 1 && ZombieEvent.Instance.zombieRows[3].Count == 1)
        return;
    }
    else if (row == 3)
    {
      if (ZombieEvent.Instance.zombieRows[2].Count == 1 && ZombieEvent.Instance.zombieRows[3].Count == 1 && ZombieEvent.Instance.zombieRows[4].Count == 1)
        return;
    }
    else
    {
      if (ZombieEvent.Instance.zombieRows[3].Count == 1 && ZombieEvent.Instance.zombieRows[4].Count == 1)
        return;
    }

    shootTimer += Time.deltaTime;
    if (shootTimer >= firingInterval)
    {
      anim.SetBool("isShoot", true);
      StartCoroutine(WaitSecondsShoot(OffestShoot, Shoot));//OffestShoot:1
      StartCoroutine(ChangeShootAnimation(OffestAnim, anim));//OffestAnim:1.85
      shootTimer = 0;
    }
  }

  public override void Die()
  {
    base.Die();
    BufferPoolManager.Instance.PushObj(PlantManger.Instance.plantType[(int)PlantTypes.ThreeShotPeashooter], this.gameObject);
  }
}