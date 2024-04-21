using System.Collections;
using DG.Tweening;
using UnityEngine;

public class LitterGhostGreenZombie : LitterGhostZombie
{
  protected override void OnEnable()
  {
    base.OnEnable();
    float random = Random.Range(2f, 4f);
    MoveOnParabola(transform.position, new Vector3(transform.position.x - random, transform.position.y, transform.position.z), 3, 2);
    StartCoroutine(AnimTOWalk());
  }
  IEnumerator AnimTOWalk()
  {
    yield return new WaitForSeconds(2);
    anim.Play("Walk");
  }
  /// <summary>
  /// 抛物线运动
  /// </summary>
  void MoveOnParabola(Vector3 start, Vector3 end, float height, float duration)
  {
    Sequence sequence = DOTween.Sequence();
    sequence.Append(transform.DOJump(end, height, 1, duration).SetEase(Ease.Linear));
  }
}