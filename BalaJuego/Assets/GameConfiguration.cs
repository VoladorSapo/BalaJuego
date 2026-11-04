using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfiguration", menuName = "ScriptableObject/GameConfiguration")]

public class GameConfiguration : ScriptableObject
{
    [field: SerializeField] public bool canStopTimeWithGunFull { get; private set; }

    [field: SerializeField] public float slowDownTimeMagnitude { get; private set; }
    [field: SerializeField] public float slowDownTimeMagnitudeAgatha { get; private set; }


    [field:SerializeField] public List<IGameState.gameState> interactableGameStates { get; private set; }
    [field: SerializeField] public List<IGameState.gameState> shootAndUseEquipmentStates { get; private set; }

    [field: SerializeField] public List<IGameState.gameState> stopTimeStates { get; private set; }




}