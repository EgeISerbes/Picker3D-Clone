using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public int currentLevelIndex = 0;
    public int nextLevelIndex = 1;
    public int currentLevelNumber = 1;
    public int points = 0;

    public void Init()
    {
        currentLevelIndex = 0;
        nextLevelIndex = 1;
        currentLevelNumber = 1;
        points = 0;
    }
}

[CreateAssetMenu(fileName = "Data", menuName = "LevelManager", order = 1)]
public class LevelManager : ScriptableObject
{
    [SerializeField] private List<LevelObj> _levels = new List<LevelObj>();
    [SerializeField] private List<LevelObj> _spawnedLevels = new List<LevelObj>();
    private LevelObj _currentLevel, _nextLevel;
    public SaveData _saveData = new SaveData();
    private string _dataText = string.Empty;
    public int CurrentLevelNumber
    {
        get => _saveData.currentLevelNumber;
    }
    public void GetLevelData(string savePath)
    {
        _dataText = string.Empty;
       if(!CheckFileAvailability(savePath)) return;
        _dataText = File.ReadAllText(savePath);
        JsonUtility.FromJsonOverwrite(_dataText, _saveData);
    }
    public void ClearSave()
    {
        _saveData.Init();
    }
    public void ClearLevels()
    {
        _spawnedLevels.Clear();
    }
    public void ClearData()
    {
        ClearLevels();
        var _path = Application.persistentDataPath + "/saveData.json";
        CheckFileAvailability(_path);
        File.Delete(_path);
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

    private bool CheckFileAvailability(string savePath)
    {
        if (File.Exists(savePath)) return true;
        else File.Create(savePath); return false;

    }

    public LevelObj GetCurrentLevel()
    {
        return _spawnedLevels[_saveData.currentLevelIndex];
    }
    public void AddLevelsToScene()
    {
        foreach (LevelObj level in _levels)
        {
            var obj = Instantiate(level);
            _spawnedLevels.Add(obj);
            obj.gameObject.SetActive(false);

        }
    }

    public void DestroyOldCurrentLevel()
    {
        Destroy(_currentLevel.gameObject);
    }
    public void SetNextLevel()
    {
        var objToDestroy = _spawnedLevels[_saveData.currentLevelIndex].gameObject.GetComponent<LevelObj>();
        _currentLevel = objToDestroy;
        var index = _spawnedLevels.IndexOf(objToDestroy);
        var objToSpawn = Instantiate(_levels[_saveData.currentLevelIndex].gameObject).GetComponent<LevelObj>();
        _spawnedLevels.RemoveAt(index);
        _spawnedLevels.Insert(index, objToSpawn);
        _spawnedLevels[_saveData.currentLevelIndex].gameObject.SetActive(false);
        _saveData.currentLevelIndex = _saveData.nextLevelIndex;
        _saveData.currentLevelNumber += 1;
        if (_saveData.currentLevelNumber >= _spawnedLevels.Count)
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
