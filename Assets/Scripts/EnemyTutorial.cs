using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyTutorial : MonoBehaviour
{
    GameObject canvas;
    void Start()
    {
        canvas = transform.Find("Canvas").gameObject;
        canvas.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canvas.SetActive(true);
            Vector3 originalScale = canvas.transform.localScale; 
            canvas.transform.localScale = Vector3.zero;
            canvas.transform.DOScale(originalScale, 0.4f).SetEase(Ease.OutBack);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameController.instance.enemyTutotialShown = true;
            canvas.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack)
            .OnComplete(() => canvas.gameObject.SetActive(false));
            Destroy(gameObject, 0.4f);
        }
    }

}
