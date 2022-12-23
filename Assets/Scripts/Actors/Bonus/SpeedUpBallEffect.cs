namespace Controllerrs.Bonus
{
  public class SpeedUpBallEffect : BonusEffectBase
  {
    private BallActor ball;
    
    public override void Activate()
    {
      base.Activate();
      ball = GameField.Ball;
      ball.Rigidbody.velocity *= 2;
    }

    public override void Deactivate()
    {
      ball.Rigidbody.velocity /= 2;
      base.Deactivate();
    }
  }
}