using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OldWallLamp : Plant
{
  public float length = 0;
  public float width = 0;
  public Vector2 boxSize; //长方形检测范围的尺寸
  public Light2D light2d;
  public float minIntensity = 0.8f;//最小亮度
  public float maxIntensity = 1.2f;//最大亮度
  public float breathDuration = 2.5f;//呼吸周期
  float AddBloodTime = 5; // 加血时间
  public float AddBloodTimer = 0; // 加血计时器

  protected override void OnEnable()//TODO 未测试加血特效
  {
    base.OnEnable();
    AlterHP = HP;
    AddBloodTimer = 0;
  }
  void Awake()
  {
    light2d = transform.Find("Light").GetComponent<Light2D>();
    boxSize = new Vector2(length, width);
  }
  private float elapsedTime = 0f; // 经过的时间
  private bool isBreathingIn = true; // 是否增加亮度

  void Breathe()
  {
    if (AlterHP > 0)
    {
      float HPpercent = AlterHP / HP; // 计算生命值百分比
      float startIntensity = minIntensity * HPpercent;
      float targetIntensity = maxIntensity * HPpercent;

      elapsedTime += Time.deltaTime;

      if (isBreathingIn)
      {
        // 增加光强
        light2d.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsedTime / (breathDuration / 2));
        if (elapsedTime >= breathDuration / 2)
        {
          isBreathingIn = false;
          elapsedTime = 0;
        }
      }
      else
      {
        // 减少光强
        light2d.intensity = Mathf.Lerp(targetIntensity, startIntensity, elapsedTime / (breathDuration / 2));
        if (elapsedTime >= breathDuration / 2)
        {
          isBreathingIn = true;
          elapsedTime = 0;
        }
      }
    }
  }
  protected override void EnableUpdate()
  {
    Breathe();
    AddBloodTimer += Time.deltaTime;
    if (AddBloodTimer >= AddBloodTime)
    {
      CheckNearbyPlant();
      AddBloodTimer = 0;
    }
  }
  public override void Die()
  {
    base.Die();
    StopAllCoroutines();
    BufferPoolManager.Instance.PushObj(PlantManger.Instance.plantType[(int)PlantTypes.OldWallLamp], this.gameObject);

  }
  void CheckNearbyPlant()//检测周围植物
  {
    Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, boxSize, 0f);// 执行长方形范围检测
    foreach (Collider2D collider in colliders)// 遍历检测到的植物
    {
      if (collider.CompareTag("Plant") && collider.GetComponent<Plant>().plantType != this.plantType) // 确保检测到的不是自己
      {
        Plant nearbyPlant = collider.GetComponent<Plant>();
        if (nearbyPlant != null)
        {
          collider.GetComponent<Plant>().AddBlood(20);
        }
      }
    }
  }
#if TEXTING
  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    Gizmos.DrawWireCube(transform.position, boxSize);
  }
#endif

  //! 协程虽好,但这里的呼吸效果跟随生命值变化导致debug打印值和编辑器内值不一致,Update就可以
  // IEnumerator Breathe()//使用光照强度的改变表示呼吸
  // {
  //   Debug.Log("呼吸开始");
  //   float t = 0f; // 初始化线性插值参数
  //   float elapsedTime = 0f; // 记录已经过去的时间
  //   float HPpercent = AlterHP / HP; // 生命值百分比
  //   float startIntensity = minIntensity * HPpercent; // 呼吸开始时的光照强度
  //   float targetIntensity = maxIntensity * HPpercent; // 呼吸结束时的光照强度
  //   while (AlterHP > 0)
  //   {
  //     while (elapsedTime < breathDuration / 2f) // 在呼吸周期的前一半时间内执行
  //     {
  //       elapsedTime += Time.deltaTime; // 增加已经过去的时间
  //       t = elapsedTime / (breathDuration / 2f); // 计算线性插值参数
  //       light2d.intensity = Mathf.Lerp(startIntensity, targetIntensity, t); // 根据线性插值参数更新光照强度
  //       Debug.Log("光照强度：" + light2d.intensity);
  //       yield return null; // 等待一帧
  //     }

  //     // 交换起始和目标强度，实现呼吸效果
  //     float temp = startIntensity; // 临时变量用于交换强度数值
  //     startIntensity = targetIntensity; // 将目标强度赋值给起始强度
  //     targetIntensity = temp; // 将起始强度赋值给目标强度

  //     elapsedTime = 0f; // 重置已经过去的时间
  //     while (elapsedTime < breathDuration / 2f) // 在呼吸周期的后一半时间内执行
  //     {
  //       elapsedTime += Time.deltaTime; // 增加已经过去的时间
  //       t = elapsedTime / (breathDuration / 2f); // 计算线性插值参数
  //       light2d.intensity = Mathf.Lerp(startIntensity, targetIntensity, t); // 根据线性插值参数更新光照强度
  //       yield return null; // 等待一帧
  //     }

  //     yield return null; // 在两次呼吸之间等待一帧
  //   }
  // }
}