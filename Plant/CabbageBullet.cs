using System.Linq;
using UnityEngine;
using DG.Tweening;

public class CabbageBullet : MonoBehaviour
{
  public float speed = 3;// bullet speed
  public int attack = 25;// bullet attack power
  protected float flightTime = 2f; // 飞行时间（抛物线运动持续时间）
  protected Zombie lastZombie; // 最后一个僵尸
  protected int row; // 行号
  public Transform shadow;

  public void SetSpeed(float speed) => this.speed = speed;
  public void SetAttack(int attack) => this.attack = attack;
  //!有一些奇怪的bug,子弹有时会在原地旋转一会
  protected virtual void OnEnable()
  {
    shadow = transform.Find("Shadow");
    lastZombie = ZombieEvent.Instance.zombieRows[row].Last();// 最后一个僵尸
    if (lastZombie != null)
    {
      Vector3 targetPos = lastZombie.transform.position;
      targetPos.y -= 1f;
      MoveOnParabola(transform.position, targetPos, 2, flightTime);
    }

    StartCoroutine(BufferPoolManager.Instance.WaitAndPush(BulletManger.Instance.CabbageBullet, this.gameObject, 2.1f));
  }
  void Update()
  {
    SetShadowPosition();
  }
  /// <summary>
  /// 抛物线运动
  /// </summary>
  protected void MoveOnParabola(Vector3 start, Vector3 end, float height, float duration)
  {
    Sequence sequence = DOTween.Sequence();
    sequence.Append(transform.DOJump(end, height, 1, duration).SetEase(Ease.Linear));
  }
  /// <summary>
  /// 设置影子的位置
  /// </summary>
  protected void SetShadowPosition()
  {
    // 将影子子物体的位置设置为目标物体的位置，但是Y坐标固定在地面下方一定的距内
    float shadowY = -2f;
    Vector3 shadowPosition = new Vector3(transform.position.x, transform.position.y + shadowY, transform.position.z);
    shadow.transform.position = shadowPosition;
  }
  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Zombie"))
    {
      //TODO hit Audio
      BufferPoolManager.Instance.PushObj(BulletManger.Instance.CabbageBullet, this.gameObject);
      StopAllCoroutines();
      other.GetComponent<Zombie>().TakeDamage(attack);
      GameObject effect = BufferPoolManager.Instance.GetObj(BulletHitManger.Instance.CabbageBulletHit);
      effect.transform.position = transform.position;
      BulletHitManger.Instance.PushEffect(BulletHitManger.Instance.CabbageBulletHit, effect, 2);
    }
  }
  public void SetRowNumber(int Row)// 设置行号
  {
    row = Row;
  }
}

