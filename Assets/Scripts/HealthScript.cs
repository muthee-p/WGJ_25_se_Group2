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
    [SerializeField] private Image bloodyOverlay;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private AudioClip healthLowWarning, faintingSound;
    public float maxHealth, lowHealthThreshold, currentHealth;
    float healthLoss = 10;
    bool messageShown = false;
    Coroutine coroutineDrainHealth;
    AudioSource audioSource;
    Vector3 messagePanelScale;

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
        messagePanelScale = messagePanel.transform.localScale;

        audioSource = GetComponent<AudioSource>();
    }
    IEnumerator DrainHealth()
    {
        while (currentHealth > 0)
        {
            currentHealth -= healthLoss;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            healthBar.value = currentHealth;

            if (currentHealth <= 3000)
            {
                GlitchController.instance.SetNoise(0.1f);
                GlitchController.instance.SetGlitchStrength(0.1f);
                GlitchController.instance.SetScanLines(0.8f);
            }

            if (currentHealth <= lowHealthThreshold && !messageShown
                && CharacterCheckAttack.instance.beingAttacked == CharacterCheckAttack.BeingAttacked.NotBeingAttacked)
            {
                messageShown = true;
                ChangeSprites();

                audioSource.clip = healthLowWarning;
                if (!audioSource.isPlaying) audioSource.Play();
                yield return StartCoroutine(ShowMessage("You are getting low on health!", 4f));
            }

            yield return new WaitForSeconds(0.2f);
        }

        if (CharacterCheckAttack.instance.beingAttacked == CharacterCheckAttack.BeingAttacked.NotBeingAttacked)
        {

            GameOver("You have Fainted!");
        }
        else
        {
            GameOver("You have been Captured!");
            CharacterCheckAttack.instance.beingAttacked = CharacterCheckAttack.BeingAttacked.Captured;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.value = currentHealth;
        GameObject.Find("Virtual Camera").GetComponent<CameraShake>().SetNoise(1f, 0.5f);
        
        bloodyOverlay.DOFade(1f, 0.1f).OnComplete(() => bloodyOverlay.DOFade(0.35f, 0.1f));
        //StartCoroutine(HitStop());
    }

    IEnumerator HitStop()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1f;
    }

    public void RefillHealth()
    {
        if (audioSource.isPlaying) audioSource.Stop();
        audioSource.clip = null;

        Color color = bloodyOverlay.color;
        color.a = 0;
        bloodyOverlay.color = color;

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

    public void StopRoutineDrainHealth()
    {
        if (coroutineDrainHealth != null)
        {
            StopCoroutine(coroutineDrainHealth);
        }
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
        messagePanel.transform.localScale = Vector3.zero;
        messagePanel.transform.DOScale(messagePanelScale, 0.4f).SetEase(Ease.OutBack);

        yield return new WaitForSeconds(duration);
        messagePanel.SetActive(false);
    }

    public void StartShowMessage(string message, float duration)
    {
        StartCoroutine(ShowMessage(message, duration));
    }

    #region Game States
    public void GameOver(string message)
    {
        StopCoroutine(coroutineDrainHealth);
        CharacterController.instance.Faint();

        audioSource.clip = faintingSound;
        audioSource.loop = false;
        audioSource.volume = 0.5f;
        if (!audioSource.isPlaying) audioSource.Play();

        messageText.text = message;

        messagePanel.SetActive(true);
        messagePanel.transform.localScale = Vector3.zero;
        messagePanel.transform.DOScale(messagePanelScale, 0.4f).SetEase(Ease.OutBack);

        restartButton.SetActive(true);
        bloodyOverlay.DOFade(0f, 0.1f);
    }


    public void RestartLevel()
    {
        if (audioSource.isPlaying) audioSource.Stop();
        audioSource.clip = null;

        GameController.instance.RespawnPlayer();
        CharacterCheckAttack.instance.beingAttacked = CharacterCheckAttack.BeingAttacked.NotBeingAttacked;
        currentHealth = maxHealth;
        RestartRoutineDrainHealth();
        restartButton.SetActive(false);
        messagePanel.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack)
        .OnComplete(() => messagePanel.gameObject.SetActive(false));
        bloodyOverlay.DOFade(0f, 0.1f);
    }
    
    #endregion

}
