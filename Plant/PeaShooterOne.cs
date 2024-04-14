using UnityEngine;

public class PeaShooterOne : PeaShooter
{

  protected override void OnEnable()
  {
    base.OnEnable();
    shootTimer = 0;//重置射击计时器
    AlterHP = HP;//死亡时恢复生命值
  }
  protected override void EnableUpdate()
  {
    if (ZombieEvent.Instance.zombieRows[row].Count == 1) return;//@如果该行没有僵尸，则不再射击

    shootTimer += Time.deltaTime;
    if (shootTimer >= firingInterval)
    {
      anim.SetBool("isShoot", true);
      StartCoroutine(WaitSecondsShoot(OffestShoot, Shoot));//OffestShoot:0.7
      StartCoroutine(ChangeShootAnimation(OffestAnim, anim));//OffestAnim:1.86
      shootTimer = 0;
    }
  }
  public override void Die()
  {
    base.Die();
    StopAllCoroutines();
    BufferPoolManager.Instance.PushObj(PlantManger.Instance.plantType[(int)PlantTypes.PeaShooterOne], this.gameObject);
  }
}