using UnityEngine;

public class ConeHeadZombie : Zombie
{
  override public void TakeDamage(int damage)
  {
    if (currentHP <= 0) return;
    GetComponent<SpriteRenderer>().color = Color.white;
    StartCoroutine(ChangeColor());
    this.currentHP -= damage;

    float hppercent = (float)currentHP / HP;//计算当前生命值百分比
    if (hppercent < 0.3f)//如果生命值低于30%,就更新为普通僵尸
    {
      GameObject newZombie = BufferPoolManager.Instance.GetObj(ZombieManger.Instance.zombieTypeList[(int)ZombieTypes.NormalZombie]);
      newZombie.transform.position = transform.position;
      ZombieEvent.Instance.OnZombieExited(Row, this);//@通知管理器僵尸离开该行
      BufferPoolManager.Instance.PushObj(ZombieManger.Instance.zombieTypeList[zombieType], this.gameObject);

      ZombieManger.Instance.zombies.Add(newZombie.GetComponent<Zombie>());
    }
  }
}
