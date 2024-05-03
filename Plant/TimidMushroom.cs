using UnityEngine;

public class TimidMushroom : PeaShooter
{
  public Vector2 boxSize; //长方形检测范围的尺寸
  public bool isFear; //是否处于恐惧状态
  protected override void OnEnable()
  {
    base.OnEnable();
    isFear = false;
  }

  protected override void EnableUpdate()
  {
    CheckNearbyZombies();

    if (isFear) return;//如果处于恐惧状态，则不再射击
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
  void CheckNearbyZombies()
  {
    Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f);// 执行长方形范围检测
    foreach (Collider2D collider in colliders)
    {
      if (collider.CompareTag("Zombie"))
      {
        anim.Play("Feard");
        isFear = true;
        shootTimer = 0;
        return;
      }
    }
    if (isFear)
    {
      anim.Play("Idle");
      isFear = false;
    }
  }
  void PlayFeard()//帧事件调用
  {
    anim.Play("Feard");
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
#if TEXTING
  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(transform.position, boxSize);
  }
#endif
}