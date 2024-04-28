using System.Collections;
using UnityEngine;

public class WatermelonBullet : CabbageBullet
{
  public float radius = 0; //球形检测范围的半尺寸
  Vector2 size;
  protected int count = 1;

  protected override void OnEnable()
  {
    count = 1;
    shadow = transform.Find("Shadow");
    if (ZombieEvent.Instance.zombieRows[row].Count > 1)
    {
      Vector3 targetPos = ZombieEvent.Instance.zombieRows[row][1].transform.position;
      targetPos.y -= 1f;
      MoveOnParabola(transform.position, targetPos, 2, flightTime);
    }
    StartCoroutine(AttackNearbyZombie());
    StartCoroutine(BufferPoolManager.Instance.WaitAndPush(BulletManger.Instance.WatermelonBullet, this.gameObject, 2.1f));
  }

  protected virtual IEnumerator AttackNearbyZombie()
  {
    yield return new WaitForSeconds(2);
    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
    foreach (Collider2D collider in colliders)
    {
      if (collider.CompareTag("Zombie"))
        collider.GetComponent<Zombie>().TakeDamage(attack - count * 2);
      ++count;
    }
    BufferPoolManager.Instance.PushObj(BulletManger.Instance.WatermelonBullet, this.gameObject);
    StopAllCoroutines();
  }

#if TEXTING
  void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, radius);
  }
#endif
}