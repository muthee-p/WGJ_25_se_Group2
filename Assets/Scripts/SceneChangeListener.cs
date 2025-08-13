using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class SceneChangeListener : MonoBehaviour
{
    CanvasGroup fadeinPanel;
    Image sceneNameImage;
    TextMeshProUGUI sceneNameText;

    public float dropDistance = 500f;
    public float dropTime = 1f;
    public float bounceTime = 0.3f;
    public float fadeTime = 0.5f;
    public float fadeDuration = 0.5f;

    void Start()
    {
        GameController.instance.ChangeScene();
        fadeinPanel = GameObject.Find("FadeInPanel").GetComponent<CanvasGroup>();
        sceneNameImage = GameObject.Find("SceneNameImage").GetComponent<Image>();
        sceneNameText = GameObject.Find("SceneNameText").GetComponent<TextMeshProUGUI>();

        PlayTransition();

        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                sceneNameText.text = "Main Lab";
                break;
            case 1:
                sceneNameText.text = "Main Lobby";
                break;
            case 2:
                sceneNameText.text = "Specimen Quarantine";
                break;
            case 3:
                sceneNameText.text = "Biohazard Containment";
                break;
        }

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            PlayTransition();
        }
    }

    public void PlayTransition()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(fadeinPanel.DOFade(0f, fadeDuration));

        Vector3 startPos = sceneNameImage.rectTransform.anchoredPosition;

        seq.Append(sceneNameImage.DOFade(0f, fadeTime));
        seq.Join(sceneNameImage.rectTransform.DOAnchorPosY(startPos.y - dropDistance, dropTime).SetEase(Ease.OutQuad)); // drop down

        seq.Append(sceneNameImage.rectTransform.DOAnchorPosY(startPos.y, bounceTime).SetEase(Ease.OutBounce)); // bounce back up
        seq.Join(sceneNameImage.DOFade(1f, fadeTime));
    }

}
