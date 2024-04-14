using UnityEngine;

public class CarambolaBullet : PeaBullet
{
  public int direction;
  void OnEnable()
  {
    StartCoroutine(BufferPoolManager.Instance.WaitAndPush(BulletManger.Instance.CarambolaBullet, this.gameObject, 6));
  }
  void Update()
  {
    switch (direction)
    {
      case 0://上
        transform.Translate(Vector3.up * Speed * Time.deltaTime);
        break;
      case 1://下
        transform.Translate(Vector3.down * Speed * Time.deltaTime);

        break;
      case 2://左
        transform.Translate(Vector3.left * Speed * Time.deltaTime);

        break;
      case 3://右
        transform.Translate(Vector3.right * Speed * Time.deltaTime);

        break;
    }
  }
  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Zombie"))
    {
      AudioManger.Instance.PlayClip(Config.peaShoot);

      BufferPoolManager.Instance.PushObj(BulletManger.Instance.CarambolaBullet, this.gameObject);
      StopAllCoroutines();//停止所有协程
      CancelInvoke();

      other.GetComponent<Zombie>().TakeDamage(attack);
      GameObject effect = Instantiate(BulletHitManger.Instance.CarambolaBulletHit, transform.position, Quaternion.identity);//实例化特效
      Destroy(effect, 0.5f);//销毁特效
    }
  }
}
