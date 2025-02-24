using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    public Animator DoorsAnimator;
    public bool _doorIsOpen; // открыта ли дверь прямо сейчас
    public bool _doorIsLocked; // заблокирована ли дверь (можно ли её открыть)
    public bool _doorTrigger; // есть ли у двери триггер

    public AudioSource DoorsOpen;
    public AudioSource DoorsClose;

    public bool _isCloseTimer;  // есть ли таймер закрытия
    public float _timer = -1f;
    private float _saveTimer;
    public float _timeClose = 10f;  // вемя закрытия по таймеру

    public DoorButton[] DoorButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //_doorIsOpen = false;
        //_doorIsLocked = false;

        //_timer = -1f;
        //_timeClose = 10f;
        if (_doorIsOpen)
        {
            Invoke("ToCloseDoors", 2f);
        }
    }

    public void ToCloseDoors()
    {
        DoorsAnimator.SetTrigger("ToClose");
        _doorIsOpen = false;
        DoorsClose.Play();
        _timer = -1f;
        for (int i = 0; i < DoorButton.Length; i++)
        {
            if (DoorButton[i]._isPower) 
            {
                DoorButton[i].SetButtonImageToOpen();
            }            
        }
    }

    void Update()
    {
        // таймер закрытия дверей

        if (_isCloseTimer && _doorIsOpen && _timer >= 0f)
        {
            _timer += Time.deltaTime;
        }
        if (_timer > _timeClose)
        {
            CloseDoor();
        }
    }


    public void PlayerInTrigger()
    {
        if (_doorTrigger && !_doorIsOpen && !_doorIsLocked)
        {
            OpenDoor();
        }
    }

    public void EnemyInTrigger()
    {
        //----------------
        Debug.Log("Открыть дверь!");
        if (_doorTrigger && !_doorIsOpen)  // враги разблокируют заблокированные двери
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        _doorIsOpen = true;
        DoorsOpen.Play();
        DoorsAnimator.SetBool("isOpen", true);
        DoorsAnimator.SetTrigger("ToOpen");

        for (int i = 0; i < DoorButton.Length; i++)
        {
            if (DoorButton[i]._isPower)
            {
                DoorButton[i].SetButtonImageToClose();
            }
        }

        if (_isCloseTimer && !_doorIsLocked)
        {
            // запуск таймера автоматического закрытия двери
            _timer = 0f;
        }
    }

    public void CloseDoor()
    {
        _timer = -1f;
        _doorIsOpen = false;
        DoorsAnimator.SetBool("isOpen", false);
        DoorsAnimator.SetTrigger("ToClose");
        DoorsClose.Play();
        for (int i = 0; i < DoorButton.Length; i++)
        {
            if (DoorButton[i]._isPower)
            {
                DoorButton[i].SetButtonImageToOpen();
            }
        }

    }

    public void CloseAndSetLocked()  // функция закрывает и блокирует дверь
    {
        if (_doorIsOpen)
        {
            // включить автозакрытие
            CloseDoor();
        }        
        _doorIsLocked = true;
        _timer = -1f;

        for (int i = 0; i < DoorButton.Length; i++)
        {
            if (DoorButton[i]._isPower)
            {
                DoorButton[i].SetButtonImageToBlocked();
            }
        }
    }

    public void SetLocked()  // функция просто блокирует дверь
    {
        if (_timer >= 0f)
        {
            // сохранить таймер закрытия
            _saveTimer = _timer;
            _timer = -1f;
        }
        else
        {
            _saveTimer = -1f;
        }
        _doorIsLocked = true;
        for (int i = 0; i < DoorButton.Length; i++)
        {
            if (DoorButton[i]._isPower)
            {
                DoorButton[i].SetButtonImageToBlocked();
            }
        }
    }

    public void SetUnlocked()  // функция разблокирует дверь
    {
        if (_doorIsLocked)
        {

        }
        if (_saveTimer >= 0f)
        {
            // сохранить таймер закрытия
            _timer = _saveTimer;
            _saveTimer = -1f;
        }
        _doorIsLocked = false;

        for (int i = 0; i < DoorButton.Length; i++)
        {
            if (DoorButton[i]._isPower)
            {
                if (_doorIsOpen)
                {
                    DoorButton[i].SetButtonImageToClose(); 
                }
                else
                {
                    DoorButton[i].SetButtonImageToOpen();
                }
                
            }
        }
    }
}

