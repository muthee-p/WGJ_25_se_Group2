using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    [SerializeField] private List<Image> codexPieces = new List<Image>();
    [SerializeField] private GameObject cancleButton, codexPanel, startScreen;
    [SerializeField] private TextMeshProUGUI codexText, researchLogText;
    public List<GameObject> deadEnemies = new List<GameObject>();
    public HashSet<String> foundCodexPieces = new HashSet<String>();
    public HashSet<String> killedEnemies = new HashSet<String>();
    public bool enemyTutotialShown = false;
    private Cinemachine.CinemachineVirtualCamera virtualCamera;
    int sceneToRespawn, codexPieceIndex = 0;
    bool allCodexPiecesCollected = false;
    Vector3 codexPanelScale;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ChangeScene();
        codexPanelScale = codexPanel.transform.localScale;
    }

    public void ChangeScene()
    {
        virtualCamera = GameObject.FindWithTag("VirtualCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>();

        if (virtualCamera != null)
        {
            virtualCamera.LookAt = GameObject.FindWithTag("Player").transform;
            virtualCamera.Follow = GameObject.FindWithTag("Player").transform;
        }
        GameObject.FindWithTag("Player").transform.position = GameObject.FindWithTag("PlayerSpawnPoint").transform.position;

        if (enemyTutotialShown) DestroyTutorial();
    }

    public void DestroyTutorial()
    {
        GameObject enemyTutotial = GameObject.Find("EnemyTutorial");
        if (enemyTutotial != null)
        {
            Destroy(enemyTutotial);
        }
    }

    #region Codex
    public void ShowCodexPiece(string text, string researchText)
    {
        if (codexPieceIndex == codexPieces.Count)
        {
            allCodexPiecesCollected = true;
            return;
        }
        CharacterController.instance.Pause();

        codexPanel.SetActive(true);
        codexPanel.transform.localScale = Vector3.zero;
        codexPanel.transform.DOScale(codexPanelScale, 0.4f).SetEase(Ease.OutBack);

        codexText.text = text;
        researchLogText.text = researchText;
        Color color = codexPieces[codexPieceIndex].color;
        color.a = 1;
        codexPieces[codexPieceIndex].color = color;
        codexPieceIndex = codexPieceIndex + 1;
    }

    #endregion

    public void UpdateLastCheckpoint()
    {
        sceneToRespawn = SceneManager.GetActiveScene().buildIndex;
    }

    public void RespawnPlayer()
    {
        SceneManager.LoadScene(sceneToRespawn);
        GameObject.FindWithTag("Player").transform.position = GameObject.FindWithTag("Checkpoint").transform.position;
        CharacterController.instance.ResetStates();
    }

    public void AddDeadEnemy(GameObject enemy)
    {
        deadEnemies.Add(enemy);
    }

    public void DestroyDeadEnemies()
    {
        for (int i = deadEnemies.Count - 1; i >= 0; i--)
        {
            if (deadEnemies[i] == null)
            {
                deadEnemies.RemoveAt(i);
            }
        }
    }

    public void IsGameWon()
    {
        if (allCodexPiecesCollected)
        {
            HealthScript.instance.YouWon("You have won!");
        }
    }
    #region UIButtons

    public void CancleButton()
    {
        codexPanel.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack)
        .OnComplete(() => codexPanel.gameObject.SetActive(false));

        CharacterController.instance.Unpause();
    }

    public void StartGame()
    {
        startScreen.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack)
        .OnComplete(() => startScreen.gameObject.SetActive(false));

        CharacterController.instance.Unpause();
        HealthScript.instance.RestartRoutineDrainHealth();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion
}
