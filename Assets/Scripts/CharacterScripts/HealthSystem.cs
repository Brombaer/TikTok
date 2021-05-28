using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthSystem
{
    public event EventHandler OnHealthChanged;
    
    public int Health;
    public int MaxHealth;

    public HealthSystem(int maxHealth)
    {
        MaxHealth = maxHealth;
        Health = maxHealth;
    }

    public int GetHealth()
    {
        return Health;
    }

    public float GetHealthPercent()
    {
        return (float)Health / MaxHealth;
    }

    public void Damage(int damageAmount)
    {
        Health -= damageAmount;

        if (Health <= 0)
        {
            Health = 0;
        }
        if (Health == 0)
        { 
            Debug.Log("Your health is 0, you die");
            SceneManager.LoadScene(1);
        }

        if (OnHealthChanged != null)
        {
            OnHealthChanged(this,EventArgs.Empty);
        }
    }

    public void Heal(int healAmount)
    {
        Health += healAmount;

        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
        
        if (OnHealthChanged != null)
        {
            OnHealthChanged(this,EventArgs.Empty);
        }
    }
}
