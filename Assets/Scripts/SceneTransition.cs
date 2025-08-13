using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public int sceneNumber;
    GameObject fadeinPanel;
    bool canChangeScene = false;

    void Start()
    {
        fadeinPanel = GameObject.Find("FadeInPanel");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canChangeScene)
        {
            EnterLevel();
            
            if (gameObject.name == "Exit")
            {
                GameController.instance.IsGameWon();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canChangeScene = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canChangeScene = false;
        }
    }

    void EnterLevel()
    {
        SceneManager.LoadScene(sceneNumber);
        fadeinPanel.GetComponent<CanvasGroup>().alpha = 1f;
    }
}
