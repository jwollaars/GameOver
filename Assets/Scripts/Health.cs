using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField]
    private GameObject k_Slider;
    [SerializeField]
    private int k_HP;

    [SerializeField]
    private int k_MaxRandomHP;
    [SerializeField]
    private int k_MinRandomHP;

    private int k_Health;
    public int CurrentHealth
    {
        get { return k_Health; }
        set { k_Health = value; }
    }

    private void Start()
    {
        if (gameObject.tag == "Player")
        {
            SetHealth(k_HP);
        }
        else if (gameObject.tag == "Enemy")
        {
            SetEnemyHealth();
        }
    }
    private void Update()
    {
        NoHealth();
        HealthBarController(k_Slider);

        if (k_Health > 100)
        {
            k_Health = 100;
        }

        if (k_Health <= 0 && gameObject.name == "Mega_Man")
        {
            Application.LoadLevel(2);
        }
    }

    private void SetHealth(int _HP)
    {
        k_Health = _HP;
    }
    private void SetEnemyHealth()
    {
        k_Health = Random.Range(k_MinRandomHP, k_MaxRandomHP + 1);
    }
    private void HealthBarController(GameObject _Slider)
    {
        if (k_Slider != null)
        {
            _Slider.GetComponent<Slider>().value = k_Health * 0.01f;
        }
    }

    private void NoHealth()
    {
        if (k_Health <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void LoseHealth(int _Damage)
    {
        k_Health -= _Damage;
    }
    private void GainHealth(int _NewHP)
    {
        k_Health += _NewHP;
    }
}