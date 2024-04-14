using UnityEngine;
using DG.Tweening;
using System.Collections;

public enum ZombieState
{
  Move,
  Eat,
  Die,
  Pause
}
public class Zombie : MonoBehaviour
{
  //ZombieState zombieState = ZombieState.Move;
  protected ZombieState _zombieState;
  public ZombieState zombieState
  {
    get { return _zombieState; }
    set { _zombieState = value; }
  }
  protected Rigidbody2D rigid;
  protected Animator anim;
  public SpriteRenderer spriteRenderer;
  public float moveSpeed = 1;//移动速度
  public float AlterMoveSpeed;//改变后移动速度
  public int attack = 20;//攻击力
  public float attackInterval = 2;//攻击间隔
  public float attackTimer = 0;//攻击计时器
  public int HP = 100;//生命值
  public int currentHP;//当前生命值
  protected Plant currentPlant;//当前吃植物的对象
  public GameObject zombieHead;//僵尸头
  protected bool havehead = true;//是否有头
  public int Row;
  public bool isPush = false;//有没有进入列表
  private Color origincolor;
  public int zombieType;//僵尸类型

  protected virtual void OnEnable()
  {
    this.GetComponent<Collider2D>().enabled = true;
    isPush = true;
    zombieState = ZombieState.Move;
    currentHP = HP;
    currentPlant = null;
    attackTimer = 0;
    havehead = true;
    AlterMoveSpeed = moveSpeed;
  }
  protected void Start()
  {
    rigid = this.GetComponent<Rigidbody2D>();
    anim = this.GetComponent<Animator>();
    spriteRenderer = this.GetComponent<SpriteRenderer>();
    origincolor = GetComponent<SpriteRenderer>().color;

    isPush = true;
    AlterMoveSpeed = moveSpeed;
    zombieState = ZombieState.Move;
    currentHP = HP;
  }

  protected virtual void FixedUpdate()
  {
    switch (zombieState)
    {
      case ZombieState.Move:
        MoveUpdate();
        break;
      case ZombieState.Eat:
        EatUpdate();
        break;
      case ZombieState.Die:
        break;
      default:
        break;
    }
  }
  /// <summary>
  /// 移动
  /// </summary>
  protected virtual void MoveUpdate()
  {
    if (isPush)
    {
      ZombieEvent.Instance.OnZombieEntered(Row, this);//@通知事件管理器生成了僵尸在哪一行
      isPush = false;
    }

    rigid.MovePosition(rigid.position + Vector2.left * AlterMoveSpeed * Time.deltaTime);
  }
  /// <summary>
  /// 吃植物
  /// </summary>
  protected virtual void EatUpdate()
  {

    attackTimer += Time.deltaTime;
    if (attackTimer >= attackInterval && currentPlant != null)
    {
      //吃植物音效
      AudioManger.Instance.PlayClip(Config.eat);
      currentPlant.TakeDamage(attack);
      attackTimer = 0;
    }
  }

  /// <summary>
  /// 根据碰撞检测来切换是否吃植物
  /// </summary>
  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag("Plant"))
    {
      anim.SetBool("isEating", true);
      zombieState = ZombieState.Eat;
      currentPlant = other.GetComponent<Plant>();//获取触碰到的植物对象的Palnt脚本
    }

    if (other.CompareTag("Door"))
    {
      transform.DOMoveY(-0.5f, 2);//移动到门口
      GameManger.Instance.GameOverFail();//摄像机移动
    }
  }
  void OnTriggerExit2D(Collider2D other)
  {
    if (other.CompareTag("Plant"))
    {
      anim.SetBool("isEating", false);
      zombieState = ZombieState.Move;
      currentPlant = null;//离开植物对象时，将当前吃植物的对象设置为空
      attackTimer = 0;//攻击计时器归零
    }
  }
  public virtual void TransToPause()
  {
    zombieState = ZombieState.Pause;
    anim.enabled = false;
    rigid.bodyType = RigidbodyType2D.Static;//刚体将不会受到物理效果
  }
  /// <summary>
  /// 僵尸受到伤害
  /// </summary>
  public virtual void TakeDamage(int damage)
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
      Dead();
    }
    float hppercent = (float)currentHP / HP;//计算当前生命值百分比
    anim.SetFloat("HPpercent", hppercent);

    if (hppercent < 0.4f && havehead)//是否掉头
    {
      havehead = false;
      // GameObject head = GameObject.Instantiate(zombieHead, transform.position, Quaternion.identity);
      GameObject head = BufferPoolManager.Instance.GetObj(zombieHead);
      head.transform.position = transform.position;
      head.GetComponent<Animator>().Play("Zombie_LostHead");
      //Destroy(head, 2);
      StartCoroutine(BufferPoolManager.Instance.WaitAndPush(zombieHead, head, 1.6f));
    }
  }
  /// <summary>
  /// 僵尸加血
  /// </summary>
  public void AddBlood(int Blood)
  {
    this.currentHP += Blood;
    StartCoroutine(AddBloodEffect.instance.SpawnBloodEffect(transform, AddBloodEffect.instance.ZombieBloodEffect));

    if (this.currentHP >= HP)
    {
      this.currentHP = HP;
    }
  }
  protected IEnumerator ChangeColor()
  {
    yield return new WaitForSeconds(0.1f);
    GetComponent<SpriteRenderer>().color = origincolor;
  }
  protected bool isLastWave = true;
  protected bool isBigWave = true;
  public virtual void Dead()
  {
    if (zombieState == ZombieState.Die) return;
    ZombieEvent.Instance.OnZombieExited(Row, this);//@通知管理器僵尸离开该行
    zombieState = ZombieState.Die;
    GetComponent<Collider2D>().enabled = false;
    ZombieManger.Instance.RemoveZombie(this);//从列表移除

    if (ZombieManger.Instance.zombies.Count == 0)//开始下一波
    {
      ZombieManger.Instance.currentWave++;
      ZombieManger.Instance.TableCreateZombie();
    }
    if (isLastWave && ZombieManger.Instance.currentWave == ZombieManger.Instance.maxProgress)//最后一波
    {
      isLastWave = false;
      UIManger.Instance.ShowLastWaveUI();
    }
    if (ZombieManger.Instance.levelData.levelDataList[ZombieManger.Instance.currentWave].displayWarning == 1)//显示大波僵尸来临UI
    {
      isBigWave = false;
      UIManger.Instance.ShowBigWaveUI();
    }
  }
}