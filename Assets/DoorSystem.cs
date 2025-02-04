using UnityEngine;

public class DoorSystem : MonoBehaviour
{
    public Animator DoorsAnimator;
    public bool _doorIsOpen; // ������� �� ����� ����� ������
    public bool _doorIsLocked; // ������������� �� ����� (����� �� � �������)
    public bool _doorTrigger; // ���� �� � ����� �������

    public AudioSource DoorsOpen;
    public AudioSource DoorsClose;

    public bool _isCloseTimer;  // ���� �� ������ ��������
    public float _timer = -1f;
    private float _saveTimer;
    public float _timeClose = 10f;  // ���� �������� �� �������

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
        // ������ �������� ������

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
        Debug.Log("������� �����!");
        if (_doorTrigger && !_doorIsOpen)  // ����� ������������ ��������������� �����
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
            // ������ ������� ��������������� �������� �����
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

    public void CloseAndSetLocked()  // ������� ��������� � ��������� �����
    {
        if (_doorIsOpen)
        {
            // �������� ������������
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

    public void SetLocked()  // ������� ������ ��������� �����
    {
        if (_timer >= 0f)
        {
            // ��������� ������ ��������
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

    public void SetUnlocked()  // ������� ������������ �����
    {
        if (_doorIsLocked)
        {

        }
        if (_saveTimer >= 0f)
        {
            // ��������� ������ ��������
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

