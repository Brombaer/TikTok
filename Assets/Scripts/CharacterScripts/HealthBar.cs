using System;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private HealthSystem _healthSystem;

    public void Setup(HealthSystem healthSystem)
    {
        _healthSystem = healthSystem;
        _healthSystem.OnHealthChanged += HealthSystemOnHealthChanged;
    }

    private void HealthSystemOnHealthChanged(object sender, EventArgs e)
    {
        transform.Find("FillArea/Fill").localScale = new Vector3(_healthSystem.GetHealthPercent(), 1);
    }
}
