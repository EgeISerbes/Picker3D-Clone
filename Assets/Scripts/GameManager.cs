using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CameraFollow _cameraFollow;
    [SerializeField] private MainChar _mainChar;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private Transform _startPos;
    [SerializeField] private Transform _charStartPos;
    private string _savePath;
    private void Awake()
    {
        _savePath = Application.persistentDataPath + "/saveData.json"; 
        _cameraFollow.Init(_mainChar);
        _mainChar.Init(GameFinished);
        _uiManager.Init(GameStartState);
        _levelManager.ClearLevels();
        _levelManager.AddLevelsToScene();
        _levelManager.SetPositions(out _startPos, out _charStartPos);
        OnStart();
        
    }

    void OnStart()
    {
        
        _levelManager.LoadLevelatPos(new Pose(_startPos.position, _startPos.rotation),true);
        _mainChar.transform.position = _charStartPos.position;
        _mainChar.transform.rotation = _charStartPos.rotation;
        //_levelManager.SetPositions(out _startPos, out _charStartPos);
    }
    void GameFinished(bool hasWon)
    {
        if (hasWon)
        {
            _levelManager.SetNextLevel();
            _levelManager.SetPositions(out _startPos, out _charStartPos);
            _uiManager.SetStartMenu();
            OnStart();
        }
        else
        {
            OnStart();
        }
    }

    void GameStartState(bool hasStarted)
    {

    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            _levelManager.SaveLevelData(_savePath);
        }
    }
    private void OnApplicationQuit()
    {
        _levelManager.SaveLevelData(_savePath);
    }

}
