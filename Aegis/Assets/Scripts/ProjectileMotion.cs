using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileMotion : MonoBehaviour
{
    [SerializeField] private Vector3 maxVelocity = new Vector3(-5.0f, 0f, 0f);
    [SerializeField] private float rampUpTime = 0.5f;
    [SerializeField] private AnimationCurve velocityCurve = new AnimationCurve();
    private bool moving = false;
    private float timer = 0.0f;

    public void Fire()
    {
        moving = true;
    }

    void Update()
    {
        if (moving)
        {
            timer += Time.deltaTime;
            var rampUp = Mathf.Clamp01(timer / this.rampUpTime);
            var rampedVelocity = this.velocityCurve.Evaluate(rampUp) * this.maxVelocity;
            this.GetComponent<Rigidbody>().velocity = new Vector3(rampedVelocity.x, rampedVelocity.y, rampedVelocity.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);
    }

}
