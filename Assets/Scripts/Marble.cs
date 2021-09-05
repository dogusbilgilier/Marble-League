using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marble : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float defaultSpeed, speedMult;
    InputManager inputManager;
    public float boostTime, cooldownTime;
    [HideInInspector]
    public float totalCooldown,finTime;
    bool stopBoost,cooldownComplete;
    bool isboost;
    UIManager uiManager;
    bool aiInput;

    public bool fin;
   
    void Start()
    {
        uiManager = UIManager.inst;
        inputManager = InputManager.inst;
        rb = GetComponent<Rigidbody>();

        totalCooldown = cooldownTime + boostTime;
        uiManager.cooldownTime = totalCooldown;

        cooldownComplete = true;
        isboost = false;
        stopBoost = true;

        rb.useGravity = false;
        rb.velocity = Vector3.zero;

        if (!CompareTag("player"))
            InvokeRepeating(nameof(RandomiseAIinput), 2, totalCooldown);

        StartCoroutine(Go());
    }
    void FixedUpdate()
    {
        if (!fin)
        {
            if (gameObject.CompareTag("player"))
                PlayerBoost();
            else
                AIBoost();
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }
    IEnumerator Boost(Vector3 dir)
    {
        stopBoost = false;
        cooldownComplete = false;
        while (!stopBoost)
        {
            isboost = true;
            yield return new WaitForFixedUpdate();
            rb.velocity = dir;
        }
        isboost = false;
    }
    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(totalCooldown);
        cooldownComplete = true; inputManager.boost = false;
    }
    IEnumerator StopBoost()
    {
        yield return new WaitForSeconds(boostTime);
        stopBoost = true;
        rb.velocity = new Vector3(rb.velocity.x, -defaultSpeed);
    }
    private void OnCollisionEnter(Collision collision)
    {
       /* if (collision.transform.gameObject.CompareTag("obstacle"))
        {
            Debug.Log("Hit Obstacle");
            rb.velocity = new Vector3(-rb.velocity.x, 1.3f);
        }*/
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("obstacle"))
        {
            if (isboost && rb.velocity.y <= -0.5f)
            {
                other.GetComponent<Obstacle>().Explode();
            }
                

            else
                StartCoroutine(ObstacleHit(other.gameObject));

        }
        if (other.CompareTag("fin"))
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            fin = true;
            
        }
    }
    IEnumerator ObstacleHit(GameObject obs)
    {
        Vector3 dir = -rb.velocity*3;
        while (Vector3.Distance(transform.position, obs.transform.position) <= 0.15f)
        {
            yield return new WaitForFixedUpdate();
            rb.velocity = dir;
        }
    }
    void SpeedLimit()
    {
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, defaultSpeed);
    }
    Vector3 BoostDir()
    {
        return Vector3.ClampMagnitude(rb.velocity * speedMult, 2);
    }
    void PlayerBoost()
    {
        if (inputManager.boost && stopBoost == true && cooldownComplete == true)
        {

            StartCoroutine(Boost(BoostDir()));
            StartCoroutine(StopBoost());
            StartCoroutine(Cooldown());
            
            uiManager.startCooldown = true;

        }
        if (!isboost)
        {
            SpeedLimit();
        }
    }
    void AIBoost()
    {
        if ( stopBoost == true && cooldownComplete == true)
        {

            

            if (aiInput == true)
            {
                Debug.Log(aiInput + " " + name);
                StartCoroutine(Boost(BoostDir()));
                StartCoroutine(StopBoost());
                StartCoroutine(Cooldown());
            }
            
        }
        if (!isboost)
        {
            SpeedLimit();
        }
    }
    void RandomiseAIinput()
    {
        aiInput = (Random.Range(0f, 1f) > 0.5f);
    }
    IEnumerator Go()
    {
        yield return new WaitForSeconds(3);
        rb.useGravity = true;
    }
}
