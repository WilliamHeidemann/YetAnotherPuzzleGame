using System;
using Systems;
using TMPro;
using UnityEngine;

public class WorldButtonDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI trialWorldText;
    [SerializeField] private TextMeshProUGUI worldOneText;
    [SerializeField] private TextMeshProUGUI frogWorldText;

    private void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        var trialLevelsCompleted = LevelManager.Instance.GetLevelsCompleted(LevelManager.World.Trial).ToString();
        var trialLevels = LevelManager.Instance.GetLevelsTotal(LevelManager.World.Trial).ToString();
        
        var worldOneLevelsCompleted = LevelManager.Instance.GetLevelsCompleted(LevelManager.World.One).ToString();
        var worldOneLevels = LevelManager.Instance.GetLevelsTotal(LevelManager.World.One).ToString();

        var frogWorldLevelsCompleted = LevelManager.Instance.GetLevelsCompleted(LevelManager.World.Frog).ToString();
        var frogLevels = LevelManager.Instance.GetLevelsTotal(LevelManager.World.Frog).ToString();

        trialWorldText.text = $"{trialLevelsCompleted} / {trialLevels}";
        worldOneText.text = $"{worldOneLevelsCompleted} / {worldOneLevels}";
        frogWorldText.text = $"{frogWorldLevelsCompleted} / {frogLevels}";
    }
}
