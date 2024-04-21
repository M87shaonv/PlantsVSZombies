using UnityEngine;
using DG.Tweening;
using System.Collections;

public class CornCannonBullet : MonoBehaviour
{
  public int attack = 800;
  float movementDuration = 2;
  public float radius = 2;
  Transform targetPosition;
  /// <summary>
  /// 设置目标位置并启动移动协程
  /// </summary>
  public void SetTargetPosition(Transform targetPosition)
  {
    this.targetPosition = targetPosition;
    StartCoroutine(Move());
  }
  IEnumerator Move()
  {
    yield return new WaitForSeconds(2);
    transform.DOMove(targetPosition.position, movementDuration)
        .SetEase(Ease.Linear)  // 设置移动的缓动类型，这里使用线性均速移动
        .OnComplete(() => CheckNearbyZombies());
  }
  void CheckNearbyZombies()
  {
    // 定义球形范围的参数：检测的位置和半径
    Vector3 center = transform.position; // 检测的位置为当前对象的位置
    BufferPoolManager.Instance.PushObj(BulletManger.Instance.CornCannonBullet, this.gameObject);
    GameObject effect = BufferPoolManager.Instance.GetObj(BulletHitManger.Instance.CornCannonBulletHit);
    effect.transform.position = transform.position;
    BulletHitManger.Instance.PushEffect(BulletHitManger.Instance.CornCannonBulletHit, effect, 0.95f);
    Collider2D[] colliders = Physics2D.OverlapCircleAll(center, radius);

    foreach (Collider2D collider in colliders)
    {
      if (collider.CompareTag("Zombie"))
      {
        Zombie zombie = collider.GetComponent<Zombie>();
        if (zombie == null) continue;
        zombie.TakeDamage(attack);
      }
      if (collider.CompareTag("Cell"))//解冻格子
      {
        collider.GetComponent<Cell>().EnableCell();
      }
    }

  }

#if TEXTING
  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, radius);
  }
#endif
}