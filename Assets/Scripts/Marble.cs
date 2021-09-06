using System;
using System.Collections;
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

    [SerializeField]ParticleSystem effectBoost, effectMax;
   
    void Start()
    {
        finTime = 0;
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
            PlayEffect(rb.velocity);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
        finTime += Time.fixedDeltaTime;

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
            Fin();
        }
        if (other.CompareTag("border"))
        {
            StartCoroutine(BorderHit(other.gameObject));
        }
    }
    void PlayEffect(Vector3 speed)
    {

        if (speed.x > 1.8f || speed.y > 1.8f ||
            speed.x < -1.8f || speed.y < -1.8f)
        {
            if (effectBoost.isPlaying)
                effectBoost.Stop();
            if (effectMax.isStopped)
                effectMax.Play();
        }

        else if (speed.x > 0.5f || speed.y > 0.5f ||
           speed.x < -0.5f || speed.y < -0.5f)
        {
            if (effectBoost.isPlaying)
                effectMax.Stop();
            if (effectBoost.isStopped)
                effectBoost.Play();
        }

        else
        {
            if (effectMax.isPlaying)
                effectMax.Stop();
            if (effectBoost.isPlaying)
                effectBoost.Stop();
        }

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
        if (stopBoost == true && cooldownComplete == true)
        {
            if (aiInput == true)
            {
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
        aiInput = UnityEngine.Random.Range(0f, 1f) > UnityEngine.Random.Range(0.2f, 0.7f);
    }
    void SpeedLimit()
    {
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, defaultSpeed);
    }
    Vector3 BoostDir()
    {
        Vector3 dir = Vector3.ClampMagnitude(rb.velocity * speedMult, 2);
        //PlayEffect(dir);
        return dir;

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
    IEnumerator ObstacleHit(GameObject obs)
    {
        Vector3 dir = -rb.velocity*3;
        while (Vector3.Distance(transform.position, obs.transform.position) <= 0.25f)
        {
            yield return new WaitForFixedUpdate();
            rb.velocity = dir;
        }
    }
    IEnumerator BorderHit(GameObject obs)
    {
        Vector3 dir = -rb.velocity * 3;
        while (Mathf.Abs(transform.position.x- obs.transform.position.x) <= 1f)
        {
            yield return new WaitForFixedUpdate();
            rb.velocity = dir;
        }
    }
    IEnumerator Go()
    {
        yield return new WaitForSeconds(3);
        rb.useGravity = true;
    }

    void Fin()
    {

        if (CompareTag("player"))
        {
            uiManager.fin.SetActive(true);
            name = "Me";
        }
        string finTimeStr = String.Format("{0:0.00}", finTime);
        uiManager.AddList(name, finTimeStr);
        finTime = 0;
    }

}
