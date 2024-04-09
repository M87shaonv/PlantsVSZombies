using UnityEngine;

public class FirePeaBullet : PeaBullet
{
  void OnEnable()
  {
    StartCoroutine(BufferPoolManager.Instance.WaitAndPush(BulletManger.Instance.FirePeaBullet, this.gameObject, 6f));
  }
  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.tag == "Zombie")
    {
      AudioManger.Instance.PlayClip(Config.peaShoot);
      //Destroy(this.gameObject);
      BufferPoolManager.Instance.PushObj(BulletManger.Instance.FirePeaBullet, this.gameObject);
      StopAllCoroutines();//停止所有协程
      CancelInvoke();

      other.GetComponent<Zombie>().TakeDamage(attack);

      GameObject effect = GameObject.Instantiate(BulletHitManger.Instance.FirePeaBulletHit, transform.position, Quaternion.identity);//实例化特效
      Destroy(effect, 0.5f);//销毁特效
    }
  }
}
