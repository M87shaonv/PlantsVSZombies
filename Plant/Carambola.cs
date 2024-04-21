using System.Collections;
using UnityEngine;

public class Carambola : PeaShooter
{

  public float raycastDistance = 5f;
  public LayerMask detectionLayer;
  public Transform[] FirePoints = new Transform[4];
  void Awake()
  {
    detectionLayer = LayerMask.GetMask("Default");
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    shootTimer = 0;
    StartCoroutine(radiographicInspection());//开启定期检查
  }

  protected override void EnableUpdate()
  {
    //空语句重写,不执行任何语句,继承自PeaShooter
  }
  protected override void Shoot()
  {
    for (int i = 0; i <= 3; i++)
    {
      CarambolaBullet bullet = BufferPoolManager.Instance.GetObj(BulletManger.Instance.CarambolaBullet).GetComponent<CarambolaBullet>();
      bullet.GetComponent<CarambolaBullet>().direction = i;
      bullet.transform.position = FirePoints[i].position;
      bullet.SetSpeed(bulletSpeed);
      bullet.SetAttack(attack);
    }

  }
  public override void Die()
  {
    base.Die();
    StopAllCoroutines();
    AlterHP = HP;//死亡时恢复生命值
    BufferPoolManager.Instance.PushObj(PlantManger.Instance.plantType[(int)PlantTypes.Carambola], this.gameObject);
  }

  IEnumerator radiographicInspection()//定期射线检测僵尸
  {
    while (AlterHP > 0)
    {
      RadiographicInspection();
      yield return new WaitForSeconds(2f);//该植物的攻击时间由这里控制
    }
  }
  void RadiographicInspection()
  {
    RaycastHit2D[] hitsLeft = Physics2D.RaycastAll(transform.position, Vector2.left, raycastDistance, detectionLayer);
    RaycastHit2D[] hitsRight = Physics2D.RaycastAll(transform.position, Vector2.right, raycastDistance, detectionLayer);
    RaycastHit2D[] hitsUp = Physics2D.RaycastAll(transform.position, Vector2.up, raycastDistance, detectionLayer);
    RaycastHit2D[] hitsDown = Physics2D.RaycastAll(transform.position, -Vector2.up, raycastDistance, detectionLayer);

    PrintCollisionTag(hitsLeft);
    PrintCollisionTag(hitsRight);
    PrintCollisionTag(hitsUp);
    PrintCollisionTag(hitsDown);
  }

  void PrintCollisionTag(RaycastHit2D[] hits)
  {
    if (this.plantstate == PlantState.Enable)//!如果没有该条件,会在生成时就调用该函数,导致报null引用错
      if (hits != null)
        foreach (var hit in hits)
        {
          if (hit.collider != null && hit.collider.CompareTag("Zombie"))
          {
            anim.SetBool("isShoot", true);
            StartCoroutine(WaitSecondsShoot(OffestShoot, Shoot));
            StartCoroutine(ChangeShootAnimation(OffestAnim, anim));
            break;
          }
        }
  }
}




