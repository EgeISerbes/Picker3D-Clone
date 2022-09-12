using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public int currentLevelIndex;
    public int nextLevelIndex;
    public int currentLevelNumber = 1;
}

[CreateAssetMenu(fileName = "Data", menuName = "LevelManager", order = 1)]
public class LevelManager : ScriptableObject
{
    [SerializeField] private List<LevelObj> _levels = new List<LevelObj>();
    private List<LevelObj> _spawnedLevels = new List<LevelObj>();
    private SaveData _saveData = new SaveData();
    private string _dataText = string.Empty;
    public void GetLevelData(string savePath)
    {
        _dataText = string.Empty;
        CheckFileAvailability(savePath);
        _dataText = File.ReadAllText(savePath);
        JsonUtility.FromJsonOverwrite(_dataText, _saveData);
    }

    public void ClearLevels()
    {
        _spawnedLevels.Clear();
    }
    public void SaveLevelData(string savePath)
    {
        _dataText = string.Empty;
        _dataText = JsonUtility.ToJson(_saveData);
        CheckFileAvailability(savePath);
        File.WriteAllText(savePath, _dataText);
    }


    public void LoadLevelatPos(Pose pos, bool isCurrent)
    {
        var index = (isCurrent) ? _saveData.currentLevelIndex : _saveData.nextLevelIndex;
        var obj = _spawnedLevels[index];
        obj.gameObject.SetActive(true);
        obj.transform.position = pos.position;
        obj.transform.rotation = pos.rotation;
    }

    private void CheckFileAvailability(string savePath)
    {
        if (File.Exists(savePath)) return;
        else File.Create(savePath);

    }


    public void AddLevelsToScene()
    {
        foreach (LevelObj level in _levels)
        {
            var obj = Instantiate(level);
            obj.gameObject.SetActive(false);
            _spawnedLevels.Add(obj);

        }
    }

    public void SetNextLevel()
    {
        _spawnedLevels[_saveData.currentLevelIndex].gameObject.SetActive(false);
        _saveData.currentLevelIndex = _saveData.nextLevelIndex;
        _saveData.currentLevelNumber += 1;
        if(_saveData.currentLevelNumber>_spawnedLevels.Count)
        {
            var randomIndex = Random.Range(0, _spawnedLevels.Count - 1);
            _saveData.nextLevelIndex = randomIndex;
        }
        else
        {
            _saveData.nextLevelIndex = _saveData.currentLevelNumber;
        }
    }
    public void SetPositions(out Transform startPos, out Transform charStartPos)
    {
        startPos = _spawnedLevels[_saveData.currentLevelIndex].levelSpawnPos;
        charStartPos = _spawnedLevels[_saveData.currentLevelIndex].charStartPos;

    }



}
