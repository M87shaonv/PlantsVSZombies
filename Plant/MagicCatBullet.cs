using System.Collections;
using UnityEngine;

enum BulletState
{
  Search,//搜寻敌人
  Attack,//搜寻到敌人后攻击
}
public class MagicCatBullet : PeaBullet
{
  private Transform target;  // 目标对象
  private BulletState state = BulletState.Search;//默认搜索状态

  void OnEnable()
  {
    state = BulletState.Search;//初始化状态
  }
  void Update()
  {
    switch (state)
    {
      case BulletState.Search:
        StartCoroutine(SearchNearestEnemy());
        break;
      case BulletState.Attack:
        AttackTarget();
        break;
    }
  }
  IEnumerator SearchNearestEnemy()
  {
    while (true)
    {
      Transform nearestEnemy = null;
      float nearestEnemyDistance = Mathf.Infinity;//初始化距离为无穷大
      foreach (var Zombie in ZombieEvent.Instance.SkyZombies)//:计算天空僵尸
      {
        float distance = Vector2.Distance(transform.position, Zombie.transform.position);
        if (distance < nearestEnemyDistance)
        {
          nearestEnemy = Zombie.transform;//更新最近的僵尸
          nearestEnemyDistance = distance;//更新最近的距离
        }
      }
      foreach (var Zombie in ZombieManger.Instance.zombies)
      {
        //计算子弹当前位置与僵尸位置之间的二维距离,以此判断是否为最近的僵尸
        float distance = Vector2.Distance(transform.position, Zombie.transform.position);
        if (distance < nearestEnemyDistance)
        {
          nearestEnemy = Zombie.transform;//更新最近的僵尸
          nearestEnemyDistance = distance;//更新最近的距离
        }
      }
      if (nearestEnemy.GetComponent<Zombie>().currentHP > 0 && nearestEnemy != null)
      {
        this.target = nearestEnemy;
        state = BulletState.Attack;//切换到攻击状态
        yield break;
      }
      //没有找到敌人就进入缓存池
      StopAllCoroutines();
      BufferPoolManager.Instance.PushObj(BulletManger.Instance.MagicCatBullet, this.gameObject);
    }
  }

  void AttackTarget()
  {
    if (target != null && target.GetComponent<Zombie>().currentHP > 0)
    {
      // 获取目标方向
      Vector3 direction = (target.position - transform.position).normalized;

      // 计算朝向目标的旋转角度
      float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
      //应用朝向目标的旋转角度
      transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
      // 沿着目标方向移动
      transform.position = Vector3.MoveTowards(transform.position, target.position, Speed * Time.deltaTime);
    }
    else
    {
      state = BulletState.Search;//目标消失，切换到搜索状态查看是否有新的目标
      StartCoroutine(SearchNearestEnemy());
    }
  }
  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Zombie"))
    {
      StopAllCoroutines();
      BufferPoolManager.Instance.PushObj(BulletManger.Instance.MagicCatBullet, this.gameObject);
      other.GetComponent<Zombie>().TakeDamage(attack);
    }
    if (other.CompareTag("SkyZombie"))
    {
      StopAllCoroutines();
      BufferPoolManager.Instance.PushObj(BulletManger.Instance.MagicCatBullet, this.gameObject);
      other.GetComponent<Zombie>().TakeDamage(1);
    }
  }
}