using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float r;
    public int planes;
    private Rigidbody rigidbody1;
    //private float lastVelocity;
    public Vector3  StarterForce;
    void Awake()
    {
        rigidbody1 = gameObject.GetComponent<Rigidbody>();
        rigidbody1.AddForce(StarterForce);
        //lastVelocity = rigidbody.velocity.magnitude;
    }
    private Rigidbody rigidbody2;
    void OnTriggerStay(Collider collider)
    {
        if (collider.transform.parent.gameObject.GetComponent<Rigidbody>() != null)
        {
            rigidbody2 = collider.transform.parent.gameObject.GetComponent<Rigidbody>();

            Vector3 direction = (rigidbody2.transform.position - transform.position).normalized;
            float r = Vector3.Distance(rigidbody2.transform.position, transform.position);
            float m1 = rigidbody1.mass;
            float m2 = rigidbody2.mass;
            float F = (m1 * m2) / (r * r);
            gameObject.GetComponent<Rigidbody>().AddForce(direction * F);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.parent.gameObject.GetComponent<Rigidbody>() != null)
        {
            PlanetGenerator Generator = transform.parent.gameObject.GetComponent<PlanetGenerator>();
            float r2 = collision.gameObject.GetComponent<Gravity>().r;
            int planes2 = collision.gameObject.GetComponent<Gravity>().planes;
            Generator.GenerateCube(planes2 + planes, PlanetGenerator.ConstantSubdivisions, r + r2);
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
