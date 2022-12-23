using System;
using Models;
using Unity.Netcode;
using UnityEngine;

namespace Controllerrs.Bonus
{
  public class BonusView : NetworkBehaviour
  {
    public event Action<BonusView> OnCollect;
    
    [SerializeField]
    private BonusEffectBase.EffectType _effectType;
    [SerializeField]
    private float _effectLifeTime;

    public BonusEffectBase.EffectType EffectType => _effectType;
    public float EffectLifeTime => _effectLifeTime;
    public Player Player { get; private set; }

    public void Collect(Player player)
    {
      Player = player;
      OnCollect?.Invoke(this);
    }
    
  }
}