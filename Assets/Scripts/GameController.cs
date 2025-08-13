using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.TextCore.Text;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    [SerializeField] private List<Image> codexPieces = new List<Image>();
    [SerializeField] private GameObject cancleButton, codexPanel, startScreen, pauseScreen,settingsPanel, credisPanel, gameWonScreen;
    [SerializeField] private TextMeshProUGUI codexText, researchLogText;
    [SerializeField] private TMP_FontAsset normalFont, distortedFont;
    [SerializeField] Toggle musicToggle;
    public List<String> codexMessages = new List<String>();
    public List<String> codexTitles = new List<String>();
    public HashSet<String> foundCodexPieces = new HashSet<String>();
    public HashSet<String> killedEnemies = new HashSet<String>();
    public bool enemyTutotialShown = false;
    private Cinemachine.CinemachineVirtualCamera virtualCamera;
    int sceneToRespawn, codexPieceIndex = 0;
    Vector3 codexPanelScale, pauseScreenScale, settingsPanelScale, credisPanelScale, gameWonScreenScale;
    AudioSource audioSource;

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
        pauseScreenScale = pauseScreen.transform.localScale;
        settingsPanelScale = settingsPanel.transform.localScale;
        credisPanelScale = credisPanel.transform.localScale;
        gameWonScreenScale = gameWonScreen.transform.localScale;
        audioSource = GetComponent<AudioSource>();
        musicToggle.isOn = audioSource.isPlaying;
        musicToggle.onValueChanged.AddListener(MusicToggle);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && CharacterController.instance.gameState == CharacterController.GameState.Playing)
        {
            CharacterController.instance.Pause();
            pauseScreen.SetActive(true);
            pauseScreen.transform.DOScale(pauseScreenScale, 0.4f).SetEase(Ease.OutBack);
        }
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
    public void ShowCodexPiece(string text, string researchText, bool shouldAdd = false)
    {
        if (codexPieceIndex == codexPieces.Count-1)
        {
            HealthScript.instance.StartShowMessage("You have found all the codex pieces!", 4f); //StartCoroutine(ShowMessage("You are getting low on health!", 4f));
            return;
        }
        CharacterController.instance.Pause();

        codexPanel.SetActive(true);
        codexPanel.transform.localScale = Vector3.zero;
        codexPanel.transform.DOScale(codexPanelScale, 0.4f).SetEase(Ease.OutBack);

        if (HealthScript.instance.currentHealth > 3000) codexText.font = distortedFont;
        else codexText.font = normalFont;

        codexText.text = text;
        researchLogText.text = researchText;

        if (shouldAdd)
        {
            Color color = codexPieces[codexPieceIndex].color;
            color.a = 1;
            codexPieces[codexPieceIndex].color = color;
            codexPieceIndex = codexPieceIndex + 1;
        }
    }


    public void AddCodexMessage(string message)
    {
        codexMessages.Add(message);
    }

    public void AddCodexTitle(string title)
    {
        codexTitles.Add(title);
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


    public void IsGameWon()
    {
        if (codexPieces.Count == 5 && killedEnemies.Count == 5)
        {
            HealthScript.instance.StopRoutineDrainHealth();
            CharacterController.instance.Faint();

            gameWonScreen.SetActive(true);
            gameWonScreen.transform.DOScale(gameWonScreenScale, 0.4f).SetEase(Ease.OutBack);

        }
    }
#region UIButtons

public void CancleButton()
    {
        codexPanel.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack)
        .OnComplete(() => codexPanel.gameObject.SetActive(false));

        CharacterController.instance.Unpause();
    }

    public void ResumeButton()
    {
        pauseScreen.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack)
        .OnComplete(() => pauseScreen.gameObject.SetActive(false));
        CharacterController.instance.Unpause();
    }

    public void SettingsButton()
    {
        settingsPanel.SetActive(true);
        settingsPanel.transform.DOScale(settingsPanelScale, 0.4f).SetEase(Ease.OutBack);
    }

    public void BackSettingsButton()
    {
        settingsPanel.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack)
        .OnComplete(() => settingsPanel.gameObject.SetActive(false));
    }

    public void CreditsButton()
    {
        credisPanel.SetActive(true);
        credisPanel.transform.DOScale(credisPanelScale, 0.4f).SetEase(Ease.OutBack);
    }

    public void BackCreditsButton()
    {
        credisPanel.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack)
        .OnComplete(() => credisPanel.gameObject.SetActive(false));
    }
    public void MusicToggle(bool isOn)
    {
        if (isOn)
        {
            audioSource.Play();
            audioSource.loop = true;
        }
        else
        {
            audioSource.Stop();
            audioSource.loop = false;
        }
            
    }

    public void RestartButton()
    {
        HealthScript.instance.RestartLevel();
        pauseScreen.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack)
        .OnComplete(() => pauseScreen.gameObject.SetActive(false));
        gameWonScreen.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack)
        .OnComplete(() => gameWonScreen.gameObject.SetActive(false));
        killedEnemies.Clear();
        foundCodexPieces.Clear();
        codexMessages.Clear();
        SceneManager.LoadScene(0);
        for (int i = 0; i < codexPieces.Count; i++)
        {
            Color color = codexPieces[i].color;
            color.a = 0.2f;
            codexPieces[i].color = color;
        }
        GlitchController.instance.SetNoise(0.1f);
        GlitchController.instance.SetGlitchStrength(0.1f);
        GlitchController.instance.SetScanLines(0.8f);
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
