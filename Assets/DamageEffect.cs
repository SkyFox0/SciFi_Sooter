using UnityEngine;
using UnityEngine.Rendering;

public class DamageEffect : MonoBehaviour
{

    [SerializeField] private Volume DamageVolume;
   
    [SerializeField] private CanvasGroup CanvasGroup;

    public bool _isEffectEnabled;
    public float _effectForce;
    private int _minHealth;
    public float _effectForceMAX;

    public bool _pulse;
    public float _timer;
    public AnimationCurve WeightStrenght;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _minHealth = 100;
        _effectForceMAX = 0.9f;
        _timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {        
        if (_pulse)
        {
            _timer += Time.deltaTime;
            if (_timer > 1f)
            { _timer = 0f; }
            _effectForce = _effectForce + WeightStrenght.Evaluate(_timer) * Time.deltaTime;
            CanvasGroup.alpha = _effectForce;
            DamageVolume.weight = _effectForce + 0.05f;
        }
        
    }
    public void SetHealth(float Health)
    {
        if (Health < _minHealth)
        {
            _effectForce = (1f - Health / _minHealth) * _effectForceMAX;    //10/100 
            if (_effectForce > 0f)
            {
                _isEffectEnabled = true;
                CanvasGroup.alpha = _effectForce;
                DamageVolume.weight = _effectForce + 0.05f;

            }
        }
        else
        {
            _isEffectEnabled = false;
            CanvasGroup.alpha = 0f;
            DamageVolume.weight = 0.1f;
        }

        if (_effectForce > (_effectForceMAX - 0.4f))
        {
            _pulse = true;
        }
        else
        {
            _pulse = false;
        }



    }



}
