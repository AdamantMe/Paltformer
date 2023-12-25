using UnityEngine;

[CreateAssetMenu(fileName = "GameConfiguration", menuName = "Game Configuration/GameConfiguration")]
public class GameConfiguration : ScriptableObject
{
    public string SceneOne = "LevelOne";
    public string SceneTwo = "LevelTwo";
    public string SceneThree = "LevelThree";

    public int PlayerMaxHealth = 100;
    public int HealthDeductionOnFall = 40;

    public int MaximumCollectablesOnLevelOne = 10;
    public float DeathBorder = -10f;


    public int CubesToSpawn = 1500;
    public int EnergyPointsToSpawn = 10;
    public Vector3 CubeAreaSize = new Vector3(5, 15, 5);
    public float EnergyPointSpawnAreaFactor = 0.8f;
}