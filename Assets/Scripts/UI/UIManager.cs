using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _playButton;
    [SerializeField] private GameObject _pauseButton;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _blurOBJ;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _points;
    private System.Action<bool> GameStartState;
    private void Awake()
    {
        SetStartMenu();
    }

    public void SetStartMenu()
    {
        _playButton.SetActive(true);
        _pauseButton.SetActive(false);
        _pauseMenu.SetActive(false);
        _blurOBJ.SetActive(true);
        Time.timeScale = 1f;
    }
    public void Init(System.Action<bool> GameStartState)

    {
        this.GameStartState = GameStartState;
    }
    public void GameStarted()
    {
        _playButton.SetActive(false);
        _pauseButton.SetActive(true);
        _blurOBJ.SetActive(false);
        GameStartState(true);
    }

    public void GamePaused()
    {
        _pauseButton.SetActive(false);
        _pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void GameResumed()
    {
        _pauseButton.SetActive(true);
        _pauseMenu.SetActive(false);
        Time.timeScale = 1f;

    }
    public void GameRestarted()
    {
        Time.timeScale = 1f;
        _playButton.SetActive(true);
        _pauseMenu.SetActive(false);
        GameStartState(false);
    }

    public void SetLevelCount(int level)
    {
        _levelText.SetText("Level " + level.ToString());
    }

    public void SetPoint(int point)
    {
        _points.SetText(point.ToString());
    }
}
