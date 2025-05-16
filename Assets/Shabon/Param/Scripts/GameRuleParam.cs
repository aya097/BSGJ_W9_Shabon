#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameRule", menuName = "ScriptableObjects/CreateGameRule")]
public class GameRuleParam : ScriptableObject
{
    [Header("フェーズに関するパラメータ（上から1,2,3...）")]
    public List<GamePhaseData> gamePhaseDataList;
}

[Serializable]
public class GamePhaseData
{
    [Header("一度に生成されるバブルの数")]
    [Min(0)]
    public int BubblesPerSpawn;

    [Header("バブルの生成間隔")]
    [Min(0)]
    public float SpawnBubbleInterval;

    [Header("バブルのステージ上の最大数")]
    [Min(0)]
    public int MaxBabbleOnField;
}
