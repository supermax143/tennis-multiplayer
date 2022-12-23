using UnityEngine;

namespace Controllerrs.Bonus
{
  public class ScaleUpEffect : BonusEffectBase
  {
    private PlayerActor player;
    private Vector3 startScale;
    public override void Activate()
    {
      if (GameField.TryGetPlayer(Activator.Id, out player))
      {
        startScale = player.transform.localScale;
        var newScale = startScale;
        newScale.x *= 2;
        player.transform.localScale = newScale;
      }
      base.Activate();
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