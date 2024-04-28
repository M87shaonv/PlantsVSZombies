using UnityEngine;

public class TimidMushroomBullet : PeaBullet
{
  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Zombie"))
    {

      AudioManger.Instance.PlayClip(Config.peaShoot);
      BufferPoolManager.Instance.PushObj(BulletManger.Instance.TimidMushroomBullet, this.gameObject);
      StopAllCoroutines();//停止所有协程

      other.GetComponent<Zombie>().TakeDamage(attack);
      GameObject effect = BufferPoolManager.Instance.GetObj(BulletHitManger.Instance.TimidMushroomBulletHit);
      effect.transform.position = this.transform.position;
      BulletHitManger.Instance.PushEffect(BulletHitManger.Instance.TimidMushroomBulletHit, this.gameObject, 0.5f);
    }
  }
}