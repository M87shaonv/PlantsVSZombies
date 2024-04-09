using System.Collections;
using UnityEngine;

public class SnowPeaBullet : PeaBullet
{
  void OnEnable()
  {
    StartCoroutine(BufferPoolManager.Instance.WaitAndPush(BulletManger.Instance.SnowPeaBullet, this.gameObject, 6f));
  }
  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Zombie"))
    {
      if (other.GetComponent<IronGateZombie>() != null)//铁门僵尸可以防御冰子弹
      {
        //AudioManger.Instance.PlayClip(Config.peaShoot);
        //Destroy(this.gameObject);
        BufferPoolManager.Instance.PushObj(BulletManger.Instance.SnowPeaBullet, this.gameObject);
        return;
      }
      AudioManger.Instance.PlayClip(Config.peaShoot);
      //Destroy(this.gameObject);
      BufferPoolManager.Instance.PushObj(BulletManger.Instance.SnowPeaBullet, this.gameObject);

      Vector3 offest = new Vector3(-0.8f, 0.8f, 0);
      Vector3 newPos = transform.position - offest;
      other.GetComponent<Zombie>().TakeDamage(attack);
      GameObject effect = GameObject.Instantiate(BulletHitManger.Instance.SnowPeaBulletHit, newPos, Quaternion.identity);//实例化特效
      Destroy(effect, 0.5f);//销毁特效
      other.GetComponent<Zombie>().spriteRenderer.color = new Color32(60, 255, 255, 255);
      if (other.GetComponent<Zombie>().AlterMoveSpeed > 1)
      {
        if (other.GetComponent<Zombie>().AlterMoveSpeed <= 1)
          return;

        other.GetComponent<Zombie>().AlterMoveSpeed /= 2;
      }
    }
  }
}
