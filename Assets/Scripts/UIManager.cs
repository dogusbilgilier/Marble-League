using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager inst;
    public static bool go;

    int countDown;
    int index;

    public Image marbleCooldown;
    public GameObject fin;

    [SerializeField] GameObject playerMarble;
    [SerializeField] TextMeshProUGUI countDownTxt;

    [HideInInspector] public bool startCooldown;
    [HideInInspector] public float fillamount = 0;
    [HideInInspector] public float cooldownTime;
    [SerializeField] GameObject[] lines;

    void Start()
    {
        index = 0;
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
    public void AddList(string name,string time)
    {
        lines[index].transform.GetChild(0).GetComponent<Text>().text = name;
        lines[index].transform.GetChild(1).GetComponent<Text>().text = time;
        index++;
    }

}
