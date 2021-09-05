using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] Material matIn, matOut;
    ParticleSystem particles;
    MeshRenderer renderer;
    BoxCollider collider;
    GameObject part1, part2;
    private void OnTriggerEnter(Collider other)
    {
       renderer.material = matIn;
    }
    private void OnTriggerExit(Collider other)
    {
        renderer.material = matOut;
    }

    private void Start()
    {
        collider = GetComponent<BoxCollider>();
        renderer = GetComponent<MeshRenderer>();
        particles = GetComponentInChildren<ParticleSystem>();
    }

    public void Explode()
    {
        collider.enabled = false;
        renderer.enabled = false;
        particles.Play();
        StartCoroutine(WaitForParticleStop());
    }

    IEnumerator WaitForParticleStop()
    {
        yield return new WaitForSeconds(5);
        particles.Stop();

    }
}
