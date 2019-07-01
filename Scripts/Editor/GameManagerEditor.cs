using UnityEditor;

using UnityEngine;

// Custom editor for all game manager settings
[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    // Editor references
    public GameManager gm;
    public Editor baseSettingsEditor;
    public Editor heightmapNoiseSettingsEditor;
    public Editor temperatureNoiseSettingsEditor;
    public Editor humidityNoiseSettingsEditor;
    public Editor sedimentNoiseSettingsEditor;
    public Editor stoneNoiseSettingsEditor;
    public Editor cutoffSettingsEditor;
    public Editor tileSettingsEditor;

    // Set up settings editor windows while editor is open in inspector
    public override void OnInspectorGUI()
    {
        using(EditorGUI.ChangeCheckScope check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if(check.changed)
            {
                this.gm.GenerateNewWorld();
            }
        }
        if(GUILayout.Button("Randomize Seeds/Regenerate World"))
        {
            this.gm.RandomizeSeeds();
        }
        if(GUILayout.Button("Reset/Spawn New Lifeforms"))
        {
            this.gm.SpawnLifeforms();
        }
        if(GUILayout.Button("Write Log to File"))
        {
            this.gm.WriteLogToFile();
        }
        this.DrawSettingsEditor(this.gm.baseSettings, this.gm.GenerateNewWorld, ref this.gm.baseSettingsFoldout, ref this.baseSettingsEditor);
        this.DrawSettingsEditor(this.gm.heightmapNoiseSettings, this.gm.GenerateNewWorld, ref this.gm.heightmapNoiseSettingsFoldout, ref this.heightmapNoiseSettingsEditor);
        this.DrawSettingsEditor(this.gm.temperatureNoiseSettings, this.gm.GenerateNewWorld, ref this.gm.temperatureNoiseSettingsFoldout, ref this.temperatureNoiseSettingsEditor);
        this.DrawSettingsEditor(this.gm.humidityNoiseSettings, this.gm.GenerateNewWorld, ref this.gm.humidityNoiseSettingsFoldout, ref this.humidityNoiseSettingsEditor);
        this.DrawSettingsEditor(this.gm.sedimentNoiseSettings, this.gm.GenerateNewWorld, ref this.gm.sedimentNoiseSettingsFoldout, ref this.sedimentNoiseSettingsEditor);
        this.DrawSettingsEditor(this.gm.stoneNoiseSettings, this.gm.GenerateNewWorld, ref this.gm.stoneNoiseSettingsFoldout, ref this.stoneNoiseSettingsEditor);
        this.DrawSettingsEditor(this.gm.cutoffSettings, this.gm.GenerateNewWorld, ref this.gm.cutoffSettingsFoldout, ref this.cutoffSettingsEditor);
        this.DrawSettingsEditor(this.gm.tileSettings, this.gm.GenerateNewWorld, ref this.gm.tileSettingsFoldout, ref this.tileSettingsEditor);
    }

    // Draw settings editors
    private void DrawSettingsEditor(Object _settings, System.Action _onSettingsUpdated, ref bool _foldout, ref Editor _editor)
    {
        if(_settings != null)
        {
            _foldout = EditorGUILayout.InspectorTitlebar(_foldout, _settings);
            using(EditorGUI.ChangeCheckScope check = new EditorGUI.ChangeCheckScope())
            {
                if(_foldout == true)
                {
                    CreateCachedEditor(_settings, null, ref _editor);
                    _editor.OnInspectorGUI();
                    if(check.changed)
                    {
                        if(_onSettingsUpdated != null)
                        {
                            _onSettingsUpdated();
                        }
                    }
                }
            }
        }
    }

    // Ran when script is enabled
    private void OnEnable()
    {
        this.gm = (GameManager)this.target;
    }
}
