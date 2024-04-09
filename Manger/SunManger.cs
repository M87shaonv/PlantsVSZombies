using TMPro;
using UnityEngine;

/// <summary>
/// 太阳管理类
/// </summary>
public class SunManger : MonoBehaviour
{
  /// <summary>
  /// 太阳管理静态属性
  /// </summary>
  public static SunManger Insance { get; private set; }

  //Awake方法是C#类的一个特殊方法,在游戏对象被激活时自动执行
  private void Awake()
  {
    Insance = this;  //将当前实例赋值给Insance
  }

  /// <summary>
  /// 阳光值
  /// [SerializeField]是一个用于序列化和反序列化的属性装饰器。
  /// 当这个装饰器应用于类的属性时，在序列化和反序列化过程中，这个属性会被包含在序列化结果中。
  /// 这样当玩家加载存档时，这个属性会被恢复到玩家游戏中的状态。
  /// </summary>
  [SerializeField]
  private int sunpoint;

  /// <summary>
  /// 获取阳光值
  /// </summary>
  public int SunPoint
  {
    get { return sunpoint; }
  }

  public float produceTime = 10f;//自然阳光产生时间
  private float produceTimer = 10;//自然阳光产生计时器
  public GameObject sunPerfab;//阳光预制体

  private bool isStartProduce = false;

  private void Start()
  {
    UpdateSunPointText();
    CalculateSunPointTextPosition();
    //StartProduceSun();
  }

  private void FixedUpdate()
  {
    if (isStartProduce)
      StartCoroutine(GameManger.Instance.WaitForSeconds(ProduceSun, 6));
  }
  /// <summary>
  /// 开始生成阳光
  /// </summary>
  public void StartProduceSun()
  {
    isStartProduce = true;
  }
  public void StopProduceSun()
  {
    isStartProduce = false;
  }
  /// <summary>
  /// 显示阳光文本
  /// </summary>
  public TextMeshProUGUI sunPointText;
  /// <summary>
  /// 阳光文本的坐标
  /// </summary>
  private Vector3 sunPointTextPosition;
  /// <summary>
  /// 更新阳光值
  /// </summary>
  private void UpdateSunPointText()
  {
    sunPointText.text = sunpoint.ToString();
  }

  /// <summary>
  /// 消耗阳光值方法
  /// </summary>
  public void SubtractSunPoint(int sunPoint)
  {
    sunpoint -= sunPoint;
    UpdateSunPointText();  //更新阳光值文本
  }
  /// <summary>
  /// 增加阳光值方法
  /// </summary>
  public void AddSunPoint(int sunPoint)
  {
    sunpoint += sunPoint;
    UpdateSunPointText();  //更新阳光值文本
  }
  /// <summary>
  /// 获取阳光值文本组件世界坐标
  /// </summary>
  public Vector3 GetSunPountTextPosition()
  {
    return sunPointTextPosition;
  }
  /// <summary>
  /// 计算阳光文本组件世界坐标
  /// </summary>
  private void CalculateSunPointTextPosition()
  {
    Vector3 position = Camera.main.ScreenToWorldPoint(sunPointText.transform.position);
    position.z = 0;
    sunPointTextPosition = position;
  }
  /// <summary>
  /// 产生自然阳光
  /// </summary>
  private void ProduceSun()
  {
    produceTimer += Time.deltaTime;

    if (produceTimer > produceTime)
    {
      produceTimer = 0;  //重置计时器
      //随机生成位置
      Vector3 position = new Vector3(Random.Range(0.5f, 14.5f), 7f, -1);
      //GameObject sun = Instantiate(sunPerfab, position, Quaternion.identity);
      Sun sun = BufferPoolManager.Instance.GetObj(sunPerfab).GetComponent<Sun>();//从对象池中获取对象
      StartCoroutine(GameManger.Instance.WaitForSeconds(sun.autoCollect, 6));
      sun.transform.position = position;//设置自然生成的阳光位置
      //随机移动位置
      position.y = Random.Range(-3.75f, 3f);
      sun.GetComponent<Sun>().LinearTo(position);
    }

  }
}
