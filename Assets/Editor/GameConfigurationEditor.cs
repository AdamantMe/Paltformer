using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameConfiguration))]
public class GameConfigurationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GameConfiguration config = (GameConfiguration)target;

        EditorGUILayout.LabelField("Scene Settings", EditorStyles.boldLabel);
        config.SceneOne = EditorGUILayout.TextField("Level One Scene", config.SceneOne);
        config.SceneTwo = EditorGUILayout.TextField("Level Two Scene", config.SceneTwo);
        config.SceneThree = EditorGUILayout.TextField("Level Three Scene", config.SceneThree);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Player Settings", EditorStyles.boldLabel);
        config.PlayerMaxHealth = EditorGUILayout.IntField("Player Max Health", config.PlayerMaxHealth);
        config.HealthDeductionOnFall = EditorGUILayout.IntField("Health Deduction On Fall", config.HealthDeductionOnFall);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Level One Settings", EditorStyles.boldLabel);
        config.MaximumCollectablesOnLevelOne = EditorGUILayout.IntField("Maximum Collectables", config.MaximumCollectablesOnLevelOne);
        config.DeathBorder = EditorGUILayout.FloatField("Death Border", config.DeathBorder);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Cube Generation Settings", EditorStyles.boldLabel);
        config.CubesToSpawn = EditorGUILayout.IntField("Cubes To Spawn", config.CubesToSpawn);
        config.CubeAreaSize = EditorGUILayout.Vector3Field("Cube Area Size", config.CubeAreaSize);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Energy Point Settings", EditorStyles.boldLabel);
        config.EnergyPointsToSpawn = EditorGUILayout.IntField("Energy Points To Spawn", config.EnergyPointsToSpawn);
        config.EnergyPointSpawnAreaFactor = EditorGUILayout.FloatField("Energy Point Spawn Area Factor", config.EnergyPointSpawnAreaFactor);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(config);
        }
    }
}
