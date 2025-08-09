using System.Collections;
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
                yield return StartCoroutine(ShowMessage("You are getting low on health!", 2f));
            }

            yield return new WaitForSeconds(0.2f);
        }

        CharacterController.instance.Faint();
        messageText.text = "You Fainted!";
        messagePanel.SetActive(true);
        restartButton.SetActive (true);

    }

    public void RefillHealth()
    {
        ChangeSpritesBack();
        currentHealth = maxHealth;
        healthBar.value = currentHealth;

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

    public void RestartLevel()
    {
        GameController.instance.RespawnPlayer();
        restartButton.SetActive(false);
        messagePanel.SetActive(false);

    }

}
