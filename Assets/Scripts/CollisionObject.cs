using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollisionObject : MonoBehaviour
{
    public AABB m_collider;

	// Use this for initialization
	void Start ()
    {
        m_collider = new AABB(5.0f, 5.0f);
    }
	
	// Update is called once per frame
	void Update ()
    {
        m_collider.UpdatePosition(transform.position);
        m_collider.TestOverlap(m_collider, m_collider);
    }
}
