using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Gradient _gradient;
    [SerializeField] private Image _fill;
    
    public void SetMaxHealth(int value)
    {
        _slider.maxValue = value;
        _slider.value = value;

        _fill.color = _gradient.Evaluate(1.0f);
    }

    public void SetHealth(int value)
    {
        _slider.value = value;
        _fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }
}
