using System.Collections;
using UnityEngine;

public class WinterWatermelonBullet : WatermelonBullet
{
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
    StartCoroutine(BufferPoolManager.Instance.WaitAndPush(BulletManger.Instance.WinterWatermelonBullet, this.gameObject, 2.1f));
  }
  protected override IEnumerator AttackNearbyZombie()
  {
    yield return new WaitForSeconds(2);
    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
    foreach (Collider2D collider in colliders)
    {
      if (collider.CompareTag("Zombie"))
      {
        collider.GetComponent<Zombie>().TakeDamage(attack - count * 2);
        ++count;
        //减速
        collider.GetComponent<SpriteRenderer>().color = new Color32(60, 220, 220, 255);
        if (collider.GetComponent<Zombie>().AlterMoveSpeed > 0.1f)
        {
          Vector3 offest = new Vector3(-0.8f, 0.8f, 0);
          Vector3 newPos = transform.position - offest;
          GameObject effect = BufferPoolManager.Instance.GetObj(BulletHitManger.Instance.SnowPeaBulletHit);
          effect.transform.position = newPos;
          BulletHitManger.Instance.PushEffect(BulletHitManger.Instance.SnowPeaBulletHit, effect, 0.5f);
          collider.GetComponent<Zombie>().AlterMoveSpeed -= 0.05f;
        }
      }
    }
    BufferPoolManager.Instance.PushObj(BulletManger.Instance.WinterWatermelonBullet, this.gameObject);
    StopAllCoroutines();
  }
}