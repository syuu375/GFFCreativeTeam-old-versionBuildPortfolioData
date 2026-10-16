using UnityEngine;
using UnityEngine.UI;

public class HPBarUI : MonoBehaviour
{
    [SerializeField] private Image hpBarImage;   // HP–{‘Ì‚ÌImage
    private GameObject _player;
    private PlayerHP _playerhp;
    private float maxHealth;
    private float currentHealth;

    void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        if (_player != null)
        {
            _playerhp = _player.GetComponent<PlayerHP>();
        }
            //currentHealth = maxHealth;
            
    }

    private void Start()
    {
        maxHealth = _playerhp.MaxHP;
        currentHealth = _playerhp.CurrentHP;
        UpdateBar();
    }

    private void Update()
    {
        currentHealth = _playerhp.CurrentHP;
        UpdateBar();
    }

    //public void ChangeHP(float value)
    //{

    //    currentHealth = Mathf.Clamp(currentHealth + value, 0, maxHealth);
    //    UpdateBar();
    //}

    private void UpdateBar()
    {
        float percent = currentHealth / maxHealth;
        hpBarImage.fillAmount = percent;
    }

    //public void ResetHP()
    //{
    //    currentHealth = maxHealth;
    //    UpdateBar();
    //}
}
