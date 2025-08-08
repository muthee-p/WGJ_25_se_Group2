using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public int sceneNumber;
    bool canChangeScene = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canChangeScene)
        {
            EnterLevel();
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
    }
}
