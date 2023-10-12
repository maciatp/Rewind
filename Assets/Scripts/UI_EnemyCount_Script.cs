using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_EnemyCount_Script : MonoBehaviour
{
    public Color startColor;
    public Color cautionColor;
    public Color alertColor;

    public Image ui_EnemiesRing;

    public TMPro.TextMeshProUGUI maxEnemiesText;

    public GameManager_Script gameManager_;

    private void Awake()
    {
        maxEnemiesText = transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>();
        ui_EnemiesRing = gameObject.GetComponent<Image>();
        gameManager_ = GameObject.Find("GameManager").GetComponent<GameManager_Script>();
    }
    // Start is called before the first frame update
    void Start()
    {
        maxEnemiesText.text = PlayerPrefs.GetInt("MaxEnemies").ToString();

        if(SceneManager.GetActiveScene().name == "Title_Scene")
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //maxEnemiesText.text = PlayerPrefs.GetInt("MaxEnemies").ToString();

        ui_EnemiesRing.fillAmount = (gameManager_.currentEnemies / gameManager_.maxEnemies);
       // Debug.Log(enemiesRatio);

        if (ui_EnemiesRing.fillAmount < gameManager_.maxEnemies / 2)
        {
            ui_EnemiesRing.color = Color.Lerp(startColor, cautionColor, ui_EnemiesRing.fillAmount);
        }
        else if ((ui_EnemiesRing.fillAmount > gameManager_.maxEnemies / 2) && (ui_EnemiesRing.fillAmount < gameManager_.maxEnemies / 0.75f))
        {
            ui_EnemiesRing.color = Color.Lerp(cautionColor, alertColor, ui_EnemiesRing.fillAmount);
        }
        else if (ui_EnemiesRing.fillAmount >= gameManager_.maxEnemies / 0.75f)
        {
            ui_EnemiesRing.color = alertColor;
        }
    }

    private void FixedUpdate()
    {
        maxEnemiesText.text = PlayerPrefs.GetInt("MaxEnemies").ToString();
    }
}
