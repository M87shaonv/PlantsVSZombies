using System.Collections;
using UnityEngine;

public class JokerZombie : Zombie
{
  public float radius;
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
    Debug.Log(BlastTimer);
    if (ZombieEvent.Instance.plantRows[Row].Count == 1) return;
    BlastTimer += Time.deltaTime;
    if (BlastTimer >= BlastTime)
    {
      Vector3 pos = ZombieEvent.Instance.plantRows[Row][1].transform.position;
      //没有吃植物,且在一格左范围内
      if (zombieState != ZombieState.Eat && pos.x - transform.position.x > -1f && pos.x - transform.position.x < 1f)
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
    //GameObject effect = GameObject.Instantiate(BulletHitManger.Instance.BlastEffect, transform.position, Quaternion.identity);
    GameObject effect = BufferPoolManager.Instance.GetObj(BulletHitManger.Instance.BlastEffect);
    effect.transform.position = transform.position;
    BulletHitManger.Instance.PushEffect(BulletHitManger.Instance.BlastEffect, effect, 0.5f);
    //Destroy(effect, 0.5f);
    StartCoroutine(GameManger.Instance.WaitForSeconds(PlantDie, 0.2f));
  }
  void PlantDie()
  {
    BufferPoolManager.Instance.PushObj(ZombieManger.Instance.zombieTypeList[zombieType], this.gameObject);
    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
    foreach (Collider2D collider in colliders)
    {
      if (collider.CompareTag("Plant"))
      {
        Plant plant = collider.GetComponent<Plant>();
        if (plant != null)
        {
          plant.Die();
        }
      }
    }
  }
#if TEXTING//调试用
  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, radius);
  }
#endif
}
