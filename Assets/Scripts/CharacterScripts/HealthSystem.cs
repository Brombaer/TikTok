using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem
{
    public event EventHandler OnHealthChanged;
    
    private int _health;
    private int _maxHealth;

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
