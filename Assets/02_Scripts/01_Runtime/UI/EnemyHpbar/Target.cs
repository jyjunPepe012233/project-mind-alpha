using MinD.Runtime.Entity;
using UnityEngine;

public class Target : MonoBehaviour
{
    private float maxHealth;
    private float currentHealth;

    private Enemy enemy;

    private EnemyAttributeHandler attributeHandler;


    public float MaxHealth => maxHealth;

    public float CurrentHealth
    {
        get => currentHealth;
        set => currentHealth = Mathf.Clamp(value, 0f, maxHealth);
    }

    private void Awake()
    {
        if (enemy == null)
        {
            enemy = GetComponent<Enemy>();
        }

        if (enemy != null)
        {
            attributeHandler = enemy.GetComponent<EnemyAttributeHandler>();

            if (attributeHandler != null)
            {
                maxHealth = attributeHandler.MaxHp;
            }

            currentHealth = enemy.CurHp;
        }
    }

    private void Update()
    {
        if (enemy != null)
        {
            currentHealth = enemy.CurHp;
        }
    }
}