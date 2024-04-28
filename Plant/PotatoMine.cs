using System.Collections;
using UnityEngine;

public class PotatoMine : Plant
{
  float ReadyTime = 15;
  int damage = 1000;
  bool canBlast;
  public float Rangex = 1;
  protected override void OnEnable()
  {
#if TEXTING
    ReadyTime = 4;
#endif
    base.OnEnable();
    StartCoroutine(Ready());
    canBlast = false;
  }

  protected override void EnableUpdate()
  {
    if (canBlast)
    {
      RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.right, Rangex);
      foreach (RaycastHit2D hit in hits)
        if (hit.collider.CompareTag("Zombie"))
        {
          GameObject effect = BufferPoolManager.Instance.GetObj(BulletHitManger.Instance.PotatoMineBlast);
          effect.transform.position = transform.position;
          BulletHitManger.Instance.PushEffect(BulletHitManger.Instance.PotatoMineBlast, effect, 0.5f);
          hit.collider.GetComponent<Zombie>().TakeDamage(damage);
          this.Die();
        }
    }
  }

  IEnumerator Ready()
  {
    yield return new WaitForSeconds(ReadyTime);
    anim.Play("Ready");
    yield return new WaitForSeconds(1.03f);
    anim.Play("AlReady");
    canBlast = true;
  }
  public override void Die()
  {
    base.Die();
    StopAllCoroutines();
    BufferPoolManager.Instance.PushObj(PlantManger.Instance.plantType[(int)PlantTypes.PotatoMine], this.gameObject);
  }
#if TEXTING
  void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + Rangex, transform.position.y, transform.position.z));
  }
#endif
}

