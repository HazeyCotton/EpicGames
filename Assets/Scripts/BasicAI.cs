using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicAI : MonoBehaviour
{
    NavMeshAgent ai;

    public GameObject[] locations;

    public int currObject;

    public enum AIState
    {
        SpeedBoost,
        FinishFlag
    }

    public AIState aiState;

    // Start is called before the first frame update
    void Start()
    {
        aiState = AIState.SpeedBoost;

        ai = GetComponent<NavMeshAgent>();
        currObject = 0;

        SetNextLocation();
    }

    // Update is called once per frame
    void Update()
    {
        switch (aiState)
        {
            case AIState.SpeedBoost:
                if (!locations[currObject].activeInHierarchy && !(currObject == 3))
                { 
                    currObject++;
                    Debug.Log("Imagine if i had a real weapon");
                }
                SetNextLocation();
                break;

            case AIState.FinishFlag:
                SetNextLocation();
                break;
        }
    }

    private void SetNextLocation()
    {
        ai.SetDestination(locations[currObject].transform.position);

        float AIPos = this.transform.position.magnitude;
        float locationPos = locations[currObject].transform.position.magnitude;
        float errorBound = 0.5f;
        if (AIPos >= locationPos - errorBound && AIPos <= locationPos + errorBound)
        {
            if (!locations[currObject].CompareTag("Finish"))
            {
                locations[currObject].SetActive(false);
            }
        }
    }
}
