using UnityEngine;

public class FirePeaBullet : PeaBullet
{
  void OnEnable()
  {
    StartCoroutine(BufferPoolManager.Instance.WaitAndPush(BulletManger.Instance.FirePeaBullet, this.gameObject, 6f));
  }
  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Zombie"))
    {
      AudioManger.Instance.PlayClip(Config.peaShoot);
      isDownward = false;
      isUpward = false;
      BufferPoolManager.Instance.PushObj(BulletManger.Instance.FirePeaBullet, this.gameObject);
      StopAllCoroutines();//停止所有协程
      CancelInvoke();

      other.GetComponent<Zombie>().TakeDamage(attack);

      GameObject effect = BufferPoolManager.Instance.GetObj(BulletHitManger.Instance.FirePeaBulletHit);
      effect.transform.position = transform.position;
      BulletHitManger.Instance.PushEffect(BulletHitManger.Instance.FirePeaBulletHit, effect, 0.5f);
    }
  }
}
