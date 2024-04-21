using System.Collections;
using UnityEngine;

enum CannonState
{
  NoBulletIdle,//无子弹空闲状态
  Idle,//空闲状态
}
// :进入为NoBulletIdle状态, 仅在NoBulletIdle状态计算填弹时间
// :isLoad==true进入Load, Load动画播放完毕便isLdle==true进入有弹状态, 此时可发射炮弹
// :isShoot==true播放发射动画, 位置有偏差需要偏移, 动画播放完毕后
// :所有标志位改为false, 进入NoBulletIdle状态
public class CornCannon : Plant
{
  float LoadTIme = 20;//装载时间
  public float LoadTimer = 0;
  CannonState state = CannonState.NoBulletIdle;
  bool Tip = false;
  /// <summary>
  /// 炮弹击中点提示
  /// </summary>
  GameObject ShellLanding;
  protected override void OnEnable()
  {
#if TEXTING
    LoadTIme = 2;
#endif
    base.OnEnable();
    state = CannonState.NoBulletIdle;
    Tip = false;
  }

  protected override void EnableUpdate()
  {
    switch (state)
    {
      case CannonState.NoBulletIdle:
        NoBulletIdle();
        break;
      case CannonState.Idle:
        Idle();
        break;
      default:
        break;
    }
  }

  void NoBulletIdle()
  {
    Debug.Log(LoadTimer);
    LoadTimer += Time.deltaTime;
    if (LoadTimer >= LoadTIme)
    {
      TransToIdle();
    }
  }
  void TransToNoBulletIdle()
  {
    anim.SetBool("isLoad", false);
    state = CannonState.NoBulletIdle;
  }
  void Idle()
  {
    //点击显示炮弹击中点提示
    if (Input.GetMouseButtonDown(0))
    {
      Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);
      if (hit.collider != null && hit.collider.gameObject == this.gameObject && ShellLanding == null)
      {
        Tip = true;
        ShellLanding = BufferPoolManager.Instance.GetObj(BulletHitManger.Instance.ShellLandingTip);
        ++clickCount;
      }
    }
    if (Tip)
    {
      if (Input.GetMouseButtonUp(0))
      {
        ++clickCount;
      }
      ShellLandingTip(ShellLanding);
      //左击发射炮弹 
      if (clickCount == 2 && Input.GetMouseButtonDown(0))
      {
        Shoot();
        Tip = false;
        clickCount = 0;
        ShellLanding = null;
        LoadTimer = 0;
        TransToNoBulletIdle();//直接进入NoBulletIdle状态,否则点击还会实例化ShellLanding
      }
      //右击放弃发射
      if (Input.GetMouseButtonDown(1))
      {
        BufferPoolManager.Instance.PushObj(BulletHitManger.Instance.ShellLandingTip, ShellLanding.gameObject);
        clickCount = 0;
        Tip = false;
        ShellLanding = null;
        LoadTimer = 0;
        TransToNoBulletIdle();
      }
    }
  }
  int clickCount = 0;
  void ShellLandingTip(GameObject ShellLanding)
  {
    Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);//将屏幕坐标转换为世界坐标
    mouseWorldPosition.z = 0;//! 不设置z轴会和camera保持一致
    ShellLanding.transform.position = mouseWorldPosition;
  }
  void TransToIdle()
  {
    anim.SetBool("isLoad", true);
    StartCoroutine(SetisLdle());//装载动画播放完毕后进入有弹状态
  }
  IEnumerator SetisLdle()
  {
    yield return new WaitForSeconds(0.5f);
    anim.SetBool("isLdle", true);
    state = CannonState.Idle;
  }

  //Idle状态下点击发射炮弹
  void Shoot()
  {
    GameObject Shell = BufferPoolManager.Instance.GetObj(BulletManger.Instance.CornCannonBullet);
    Shell.transform.position = ShellLanding.transform.position + new Vector3(0, 9, 0);
    Shell.GetComponent<CornCannonBullet>().SetTargetPosition(ShellLanding.transform);
    BufferPoolManager.Instance.PushObj(BulletHitManger.Instance.ShellLandingTip, ShellLanding.gameObject);
    anim.SetBool("isShoot", true);
    StartCoroutine(ShootAfter());
  }
  IEnumerator ShootAfter()
  {
    yield return new WaitForSeconds(2.1f);
    anim.SetBool("isShoot", false);
    anim.SetBool("isLoad", false);
    anim.SetBool("isLdle", false);
  }
}
