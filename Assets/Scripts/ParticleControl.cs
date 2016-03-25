using UnityEngine;
using System.Collections;

public class ParticleControl : MonoBehaviour
{
    public GameObject m_Target;

    private Quaternion m_lookRotation;
    private Vector3 m_direction;

    void Start()
    {
        m_Target = GameObject.Find("PlaagGeest");
    }

    void Update()
    {
        m_direction = (m_Target.transform.position - transform.position).normalized;
        m_lookRotation = Quaternion.LookRotation(m_direction);
        transform.rotation = m_lookRotation;
    }
}