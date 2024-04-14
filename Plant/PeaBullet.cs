using UnityEngine;

public class PeaBullet : MonoBehaviour
{
  public float Speed = 3;//豌豆飞行速度
  public int attack = 15;//豌豆攻击力
  public bool isfire = false;//是否是火焰子弹

  /// <summary>
  /// 通过PeaShooter来设置值
  /// </summary>
  public void SetSpeed(float speed) => this.Speed = speed;
  public void SetAttack(int attack) => this.attack = attack;

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

      BufferPoolManager.Instance.PushObj(BulletManger.Instance.PeaBullet, this.gameObject);
      StopAllCoroutines();//停止所有协程

      other.GetComponent<Zombie>().TakeDamage(attack);
      GameObject effect = GameObject.Instantiate(BulletHitManger.Instance.PeaBulletHit, transform.position, Quaternion.identity);//实例化特效
      Destroy(effect, 0.5f);//销毁特效
    }
  }
}
