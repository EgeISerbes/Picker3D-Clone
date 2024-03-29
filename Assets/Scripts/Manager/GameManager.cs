﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CameraFollow _cameraFollow;
    [SerializeField] private MainChar _mainChar;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private Transform _startPos;
    [SerializeField] private Transform _charStartPos;
    private string _savePath;

    [SerializeField] private float _endGameWaitSeconds;
    [SerializeField] private float _restartGameWaitSeconds;
    [SerializeField] private float _waitTimeForKinematic = 1f;
    private void Awake()
    {
        _savePath = Application.persistentDataPath + "/saveData.json";
        _cameraFollow.Init(_mainChar);
        _mainChar.Init(GameFinished);
        _uiManager.Init(GameStartState);
        _uiManager.SetPoint(_levelManager._saveData.points);
        _levelManager.ClearLevels();
        _levelManager.ClearSave();
        _levelManager.AddLevelsToScene();
        _levelManager.GetLevelData(_savePath);
        _levelManager.SetPositions(out _startPos, out _charStartPos);
        OnStart();

    }

    void OnStart()
    {

        _levelManager.LoadLevelatPos(new Pose(_startPos.position, _startPos.rotation), true);
        var obj = _levelManager.GetCurrentLevel();
        _levelManager.LoadLevelatPos(new Pose(obj.levelSpawnPos.position, obj.levelSpawnPos.rotation), false);
        _uiManager.SetLevelCount(_levelManager.CurrentLevelNumber);
        SetCharacterPosition();

        //_levelManager.SetPositions(out _startPos, out _charStartPos);
    }
    void GameFinished(bool hasWon, int points)
    {
        if (hasWon)
        {
            StartCoroutine(EndGameSequence());
            _uiManager.SetPoint(points);
            _levelManager._saveData.points += points;
        }
        else
        {

            StartCoroutine(RestartSequence());
        }
    }

    void SetCharacterPosition()
    {
        _mainChar.transform.position = _charStartPos.position;
        _mainChar.transform.rotation = _charStartPos.rotation;
    }
    void GameStartState(bool hasStarted)
    {
        if (hasStarted)
        {
            _mainChar.StartState();
        }
        else
        {
            RestartGame();
        }
    }

    void RestartGame()
    {
        _levelManager.SaveLevelData(_savePath);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

    IEnumerator RestartSequence()
    {
        yield return new WaitForSeconds(_restartGameWaitSeconds);
        RestartGame();
    }
    IEnumerator EndGameSequence()
    {
        _levelManager.SetNextLevel();
        _levelManager.SetPositions(out _startPos, out _charStartPos);
        yield return new WaitForSeconds(_waitTimeForKinematic);
        _mainChar.SetTargetPos(_charStartPos);
        _mainChar.SetRestartState();
        yield return new WaitForSeconds(_endGameWaitSeconds);
        _levelManager.DestroyOldCurrentLevel();
        _uiManager.SetStartMenu();
        OnStart();
    }
}
