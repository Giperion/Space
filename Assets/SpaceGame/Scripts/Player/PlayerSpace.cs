using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerSpace : MonoBehaviour
{
    [Header("Состояния персонажа")]
    public bool immortality = false; // Неузвимость 

    [Header("Префабы объектов")]
    public GameObject PrefabOfIceBlast; // Снаяряд выстрела

    [Header("UI интерфейса виверны")]
    public Text ScoreText;
    public Text SpeedText;
    public Text stepSpeedText;
    public Text LasersText;
    public Text HPText;
    public Text FPSText;

    // Не извесное говно, не помню для чего создал
    Rigidbody myPlayerRigid;
    Transform myPlayerTrans;
    Vector3 CurrentVelocity;

    int IceBlasts; // Колличесто зарядов виверы для выстрела
    int stepSpeed; // Ступень скорости
    int HP; // Жизнь
    float ForceSpeed; // Скорость/форс виверы

    //ограничения
    [Header("Ограничения передвижения")]
    public float MinX;
    public float MaxX;

    public float MinY;
    public float MaxY;

    public float HowManyForce { get { return ForceSpeed; } }
    public int numberOfIceBlasts { get { return IceBlasts; } }


    void Start ()
    {
        myPlayerRigid = this.gameObject.GetComponent<Rigidbody>();
        myPlayerTrans = this.gameObject.GetComponent<Transform>();

        // Инициализация переменных
        stepSpeed = 0;
        IceBlasts = 1;
        HP = 10;

        // Обработка интерфейса
        StartCoroutine("CanUseBoost"); // Таймер для возможности использования супер скорости на старте
        LasersText.text = "IceBlasts: " + IceBlasts.ToString();
        HPText.text = "HP: " + HP.ToString();
    }
	
	void Update ()
    {
        //товарищЪ это такая дичь, что я её рефакторить хотел!ы
        //DebugUpdate();
        MobileControl();
    }

    void DebugUpdate()
    {
        float z = myPlayerTrans.position.z;

        BustSpeed(z, stepSpeed);
        if (HP <= 0)
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

        if (Input.GetKeyDown(KeyCode.Space) && stepSpeed == 0)
        {
            TakeSpeed(stepSpeed++, 120);
            GetComponent<Animator>().SetFloat("Speed", 1.0f);
        }

        else if (Input.GetKeyDown(KeyCode.Space) && stepSpeed == 1)
        {
            TakeSpeed(stepSpeed++, 150);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
                Debug.DrawLine(ray.origin, hit.point, Color.cyan);
            if (IceBlasts >= 1)
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
            HP--;
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

        if (stepSpeed != 0 && CheckOK)
            transform.Translate(FinalMovement);
    }

    // Буст виверы при старте
    void TakeSpeed(int stepSpeedSet = 1, int howManySpeed = 25)
    {
        myPlayerRigid.AddForce(transform.forward * howManySpeed);
        stepSpeed = stepSpeedSet;
    }

    // Обработка скорости виверны относительно её положения
    void BustSpeed(float nowPosition, int nowStep)
    {
        int SpeedFactor = 16;
        if (nowPosition > 1000)
        {
            SpeedFactor = 32;
        }
        if (myPlayerRigid.velocity.z <= SpeedFactor) // тестовый ограничитель скорости
        { 
            ForceSpeed = (nowPosition/100) * nowStep;
            myPlayerRigid.AddForce(transform.forward * ForceSpeed);
        }
        //print(HowManyForce.ToString());
        ScoreText.text = "Score: " + nowPosition.ToString();
        SpeedText.text = "Motor acceleration: " + ForceSpeed.ToString();
        stepSpeedText.text = "Step speed: " + nowStep.ToString();
        HPText.text = "HP: " + HP.ToString();

        FPSText.text = "FPS: " + FPSCounter.FramesPerSec.ToString();
    }

    // Таймер для возможности использования супер скорости на старте
    IEnumerator CanUseBoost()
    {
        yield return new WaitForSeconds(8);
        if (stepSpeed == 1)
            stepSpeed++;
    }

    // Паблик методы бонусов
    public void UsingBonusSpeed(int HowMuchBoost = 100)
    {
        myPlayerRigid.AddForce(transform.forward * HowMuchBoost);
    }
    public void IceBlastsTake(int HowManyIceBlastsNeed = 1)
    {
        IceBlasts -= HowManyIceBlastsNeed;
        LasersText.text = "Lasers: " + IceBlasts.ToString();
    }
    public void UsingBonusIceBlastsSet(int LaserSet = 1)
    {
        IceBlasts += LaserSet;
        LasersText.text = "Lasers: " + IceBlasts.ToString();
    }

    // Паблик метод получения урона из вне
    public void HitHP(int hitHP)
    {
        if (!immortality)
            HP -= hitHP;
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
        BustSpeed(z, stepSpeed);
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
                    if (stepSpeed < 3)
                    {
                        TakeSpeed(++stepSpeed, 120);
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
