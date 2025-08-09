using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    private Cinemachine.CinemachineVirtualCamera virtualCamera;
    List<GameObject> codexPieces = new List<GameObject>();
    int sceneToRespawn;

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
    public void AddCodexPiece(GameObject piece)
    {
        codexPieces.Add(piece);
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
