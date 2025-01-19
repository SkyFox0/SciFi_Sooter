using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class DamageEffect : MonoBehaviour
{

    [SerializeField] private Volume DamageVolume;
   
    [SerializeField] private CanvasGroup CanvasGroup;
    
    public bool _isEffectEnabled;
    public bool _effectOff;
    public float _effectForceNew;
    public float _effectForcePulse;
    public float _effectForceOld;
    private int _minHealth;
    public float _effectForceMAX;
    public AudioSource HeartBeat;

    public bool _pulse;
    public float _timer;
    public AnimationCurve WeightStrenght;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _minHealth = 100;
        _effectForceMAX = 0.9f;
        _timer = 0f;
        _effectForceOld = 0f; 
        _effectForcePulse = 0f;
    }

    // Update is called once per frame
    void Update()
    {        
        if (_pulse)
        {
            _timer += Time.deltaTime;
            if (_timer > 1f)
            { _timer = 0f; }
            _effectForcePulse = _effectForcePulse + WeightStrenght.Evaluate(_timer) * Time.deltaTime;
            CanvasGroup.alpha = _effectForcePulse;
            DamageVolume.weight = _effectForcePulse + 0.05f;
        }

        
        if (_isEffectEnabled)
        {
            if (math.abs(_effectForceOld - _effectForceNew) > 0.01f)  // увеличиваем эффект
            {
                _effectForceOld = math.lerp(_effectForceOld, _effectForceNew, 0.3f * Time.deltaTime);
                HeartBeat.volume = _effectForceOld;

                CanvasGroup.alpha = _effectForceOld;
                DamageVolume.weight = _effectForceOld + 0.05f;
            }
        }

        if (_effectOff)
        {
            if (_effectForceOld > _effectForceNew)   // уменьшаем эффект
            {
                _effectForceOld = math.lerp(_effectForceOld, 0f, 0.3f * Time.deltaTime);
                //_effectForceOld = math.lerp(_effectForceOld, _effectForceNew, 0.3f * Time.deltaTime);
                CanvasGroup.alpha = _effectForceOld;
                DamageVolume.weight = _effectForceOld + 0.05f;

                /*CanvasGroup.alpha = 0f;
                DamageVolume.weight = 0.1f;*/

            }
            else
            {
                if (DamageVolume.weight == _effectForceOld + 0.05f)
                {
                    _effectOff = false;
                } 
            } 
            if (_effectForceOld < 0.1f && _effectForceNew == 0f)
            {
                _effectOff = false;
                _effectForceOld = 0f;
                CanvasGroup.alpha = _effectForceOld;
                DamageVolume.weight = _effectForceOld + 0.05f;
            }
        }
    }

 
    public void SetHealth(float Health)  // передача нового уровня здоровья игрока
    {
        if (Health < _minHealth)  // если здоровья меньше половины - включить индикацию
        {
           //_effectForceOld = _effectForceNew; // сохраняем старое значение
            _effectForceNew = (1f - Health / _minHealth) * _effectForceMAX;   // новое значение силы индикации
            if (_effectForceNew > 0f)
            {
                
                _isEffectEnabled = true;
                _effectOff = false;
                if (!HeartBeat.isPlaying)
                {
                    HeartBeat.Play();
                }
                
                /*HeartBeat.volume = _effectForceNew;
                CanvasGroup.alpha = _effectForceNew;
                DamageVolume.weight = _effectForceNew + 0.05f;*/

            }
        }
        else
        {
            _isEffectEnabled = false;
            _effectOff = true;
            _effectForceOld = _effectForceNew;
            _effectForceNew = 0f;
            HeartBeat.Stop();


        }

        if (_effectForceNew > (_effectForceMAX - 0.4f))
        {
            _pulse = true;
            _effectForcePulse = _effectForceNew;
        }
        else
        {
            _pulse = false;
        }



    }



}
