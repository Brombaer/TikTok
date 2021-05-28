using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthSystem
{
    public event EventHandler OnHealthChanged;
    
    public int _health;
    public int _maxHealth;

    public HealthSystem(int maxHealth)
    {
        _maxHealth = maxHealth;
        _health = maxHealth;
    }

    public int GetHealth()
    {
        return _health;
    }

    public float GetHealthPercent()
    {
        return (float)_health / _maxHealth;
    }

    public void Damage(int damageAmount)
    {
        _health -= damageAmount;

        if (_health <= 0)
        {
            _health = 0;
        }
        if (_health == 0)
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
        _health += healAmount;

        if (_health > _maxHealth)
        {
            _health = _maxHealth;
        }
        
        if (OnHealthChanged != null)
        {
            OnHealthChanged(this,EventArgs.Empty);
        }
    }
}
