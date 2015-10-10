using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    [Header("UI интерфейса виверны")]
    public Text ScoreText;
    public Text SpeedText;
    public Text stepSpeedText;
    public Text LasersText;
    public Text HPText;
    public Text FPSText;

    PlayerSpace PS;

    // Use this for initialization
    void Start()
    {
        PS = gameObject.GetComponent<PlayerSpace>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDebugUI();
    }

    void UpdateDebugUI()
    {
        FPSText.text = "FPS: " + FPSCounter.FramesPerSec;
        ScoreText.text = "Score: " + transform.position.z.ToString();
        SpeedText.text = "Motor acceleration: " + (transform.position.z/50).ToString();
        stepSpeedText.text = "Step speed: " + PS.stepSpeed.ToString();
        HPText.text = "HP: " + PS.HP.ToString();
        LasersText.text = "IceBlasts: " + PS.numberOfIceBlasts.ToString();
    } 
}
