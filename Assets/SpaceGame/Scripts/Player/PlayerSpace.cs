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

    // Не извесное говно, не помню для чего создал
    Rigidbody myPlayerRigid;
    Transform myPlayerTrans;
    Vector3 CurrentVelocity;

    int IceBlasts; // Колличесто зарядов виверы для выстрела
    int stepSpeed; // Ступень скорости
    int HP; // Жизнь
    float ForceSpeed; // Скорость/форс виверы

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
        float x = myPlayerTrans.position.x;
        float z = myPlayerTrans.position.z;
        BustSpeed(z, stepSpeed);
        if (HP <= 0)
            Application.LoadLevel("SceneSpace");

        if (Input.GetKey(KeyCode.A))
            if (x >= -4.5)
                MoveInSpace(Vector3.left);

        if (Input.GetKey(KeyCode.D))
            if(x <= 4.5)
                MoveInSpace(Vector3.right);

        float y = myPlayerTrans.position.y;

        if (Input.GetKey(KeyCode.W))
            if (y <= 7.5)
                MoveInSpace(Vector3.up);

        if (Input.GetKey(KeyCode.S))
            if (y >= -1.5)
                MoveInSpace(Vector3.down);

        if (Input.GetKeyDown(KeyCode.R))
            Application.LoadLevel("SceneSpace");
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            IceArmor();
        }

        if (Input.GetKeyDown(KeyCode.Space) && stepSpeed == 0)
            TakeSpeed(1, 120);        
        else if (Input.GetKeyDown(KeyCode.Space) && stepSpeed == 1)
        {
            myPlayerRigid.AddForce(transform.forward * 150);
            stepSpeed++;
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
    void MoveInSpace(Vector3 Where, int SpeedMove = 10)
    {
        if (stepSpeed != 0)
            transform.Translate(Where * Time.deltaTime * 10);
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
        ForceSpeed = (nowPosition/100) * nowStep;
        myPlayerRigid.AddForce(transform.forward * ForceSpeed);
        //print(HowManyForce.ToString());
        ScoreText.text = "Score: " + nowPosition.ToString();
        SpeedText.text = "Motor acceleration: " + ForceSpeed.ToString();
        stepSpeedText.text = "Step speed: " + nowStep.ToString();
        HPText.text = "HP: " + HP.ToString();
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
}
