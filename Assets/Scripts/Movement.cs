using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotationThrust = 1f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] ParticleSystem mainEngineParticle;
    [SerializeField] ParticleSystem leftThrusterParticle;
    [SerializeField] ParticleSystem rightThrusterParticle;
    Rigidbody rb;
    AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }
    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }

    }


    private void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine); //play one audio clip
            if (!mainEngineParticle.isPlaying) //to fix the bugs only play when its not already playing
            {
                mainEngineParticle.Play();
            }
        }
    }

      private void StopThrusting()
    {
        audioSource.Stop();
        mainEngineParticle.Stop();
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }

        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {
            StopLeftRightRotation();
        }
    }

    private void RotateLeft()
    {
        ApplyRotation(rotationThrust);
        if (!rightThrusterParticle.isPlaying)
        {
            rightThrusterParticle.Play();
        }
    }

    private void RotateRight()
    {
        ApplyRotation(-rotationThrust);
        if (!leftThrusterParticle.isPlaying)
        {
            leftThrusterParticle.Play();
        }
    }

    private void StopLeftRightRotation()
    {
        rightThrusterParticle.Stop();
        leftThrusterParticle.Stop();
    }

    void ApplyRotation(float rotationThisFrame) //to check if positive or negative parameters are used
    {
        rb.freezeRotation = true; //freezing rotation when controlling so teh physics systum doesnt get involved
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; //unfreezing rotation
    }
}
