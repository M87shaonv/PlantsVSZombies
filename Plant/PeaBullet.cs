using System.Collections;
using UnityEngine;

public class PeaBullet : MonoBehaviour
{
  public float Speed = 3;//豌豆飞行速度
  public int attack = 15;//豌豆攻击力
  public bool isfire = false;//是否是火焰子弹
  public float UpwardDistance = 3.5f;//向上飞行距离
  public float DownwardDistance = -3.5f;//向下飞行距离
  public bool isUpward = false;//是否向上飞行
  public bool isDownward = false;//是否向下飞行
  /// <summary>
  /// 通过PeaShooter来设置值
  /// </summary>
  public void SetSpeed(float speed) => this.Speed = speed;
  public void SetAttack(int attack) => this.attack = attack;
  public IEnumerator Move(float moveDuration, Vector3 target, Vector3 start)
  {
    float elapsedTime = 0;

    while (elapsedTime < moveDuration)
    {
      transform.position = Vector3.Lerp(start, target, elapsedTime / moveDuration);
      elapsedTime += Time.deltaTime;
      yield return null;
    }
    transform.position = target; // 确保最终位置准确
  }
  void OnEnable()//当激活时
  {
    StartCoroutine(BufferPoolManager.Instance.WaitAndPush(BulletManger.Instance.PeaBullet, this.gameObject, 6));
  }

  void Update()
  {
    transform.Translate(Vector3.right * Speed * Time.deltaTime);
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Zombie"))
    {
      AudioManger.Instance.PlayClip(Config.peaShoot);
      isDownward = false;
      isUpward = false;
      isfire = false;
      BufferPoolManager.Instance.PushObj(BulletManger.Instance.PeaBullet, this.gameObject);
      StopAllCoroutines();//停止所有协程

      other.GetComponent<Zombie>().TakeDamage(attack);
      GameObject effect = BufferPoolManager.Instance.GetObj(BulletHitManger.Instance.PeaBulletHit);
      effect.transform.position = this.transform.position;
      BulletHitManger.Instance.PushEffect(BulletHitManger.Instance.PeaBulletHit, this.gameObject, 0.5f);
    }
  }
}
