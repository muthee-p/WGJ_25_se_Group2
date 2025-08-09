using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    private bool withinReach = false;
    private GameObject canvas;

    void Start()
    {
        canvas = transform.Find("Canvas").gameObject;
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (withinReach)
        {
            canvas.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                MatchCollectedItem();
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            withinReach = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            withinReach = false;
            canvas.SetActive(false);
        }
    }

    void MatchCollectedItem()
    {
        switch (gameObject.tag)
        {
            case "Codex":
                GameController.instance.ShowCodexPiece();
                break;
            case "Weapon":
                CharacterController.instance.weapon = CharacterController.Weapon.Armed;
                break;
        }
        
    }
}
