using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    public static HealthScript instance;
    [SerializeField] private Slider healthBar;
    [SerializeField] private GameObject messagePanel, restartButton;
    [SerializeField] private TextMeshProUGUI messageText;
    public float maxHealth;
    float healthLoss = 10;
    float currentHealth;
    bool messageShown = false;
    Coroutine coroutineDrainHealth;

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

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;

        coroutineDrainHealth = StartCoroutine(DrainHealth());
    }

    IEnumerator DrainHealth()
    {
        while (currentHealth > 0)
        {
            currentHealth -= healthLoss;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            healthBar.value = currentHealth;

            if (currentHealth < 250 && !messageShown)
            {
                messageShown = true;
                ChangeSprites();
                yield return StartCoroutine(ShowMessage("You are getting low on health!", 3f));
            }

            yield return new WaitForSeconds(0.2f);
        }
        GameOver("You have Fainted!");
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.value = currentHealth;
    }

    public void RefillHealth()
    {
        ChangeSpritesBack();
        currentHealth = maxHealth;
        healthBar.value = currentHealth;
        messageShown = false;

        if (coroutineDrainHealth != null)
        {
            StopCoroutine(coroutineDrainHealth);
        }

    }

    public void RestartRoutineDrainHealth()
    {
        coroutineDrainHealth = StartCoroutine(DrainHealth());
    }

    void ChangeSprites()
    {
        SpriteChange[] spritesChange = FindObjectsOfType<SpriteChange>();
        if (spritesChange.Length == 0) return;
        foreach (SpriteChange spriteChange in spritesChange)
        {
            spriteChange.ChangeSprite();
        }
    }
    void ChangeSpritesBack()
    {
        SpriteChange[] spritesChange = FindObjectsOfType<SpriteChange>();
        if (spritesChange.Length == 0) return;
        foreach (SpriteChange spriteChange in spritesChange)
        {
            spriteChange.ChangeSpriteBack();
        }
    }

    IEnumerator ShowMessage(string message, float duration)
    {
        messageText.text = message;
        messagePanel.SetActive(true);
        yield return new WaitForSeconds(duration);
        messagePanel.SetActive(false);
    }

    public void GameOver(string message)
    {
        StopCoroutine(coroutineDrainHealth);
        CharacterController.instance.Faint();
        messageText.text = message;
        messagePanel.SetActive(true);
        messagePanel.transform.DOScale(1, 0.4f).SetEase(Ease.OutBack);
        restartButton.SetActive(true);

    }
     public void YouWon(string message)
    {
        StopCoroutine(coroutineDrainHealth);
        CharacterController.instance.Faint();
        messageText.text = message;
        messagePanel.SetActive(true);
    }
    

    public void RestartLevel()
    {
        GameController.instance.RespawnPlayer();
        restartButton.SetActive(false);
        messagePanel.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack)
        .OnComplete(() => messagePanel.gameObject.SetActive(false));
    }

}
