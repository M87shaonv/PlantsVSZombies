using UnityEngine;

public class BalloonZombie : Zombie
{
  // 三种状态:
  // 0. 飞行状态,无法被普通植物攻击
  // 1. 爆炸状态,播放动画,停止移动
  // 2. 步行状态,可以被普通植物攻击
  //死亡和攻击状态的切换由Zombie类实现

  //子弹使用GameObject.FindWithTag查找它的SkyZombie标签
  //如果攻击到则转换ChangeObjectTag("Zombie");转换标签

}
