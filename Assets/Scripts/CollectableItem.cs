using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    private bool withinReach = false;
    private GameObject canvas;

    void Start(){
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
                Debug.Log("Player collected codex");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            withinReach = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            withinReach = false;
            canvas.SetActive(false);
        }
    }
}
