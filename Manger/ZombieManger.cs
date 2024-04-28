using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ZombieTypes : int
{
  /// <summary>
  /// 普通僵尸
  /// </summary>
  NormalZombie,
  /// <summary>
  /// 旗帜僵尸
  /// </summary>
  flagZombie,
  /// <summary>
  /// 锥头僵尸
  /// </summary>
  ConeHeadZombie,
  /// <summary>
  /// 铁桶僵尸
  /// </summary>
  BucketheadZombie,
  /// <summary>
  /// 铁门僵尸
  /// </summary>
  IronGateZombie,
  /// <summary>
  /// 橄榄球僵尸
  /// </summary>
  RugbyZombie,
  /// <summary>
  /// 雪人僵尸
  /// </summary>
  SnowmanZombie,
  /// <summary>
  /// 篮球车僵尸
  /// </summary>
  BasketballShootingTruckZombie,
  /// <summary>
  /// 小丑僵尸
  /// </summary>
  JokerZombie,
  /// <summary>
  /// 气球僵尸
  /// </summary>
  BalloonZombie,
  /// <summary>
  /// 报纸僵尸
  /// </summary>
  NewsPaperZombie,
  /// <summary>
  /// 体育生僵尸
  /// </summary>
  SportsZombies,
  /// <summary>
  /// 矿工僵尸
  /// </summary>
  MinerZombie,
  /// <summary>
  /// 小鬼僵尸
  /// </summary>
  LitterGhostZombie,
  /// <summary>
  /// 冰车僵尸
  /// </summary>
  IceZombie,
  /// <summary>
  /// 巨型僵尸
  /// </summary>
  GiantZombie,
  /// <summary>
  /// 巨型绿僵尸
  /// </summary>
  GiantGreenZombie,
  /// <summary>
  /// 绿小鬼僵尸
  /// </summary>
  LitterGhostGreenZombie,
  /// <summary>
  /// 舞王僵尸
  /// </summary>
  DancerZombie,
  /// <summary>
  /// 追随舞王者僵尸
  /// </summary>
  BackupDancerZombie,
  /// <summary>
  /// 迪斯科僵尸
  /// </summary>
  DiscoZombie,
  /// <summary>
  /// 追随迪斯科者僵尸
  /// </summary>
  FollowerZombie,
}
public class ZombieManger : MonoBehaviour
{
  public static ZombieManger Instance { get; private set; }

  private void Awake()
  {
    Instance = this;
  }

  public Transform[] spawnPointList;//生成点数组
  public List<Zombie> zombies = new List<Zombie>();//僵尸列表
  //private List<GameObject> zombieList = new List<GameObject>();//当前波次的所有僵尸
  int zombieCount = 0;//僵尸数量
  void Start()
  {
    PlayerPrefs.SetInt("Level", 1);//初始化关卡数
    //TODO:当前关卡数
    currentLevel = PlayerPrefs.GetInt("Level");
    Readtable();
  }

  void Readtable()
  {
    StartCoroutine(LoadTable());
  }

  IEnumerator LoadTable()//异步读取表格
  {
    ResourceRequest request = Resources.LoadAsync("Level");
    yield return request;
    levelData = request.asset as LevelData;
  }

  public int currentWave = 1;//当前波数
  public LevelData levelData;//关卡数据
  public int currentLevel;//当前关卡数
  public float maxProgress = 0;//当前最大进度
  private int order = 1;//用于排序,保证后生成的僵尸在前面显示

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space))//空格暂停游戏
    {
      UIManger.Instance.settingsUI.PauseGame();
    }
  }
  /// <summary>
  /// 从表格生成僵尸
  /// </summary>
  int current = 0;
  public bool isEnd;
  public void TableCreateZombie()
  {
    isEnd = true;
    for (current = 0; current < levelData.levelDataList.Count; current++)//当前关卡当前波次的所有levelItem数据
    {
      LevelItem levelItem = levelData.levelDataList[current];
      if (levelItem.LevelID == currentLevel && levelItem.progress == currentWave)
      {
        StartSpawn(levelItem);
        isEnd = false;
      }
      if (maxProgress == 0)
      {
        maxProgress = levelItem.Maxprogress;
        LevelManger.Instance.currentLevel = currentLevel;
      }
    }
    if (isEnd && zombies.Count == 0)//游戏胜利
    {
      StopAllCoroutines();//停止所有协程
      GameManger.Instance.GameOverSuccess();
    }
  }

  /// <summary>
  /// 开始生成
  /// </summary>
  public void StartSpawn(LevelItem levelItem)
  {
    StartCoroutine(SpawnZombie(levelItem));
  }
  public List<GameObject> zombieTypeList = new List<GameObject>();//所有僵尸类型
  /// <summary>
  /// 使用协程生成僵尸
  /// </summary>
  IEnumerator SpawnZombie(LevelItem levelItem)
  {
    float currentTime = levelItem.createTime + Time.time;//BUG 这里时间不对,不是每一波的开始时间,而是游戏开始运行时的时间加
    yield return new WaitForSeconds(currentTime);//指定时间后生成
    //GameObject zombieperfab = Resources.Load("Perfabs/Zombie/Zombie" + levelItem.zombieType.ToString()) as GameObject;
    Zombie zombie = BufferPoolManager.Instance.GetObj(zombieTypeList[levelItem.zombieType]).GetComponent<Zombie>();//从缓存池中获取预制体

    int index = Random.Range(0, spawnPointList.Length);
    zombie.transform.position = spawnPointList[index].position;
    //GameObject zombie = Instantiate(zombieperfab, spawnPointList[index].position, Quaternion.identity);
    zombies.Add(zombie.GetComponent<Zombie>());//:得到僵尸身上的脚本将其添加到列表中
    if (levelItem.zombieType == 15 || levelItem.zombieType == 16)//:防止巨型僵尸在最上面出现
    {
      index = Random.Range(0, spawnPointList.Length - 1);
    }
    zombie.Row = index;//@设置行数
    // : 不同生成点的order不同,以此保证僵尸重叠时不会闪烁 
    zombie.GetComponent<SpriteRenderer>().sortingOrder = spawnPointList[index].GetComponent<SpriteRenderer>().sortingOrder + order;
    order++;//排序order
            //!TMD,忘记了写生成下一波的僵尸的代码了,找了一下午,都nm快重构了
    UIManger.Instance.progressUI.SetProgress(currentWave / maxProgress);
  }
  public void RemoveZombie(Zombie zombie)
  {
    zombies.Remove(zombie);
  }
}
