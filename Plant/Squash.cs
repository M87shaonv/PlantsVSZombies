using DG.Tweening;
using UnityEngine;

public class Squash : Plant
{
  public int attack;
  private int attackCount = 2;
  public BoxCollider2D boxCollider;
  void OnEnable()
  {
    AlterHP = HP;
    TransToDisable();//默认禁用状态
    attackCount = 2;
  }
  void Awake()
  {
    anim = GetComponent<Animator>();
    boxCollider.enabled = false;
  }
  public override void TransToEnable()
  {
    base.TransToEnable();
    boxCollider.enabled = true;
  }
  void OnTriggerEnter2D(Collider2D other)
  {
    if (!GetComponent<BoxCollider2D>().enabled)//@防止在手上时也攻击
    {
      return;
    }
    if (other.CompareTag("Zombie"))
    {
      AudioManger.Instance.PlayClip(Config.Squash);
      //获取碰撞体上离当前对象中心点最近的点，这个点通常可以看作是碰撞发生的接触点
      Vector3 othersite = other.transform.position;
      //将接触点从世界坐标转换为当前对象的局部坐标。这样做是为了在当前对象的坐标系中判断接触点的位置
      Vector3 localothersite = transform.InverseTransformPoint(othersite);

      Transform start = this.transform;
      Transform end = other.transform;
      var mid = (start.position + end.position) * 0.5f + (Vector3.up * 1);

      this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
      if (localothersite.x > 0)//假设对象面向z轴正方向,x轴正方向为右,x轴负方向为左
      {
        lookLeft();
      }
      if (localothersite.x < 0)
      {
        lookRight();
      }
      transform.DOPath(new Vector3[] { start.position, mid, end.position }, 1f, PathType.CatmullRom)
.SetEase(Ease.OutQuad).OnComplete(() =>
boom(other));
      anim.SetTrigger("def");
    }
  }

  void lookLeft()
  {
    anim.SetTrigger("left");
  }
  void lookRight()
  {
    anim.SetTrigger("right");
  }
  void Up()
  {
    anim.SetTrigger("up");
  }
  void Down()
  {
    anim.SetTrigger("down");
  }
  void boom(Collider2D other)
  {
    other.GetComponent<Zombie>().TakeDamage(attack);
    if (attackCount-- == 0)
    {
      Destroy(this.gameObject);
    }
    this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
  }
}
