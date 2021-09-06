using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] Material matIn, matOut;
    ParticleSystem particles;
    MeshRenderer _renderer;
    BoxCollider _collider;
    Vector3 startScale;
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
        startScale = transform.localScale;
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
        transform.localScale = new Vector3(startScale.x, startScale.y, 0.001f);
        _collider.enabled = true;
        _renderer.enabled = true;
        StartCoroutine(ScaleEffect());

    }
    IEnumerator ScaleEffect()
    {
        while (transform.localScale.z <= startScale.z)
        {
            transform.localScale += new Vector3(0, 0, 0.08f);
            yield return new WaitForFixedUpdate();
        }
       
    }
}
