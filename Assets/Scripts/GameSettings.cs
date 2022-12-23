using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [SerializeField]
    private int _maxScoreCount;
    [SerializeField]
    private int _bonusSpawnDelay;
    [SerializeField]
    private int _ballStartSpeed;
    
    public int MaxScoreCount => _maxScoreCount;
    public int BonusSpawnDelay => _bonusSpawnDelay;
    public int BallStartSpeed => _ballStartSpeed;
}
