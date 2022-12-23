using System;
using Game;
using Models;
using UnityEditor.VersionControl;
using Task = System.Threading.Tasks.Task;

namespace Controllerrs.Bonus
{
  public class BonusEffectBase
  {
    public event Action<BonusEffectBase> OnDeactivated;
    public enum EffectType
    {
      ScaleUpActivator, ScaleDownOpponent, SpeedupBall
    }

    public float LifeTime { get; private set; }
    public Player Activator { get; private set; }
    public GameField GameField { get; private set; }
    
    public virtual void Activate()
    {
      WaitLifetime();
    }

    public virtual void Deactivate()
    {
      OnDeactivated?.Invoke(this);
    }
    
    private async Task WaitLifetime()
    {
      int timeInMilliseconds = (int)(LifeTime * 1000);
      await Task.Delay(timeInMilliseconds);
      Deactivate();
    }
    
    public static BonusEffectBase Create(EffectType type, Player activator, float lifeTime, GameField gameField)
    {
      BonusEffectBase effect = default;
      switch (type)
      {
        case EffectType.ScaleUpActivator:
          effect = new ScaleUpEffect();
          break;
        case EffectType.ScaleDownOpponent:
          effect = new ScaleDownEffect();
          break;
        case EffectType.SpeedupBall:
          effect = new SpeedUpBallEffect();
          break;
      }

      effect.LifeTime = lifeTime;
      effect.Activator = activator;
      effect.GameField = gameField;
      
      return effect;
    }
  }
}