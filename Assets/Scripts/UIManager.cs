using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager inst;
    public static bool go;
    public Image marbleCooldown;
    [SerializeField] GameObject playerMarble;
    [SerializeField] TextMeshProUGUI countDownTxt;
    public bool startCooldown;
    public float fillamount=0;
    public float cooldownTime;
    int countDown;

    void Start()
    {
        inst = this;

        go = false;
        countDown = 3;
        StartCoroutine(CountDown());
    }

    void Update()
    {
        if (startCooldown)
        {
            fillamount += Time.deltaTime;
            marbleCooldown.fillAmount = fillamount / cooldownTime;

            if (marbleCooldown.fillAmount == 1)
            {
                fillamount = 0;
                startCooldown = false;
            } 
        }
    }

    IEnumerator CountDown()
    {
        while (countDown > 0)
        {
            yield return new WaitForSeconds(1);
            countDown--;
            countDownTxt.text = countDown.ToString();
        }
        countDownTxt.text = "GO!";
        yield return new WaitForSeconds(1);
        countDownTxt.gameObject.SetActive(false);
        go = true;
        
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

}
