using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] Material matIn, matOut;
    ParticleSystem particles;
    MeshRenderer _renderer;
    BoxCollider _collider;
    private void OnTriggerEnter(Collider other)
    {
        _renderer.material = matIn;
    }
    private void OnTriggerExit(Collider other)
    {
        _renderer.material = matOut;
    }

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _renderer = GetComponent<MeshRenderer>();
        particles = GetComponentInChildren<ParticleSystem>();
    }

    public void Explode()
    {
        _collider.enabled = false;
        _renderer.enabled = false;
        particles.Play();
        StartCoroutine(WaitForParticleStop());
    }

    IEnumerator WaitForParticleStop()
    {
        yield return new WaitForSeconds(5);
        particles.Stop();
        ReEnable();

    }
    void ReEnable()
    {
        transform.localScale = new Vector3(0.051f, 0.051f, 0.001f);
        _collider.enabled = true;
        _renderer.enabled = true;
        StartCoroutine(ScaleEffect());

    }

    IEnumerator ScaleEffect()
    {
        while (transform.localScale.z <= 2.21f)
        {
            transform.localScale += new Vector3(0, 0, 0.01f);
            yield return new WaitForFixedUpdate();
        }
       
    }
}
