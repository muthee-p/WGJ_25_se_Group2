using System.Collections;
using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    private Cinemachine.CinemachineVirtualCamera virtualCamera;

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
        DontDestroyOnLoad(GameObject.FindWithTag("Player"));
        ChangeScene();
    }

    void Update()
    {

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

}
