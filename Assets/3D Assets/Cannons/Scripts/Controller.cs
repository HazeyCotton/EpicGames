using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameObject[] m_ShootPS;
    public GameObject[] m_Cannons;
    public GameObject m_FireButton;

    private int m_ID = 0;

    void Start()
    {
        m_FireButton.SetActive(true);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            SwitchCannon(-1);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            SwitchCannon(1);
        else if (Input.GetKeyDown(KeyCode.Space))
            Shoot();
    }
    public void SwitchCannon(int _Next)
    {
        CancelInvoke("StopShoot");

        m_Cannons[m_ID].SetActive(false);

        if(m_ID != m_Cannons.Length - 1)
        m_ShootPS[m_ID].SetActive(false);

        m_ID += _Next;

        m_ID = Mathf.Clamp(m_ID, 0, m_Cannons.Length -1);

        if (m_ID == m_Cannons.Length - 1)
            m_FireButton.SetActive(false);
        else
            m_FireButton.SetActive(true);

        m_Cannons[m_ID].SetActive(true);
    }

    public void Shoot()
    {
        if (IsInvoking("StopShoot") || 
            m_ID == m_Cannons.Length - 1)
            return;

        m_ShootPS[m_ID].SetActive(true);

        Invoke("StopShoot", 3f);
    }

    void StopShoot()
    {
        m_ShootPS[m_ID].SetActive(false);
    }

}
