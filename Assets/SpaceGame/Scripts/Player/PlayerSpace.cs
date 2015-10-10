using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerSpace : MonoBehaviour
{
    [Header("Состояния персонажа")]
    public bool immortality = false; // Неузвимость 
    public bool isPCControl;

    [Header("Префабы объектов")]
    public GameObject PrefabOfIceBlast; // Снаяряд выстрела

    // Не извесное говно, не помню для чего создал
    Rigidbody myPlayerRigid;
    Transform myPlayerTrans;
    Vector3 CurrentVelocity;

    int _IceBlasts; // Колличесто зарядов виверы для выстрела
    int _stepSpeed; // Ступень скорости
    int _HP; // Жизнь
    float ForceSpeed; // Скорость/форс виверы

    float AnimSpeed;

    //ограничения
    [Header("Ограничения передвижения")]
    public float MinX;
    public float MaxX;

    public float MinY;
    public float MaxY;



    public float HowManyForce { get { return ForceSpeed; } }
    public int numberOfIceBlasts { get { return _IceBlasts; } }
    public int HP { get { return _HP; } }
    public int stepSpeed { get { return _stepSpeed; } }


    void Start ()
    {
        myPlayerRigid = this.gameObject.GetComponent<Rigidbody>();
        myPlayerTrans = this.gameObject.GetComponent<Transform>();

        // Инициализация переменных
        _stepSpeed = 0;
        _IceBlasts = 1;
        _HP = 606;

        StartCoroutine("CanUseBoost"); // Таймер для возможности использования супер скорости на старте

    }
	
	void Update ()
    {
        //товарищЪ это такая дичь, что я её рефакторить хотел!ы
        if (isPCControl) DebugUpdate();
        else MobileControl();
    }

    void DebugUpdate()
    {
        float z = myPlayerTrans.position.z;

        float TargetAnimSpeed = myPlayerRigid.velocity.z * 0.3f;
        AnimSpeed = Mathf.Lerp(AnimSpeed, TargetAnimSpeed, Time.deltaTime * 1.5f);
        GetComponent<Animator>().SetFloat("Speed", AnimSpeed);

        BustSpeed(z, _stepSpeed);
        if (_HP <= 0)
            Application.LoadLevel("SceneSpace");

        if (Input.GetKey(KeyCode.A))
            MoveInSpace(Vector3.left);
     
        if (Input.GetKey(KeyCode.D))
            MoveInSpace(Vector3.right);

        if (Input.GetKey(KeyCode.W))
            MoveInSpace(Vector3.up);

        if (Input.GetKey(KeyCode.S))
            MoveInSpace(Vector3.down);

        if (Input.GetKeyDown(KeyCode.R))
            Application.LoadLevel("SceneSpace");

        if (Input.GetKeyDown(KeyCode.E))
        {
            IceArmor();
        }

        if (Input.GetKeyDown(KeyCode.Space) && _stepSpeed == 0)
        {
            TakeSpeed(++_stepSpeed, 120);
        }

        else if (Input.GetKeyDown(KeyCode.Space) && _stepSpeed == 1)
        {
            TakeSpeed(++_stepSpeed, 150);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
                Debug.DrawLine(ray.origin, hit.point, Color.cyan);
            if (_IceBlasts >= 1)
            {
                IceBlastsTake();
            }
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            print("1");
        }
        Vector3 newVel = ClearVelocity(myPlayerRigid.velocity);
        myPlayerRigid.velocity = newVel;
        CurrentVelocity = myPlayerRigid.velocity;
    }
    
    // Выравнивание форса по x и y к 0
    Vector3 ClearVelocity(Vector3 Vel) 
    {
        const float offsetFactor = 0.01f;

        if (Vel.x != 0.0f || Vel.y != 0.0f)
        {
            Vector3 newVector = new Vector3(Vel.x, Vel.y, Vel.z);

            //for x
            if (Vel.x > 0.0f)
            {
                newVector = new Vector3(newVector.x - offsetFactor, newVector.y, newVector.z);
            }
            if (Vel.x < 0.0f)
            {
                newVector = new Vector3(newVector.x + offsetFactor, newVector.y, newVector.z);
            }

            //for y

            if (Vel.y > 0.0f)
            {
                newVector = new Vector3(newVector.x, newVector.y - offsetFactor, newVector.z);
            }
            if (Vel.y < 0.0f)
            {
                newVector = new Vector3(newVector.x, newVector.y + offsetFactor, newVector.z);
            }
            return newVector;
        }
        return Vel;
    }

    // Тут обработка врезания в мусор
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Trash" && immortality != true)
        {
            _HP--;
        }
    } 
    
    // Передвижение по x и y
    void MoveInSpace(Vector3 Where, float SpeedMove = 10)
    {
        Vector3 pos = myPlayerTrans.position;
        Vector3 FinalMovement = Where * Time.deltaTime * SpeedMove;
        Vector3 NewPos = pos + FinalMovement;

        //check range
        bool CheckOK = true;
        if (NewPos.x > MaxX || NewPos.x < MinX || NewPos.y > MaxY || NewPos.y < MinY) CheckOK = false;
        if (_stepSpeed != 0 && CheckOK)
            transform.Translate(FinalMovement);        
    }

    // Буст виверы при старте
    void TakeSpeed(int stepSpeedSet = 1, int howManySpeed = 25)
    {
        myPlayerRigid.AddForce(transform.forward * howManySpeed);
        _stepSpeed = stepSpeedSet;
    }

    // Обработка скорости виверны относительно её положения
    void BustSpeed(float nowPosition, int nowStep)
    {
        int maxSpeed = 16;
        if (nowPosition >= 300 && maxSpeed != 20)
        {
            maxSpeed = 20;
        }
        if (nowPosition >= 500 && maxSpeed != 25)
        {
            maxSpeed = 25;
        }

        if (myPlayerRigid.velocity.z <= maxSpeed) // тестовый ограничитель скорости
        { 
            ForceSpeed = nowPosition/50;
            myPlayerRigid.AddForce(transform.forward * ForceSpeed);
        }
        //print(HowManyForce.ToString());
        
    }
    

    // Таймер для возможности использования супер скорости на старте
    IEnumerator CanUseBoost()
    {
        yield return new WaitForSeconds(8);
        if (_stepSpeed == 1)
            _stepSpeed++;
    }

    // Паблик методы бонусов
    public void UsingBonusSpeed(int HowMuchBoost = 100)
    {
        myPlayerRigid.AddForce(transform.forward * HowMuchBoost);
    }
    public void IceBlastsTake(int HowManyIceBlastsNeed = 1)
    {
        _IceBlasts -= HowManyIceBlastsNeed;
        
    }
    public void UsingBonusIceBlastsSet(int LaserSet = 1)
    {
        _IceBlasts += LaserSet;       
    }

    // Паблик метод получения урона из вне
    public void HitHP(int hitHP)
    {
        if (!immortality)
            _HP -= hitHP;
    }

    // Скиллы
    void IceArmor()
    {
        if (immortality)
            immortality = false;
        else
            immortality = true;
    }

    // не рабочий таймер
    IEnumerator TimeSpell(int second, int reloadSpell)
    {
        yield return new WaitForSeconds(second);
    }


    Vector2 test_StartPos;
    Vector2 test_CurrentPos;
    Vector2 test_Offset;
    bool IsMovementEvent;

    void MobileControl ()
    {
        float z = myPlayerTrans.position.z;
        BustSpeed(z, _stepSpeed);
        //start game sequence
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    IsMovementEvent = true;
                    test_StartPos = touch.position;
                    break;
                case TouchPhase.Canceled:
                    IsMovementEvent = false;
                    break;
                case TouchPhase.Ended:
                    if (_stepSpeed < 3)
                    {
                        TakeSpeed(++_stepSpeed, 120);
                        GetComponent<Animator>().SetFloat("Speed", 1.0f);
                    }
                    IsMovementEvent = false;
                    break;
                case TouchPhase.Moved:
                    test_CurrentPos = touch.position;

                    test_Offset = test_CurrentPos - test_StartPos;
                    break;
                case TouchPhase.Stationary:
                    break;
                default:
                    break;
            }

            if (test_Offset != Vector2.zero)
            {
                MoveInSpace(test_Offset, 0.05f);
            }
        }
        
    }
}
