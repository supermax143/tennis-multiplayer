using UnityEngine;

namespace Controllerrs.Bonus
{
  public class ScaleDownEffect : BonusEffectBase
  {
    private PlayerActor player;
    private Vector3 startScale;
    public override void Activate()
    {
      base.Activate();
      if (GameField.TryGetOpponentPlayer(Activator.Id, out player))
      {
        startScale = player.transform.localScale;
        var newScale = startScale;
        newScale.x /= 2;
        player.transform.localScale = newScale;
      }
    }

    public override void Deactivate()
    {
      if (player)
      {
        player.transform.localScale = startScale;
      }
      base.Deactivate();
    }
    
  }
}