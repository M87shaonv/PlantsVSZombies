using UnityEngine;

public class BucketheadZombie : Zombie
{
  override public void TakeDamage(int damage)
  {
    if (currentHP <= 0) return;
    GetComponent<SpriteRenderer>().color = Color.white;
    StartCoroutine(ChangeColor());
    this.currentHP -= damage;

    float hppercent = (float)currentHP / HP;//计算当前生命值百分比
    if (hppercent < 0.3f)//如果生命值低于30%,就更新为锥头僵尸
    {
      GameObject newZombie = BufferPoolManager.Instance.GetObj(ZombieManger.Instance.zombieTypeList[(int)ZombieTypes.ConeHeadZombie]);
      newZombie.transform.position = transform.position;
      ZombieEvent.Instance.OnZombieExited(Row, this);//@通知管理器僵尸离开该行
      BufferPoolManager.Instance.PushObj(ZombieManger.Instance.zombieTypeList[zombieType], this.gameObject);

      //$因为普僵在开始移动时会自动调用OnZombieEntered,所以这里不需要再通知管理器
      ZombieManger.Instance.zombies.Add(newZombie.GetComponent<Zombie>());
    }
  }
}
