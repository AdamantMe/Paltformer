using UnityEngine;

[CreateAssetMenu(fileName = "GameConfiguration", menuName = "Game Configuration/GameConfiguration")]
public class GameConfiguration : ScriptableObject
{
    public string SceneOne = "LevelOne";
    public string SceneTwo = "LevelTwo";
    public string SceneThree = "LevelThree";

    public int PlayerMaxHealth = 100;
    public int HealthDeductionOnFall = 40;

    public int MaximumCollectablesOnLevelOne = 5;
    public float MinimumDistanceBetweenEnergyPoints = 14f;
    public float DeathBorder = -10f;


    public int CubesToSpawn = 1500;
    public int EnergyPointsToSpawn = 1; //TODO change back
    public Vector3 CubeAreaSize = new Vector3(5, 15, 5);
    public float EnergyPointSpawnAreaFactor = 0.8f;

    public int HealthLostPerSecond = 1;
}