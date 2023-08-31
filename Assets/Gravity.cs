using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public bool doNotMarge;
    public bool ToDestroy;
    public float r;
    public int planes;
    private Rigidbody rigidbody1;
    //private float lastVelocity;
    public Vector3  StarterForce;
    public static float MassMultyPlayer = 2;
    void Awake()
    {
        ToDestroy = false;
        rigidbody1 = gameObject.GetComponent<Rigidbody>();
        rigidbody1.AddForce(StarterForce);
        //lastVelocity = rigidbody.velocity.magnitude;
    }
    private Rigidbody rigidbody2;
    void OnTriggerStay(Collider collider)
    {
        //if (collider.transform.parent.parent == null) return;

        if (collider.transform.gameObject.GetComponent<Rigidbody>() != null)
        {
            if (collider.transform.GetComponent<Gravity>().ToDestroy == true) return;
            rigidbody2 = collider.transform.gameObject.GetComponent<Rigidbody>();

            Vector3 direction = (rigidbody2.transform.position - transform.position).normalized;
            float R = Vector3.Distance(rigidbody2.transform.position, transform.position);
            float m1 = rigidbody1.mass;
            float m2 = rigidbody2.mass;
            float F = (m1 * m2*MassMultyPlayer) / (R * R);
            gameObject.GetComponent<Rigidbody>().AddForce(direction * F, ForceMode.Force);

            float r2 = collider.transform.gameObject.GetComponent<Gravity>().r;

            if (R > r + r2 + 6f)
            {
                ToDestroy = false;
            }
            if (R < r + r2+5 & r >= r2 & doNotMarge == false )
            {
                //PlanetGenerator Generator = transform.parent.gameObject.GetComponent<PlanetGenerator>();

                //int planes2 = collider.transform.parent.gameObject.GetComponent<Gravity>().planes;
                //Generator.GenerateCube(planes2 + planes, PlanetGenerator.ConstantSubdivisions, r + r2);

                if (ToDestroy == true) return;
                rigidbody2.gameObject.GetComponent<Gravity>().ToDestroy = true;
                Vector3 v2i = rigidbody2.velocity;
                if(rigidbody2.transform.childCount == 4) rigidbody2.transform.GetChild(3).parent = transform;
                Destroy(collider.transform.gameObject);

                Vector3 v1i = rigidbody1.velocity;
                rigidbody1.AddForce(v1i +((v2i - v1i)/2f / m1 * m2) + v1i*-1, ForceMode.VelocityChange);

                float ScaleTo = Mathf.Pow((m1 + m2) / m1-1f, 1f / 3f);
                transform.localScale += ScaleTo * new Vector3(1f, 1f, 1f);
                rigidbody1.mass = m1 + m2;
                r *=  (m1 + m2)/ m1;
                ToDestroy = true;

                //((m1 - m2) / (m1 + m2)) * v1i + ((2 * m2) / (m1 + m2)) * v2i

                //Destroy(gameObject);
            }

        }
    }
}
