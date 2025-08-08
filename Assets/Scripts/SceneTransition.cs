using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public int sceneNumber;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            EnterLevel();
        }
    }
    
    void EnterLevel()
    {
        SceneManager.LoadScene(sceneNumber);
    }
}
