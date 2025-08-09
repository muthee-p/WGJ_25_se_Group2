using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    [SerializeField] private List<Image> codexPieces = new List<Image>();
    private Cinemachine.CinemachineVirtualCamera virtualCamera;
    int sceneToRespawn, codexPieceIndex = 0;

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
    }

    public void ChangeScene()
    {
        virtualCamera = GameObject.FindWithTag("VirtualCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>();

        if (virtualCamera != null)
        {
            virtualCamera.LookAt = GameObject.FindWithTag("Player").transform;
            virtualCamera.Follow = GameObject.FindWithTag("Player").transform;
        }
    }
    public void ShowCodexPiece()
    {
        if (codexPieceIndex == codexPieces.Count-1)
        {
            return;
        }
        codexPieceIndex = codexPieceIndex + 1;
        Color color = codexPieces[codexPieceIndex].color;
        color.a = 1;
        codexPieces[codexPieceIndex].color = color;
    }

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
}
