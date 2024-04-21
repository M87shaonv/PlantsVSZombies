using System.Collections;
using DG.Tweening;
using UnityEngine;
//: 进入时是Thrown状态,会丢到后排

public class LitterGhostZombie : Zombie
{
  protected override void OnEnable()
  {
    base.OnEnable();
    float random = Random.Range(5f, 8f);
    MoveOnParabola(transform.position, new Vector3(transform.position.x - random, transform.position.y, transform.position.z), 3, 2);
    StartCoroutine(AnimTOWalk());
  }
  protected override void FixedUpdate()
  {
    base.FixedUpdate();
  }
  /// <summary>
  /// 抛出后,转为Walk状态
  /// </summary>
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
  public override void TakeDamage(int damage)
  {
    if (currentHP <= 0) return;
    this.currentHP -= damage;
    if (damage != 15)
    {
      GetComponent<SpriteRenderer>().color = Color.white;
      StartCoroutine(ChangeColor());
    }
    if (currentHP <= 0)
    {
      currentHP = -1;
      Dead();
    }
    float hppercent = (float)currentHP / HP;//计算当前生命值百分比
    anim.SetFloat("HPpercent", hppercent);
  }

  public override void Dead()
  {
    base.Dead();
    StartCoroutine(BufferPoolManager.Instance.WaitAndPush(ZombieManger.Instance.zombieTypeList[(int)zombieType], this.gameObject, 1.5f));
  }
}