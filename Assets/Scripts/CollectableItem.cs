using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [SerializeField] private AudioClip collectedSound;
    public string codexText, researchLogText;
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
            Vector3 originalScale = canvas.transform.localScale; 
            canvas.transform.localScale = Vector3.zero;
            canvas.transform.DOScale(originalScale, 0.4f).SetEase(Ease.OutBack);

            if (Input.GetKeyDown(KeyCode.E))
            {
                AudioSource.PlayClipAtPoint(collectedSound, transform.position);
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
            canvas.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack)
            .OnComplete(() => canvas.gameObject.SetActive(false));
        }
    }

    void MatchCollectedItem()
    {
        switch (gameObject.tag)
        {
            case "Codex":
                GameController.instance.ShowCodexPiece(codexText, researchLogText);
                break;
            case "Weapon":
                CharacterController.instance.weapon = CharacterController.Weapon.Armed;
                break;
        }
        
    }
}
