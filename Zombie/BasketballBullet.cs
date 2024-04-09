using System.Linq;
using DG.Tweening;
using UnityEngine;

public class BasketballBullet : MonoBehaviour
{
  public float speed = 3;
  public int attack = 20;
  public int row; // 行号
  public Transform shadow;
  float flightTime = 2f;
  Plant plant;
  void OnEnable()
  {
    plant = ZombieEvent.Instance.plantRows[row][1];// 第一颗植物
    if (plant != null)
    {
      MoveOnParabola(transform.position, plant.transform.position, 2, flightTime);
    }

    StartCoroutine(BufferPoolManager.Instance.WaitAndPush(BulletManger.Instance.BasketballBullet, this.gameObject, 2.1f));
  }
  void Update()
  {
    SetShadowPosition();
  }
  /// <summary>
  /// 抛物线运动
  /// </summary>
  void MoveOnParabola(Vector3 start, Vector3 end, float height, float duration)
  {
    Sequence sequence = DOTween.Sequence();
    sequence.Append(transform.DOJump(end, height, 1, duration).SetEase(Ease.Linear));
  }
  /// <summary>
  /// 设置影子的位置
  /// </summary>
  void SetShadowPosition()
  {
    // 将影子子物体的位置设置为目标物体的位置，但是Y坐标固定在地面下方一定的距内
    float shadowY = -2f;
    Vector3 shadowPosition = new Vector3(transform.position.x, transform.position.y + shadowY, transform.position.z);
    shadow.transform.position = shadowPosition;
  }
  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Plant") && other.GetComponent<Plant>().row == row)
    {
      //TODO hit Audio
      BufferPoolManager.Instance.PushObj(BulletManger.Instance.BasketballBullet, this.gameObject);
      StopAllCoroutines();
      other.GetComponent<Plant>().TakeDamage(attack);
      float randomRotation = Random.Range(0f, 30f);

      GameObject effect = GameObject.Instantiate(BulletHitManger.Instance.BasketballBulletHit, transform.position, Quaternion.Euler(0, 0, randomRotation));
      Destroy(effect, 0.3f);
    }
  }
  public void SetRowNumber(int Row)// 设置行号
  {
    row = Row;
  }
}