using System.Collections;
using UnityEngine;

public class Spikeweed : Plant
{
  public float StartX = -1f;//射线起始位置
  public float EndX = 1f;//射线结束位置
  int damage = 20;
  float damageInterval = 2;//攻击间隔
  float damageTimer = 0f;
  int blastCarCount = 4;//可以破坏多少辆车
  protected override void OnEnable()
  {
    base.OnEnable();
    damageTimer = 0;
    blastCarCount = 3;
  }
  protected override void EnableUpdate()
  {
    if (blastCarCount <= 0)
      Die();

    damageTimer += Time.deltaTime;
    if (damageTimer >= damageInterval)
    {
      damageTimer = 0;
      Collider2D[] colliders = Physics2D.OverlapAreaAll(new Vector2(transform.position.x + StartX, transform.position.y), new Vector2(transform.position.x + EndX, transform.position.y));
      foreach (Collider2D collider in colliders)
      {
        if (collider.CompareTag("Zombie"))
        {
          StartCoroutine(Attack());
          collider.GetComponent<Zombie>().TakeDamage(damage);
          if (collider.gameObject.GetComponent<IceZombie>() != null)
          {
            collider.gameObject.GetComponent<IceZombie>().TakeDamage(damage * 100);
            blastCarCount--;
          }
          else if (collider.gameObject.GetComponent<BasketballShootingTruckZombie>() != null)
          {
            collider.gameObject.GetComponent<BasketballShootingTruckZombie>().TakeDamage(damage * 100);
            blastCarCount--;
          }
        }
      }
      damageTimer = 0;
    }
  }
  IEnumerator Attack()
  {
    anim.Play("Attack");
    yield return new WaitForSeconds(1f);
    anim.Play("Idle");
  }
  public override void Die()
  {
    base.Die();
    BufferPoolManager.Instance.PushObj(PlantManger.Instance.plantType[(int)PlantTypes.Spikeweed], this.gameObject);
  }
#if TEXTING
  void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawLine(new Vector3(transform.position.x + StartX, transform.position.y, transform.position.z), new Vector3(transform.position.x + EndX, transform.position.y, transform.position.z));
  }
#endif
}