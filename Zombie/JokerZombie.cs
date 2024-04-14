using System;
using System.Collections;
using UnityEngine;

public class JokerZombie : Zombie
{

  protected override void OnEnable()
  {
    base.OnEnable();
    BlastTimer = 0;
  }
  public override void TakeDamage(int damage)
  {
    if (currentHP <= 0) return;
    this.currentHP -= damage;
    if (damage != 15)
    {
      GetComponent<SpriteRenderer>().color = Color.white;
      StartCoroutine(ChangeColor());
    }
    if (currentHP <= 0)
    {
      currentHP = -1;
      anim.SetFloat("Die", 0);//播放死亡动画
      Dead();
    }

    float hppercent = (float)currentHP / HP;//计算当前生命值百分比
    anim.SetFloat("HPpercent", hppercent);
  }
  float BlastTime = 3;
  float BlastTimer = 0;
  void Update()
  {
    if (ZombieEvent.Instance.plantRows[Row].Count <= 1) return;
    BlastTimer += Time.deltaTime;
    if (BlastTimer >= BlastTime)
    {
      Vector3 pos = ZombieEvent.Instance.plantRows[Row][1].transform.position;
      if (pos.x - transform.position.x > -1f && pos.x - transform.position.x < 1f)//在一格左范围内
      {
        anim.SetFloat("HPpercent", -1);
        anim.SetFloat("Die", 1);
        StartCoroutine(Blast(1));
      }
      BlastTimer = 0;
    }
  }
  public override void Dead()
  {
    base.Dead();
    StartCoroutine(BufferPoolManager.Instance.WaitAndPush(ZombieManger.Instance.zombieTypeList[zombieType], this.gameObject, 1.5f));
  }

  IEnumerator Blast(float delay)
  {
    yield return new WaitForSeconds(delay);
    StopAllCoroutines();
    GameObject effect = GameObject.Instantiate(BulletHitManger.Instance.BlastEffect, transform.position, Quaternion.identity);
    Destroy(effect, 0.5f);
    StartCoroutine(GameManger.Instance.WaitForSeconds(PlantDie, 0.2f));
  }
  void PlantDie()
  {
    BufferPoolManager.Instance.PushObj(ZombieManger.Instance.zombieTypeList[zombieType], this.gameObject);
    Plant plant = ZombieEvent.Instance.plantRows[Row][1].GetComponent<Plant>();
    plant.Die();
  }
}
