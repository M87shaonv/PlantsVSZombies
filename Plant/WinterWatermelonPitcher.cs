using UnityEngine;

public class WinterWatermelonPitcher : WaterMelonPitcher
{
  protected override void Shoot()
  {
    WinterWatermelonBullet pb = BufferPoolManager.Instance.GetObj(BulletManger.Instance.WinterWatermelonBullet).GetComponent<WinterWatermelonBullet>();
    pb.transform.position = firePoint.position;
    pb.SetRowNumber(row);//设置子弹所属行号
    pb.SetSpeed(bulletSpeed);
    pb.SetAttack(attack);
  }
  public override void Die()
  {
    shootTimer = 0;//重置射击计时器
    AlterHP = HP;//死亡时恢复生命值
    StopAllCoroutines();
    BufferPoolManager.Instance.PushObj(PlantManger.Instance.plantType[(int)PlantTypes.WinterWatermelonPitcher], this.gameObject);

  }
}