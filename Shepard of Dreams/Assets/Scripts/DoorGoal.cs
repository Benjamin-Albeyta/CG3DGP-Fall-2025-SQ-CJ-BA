/**
  * Author: Sophia Qian, Benjamin Albeyta
  * Project Members: Caroline Jia, Benjamin Albeyta, Sophia Qian
  * Date Created: 9/20/2025
  * Date Last Updated: 10/12/2025
  * Summary: Responsible for keeping track of the end level objective, when player touches it, causes them to end the level and move on to the next one, through calling GameManager
  */

using System.Collections;
using UnityEngine;
using Benjathemaker;

[RequireComponent(typeof(Collider))]
public class DoorGoal : MonoBehaviour
{      
    public float reloadDelay = 0.6f;

    public GameObject Clock;

    private SimpleGemsAnim clockScript;        

    void Reset()
    {
       
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    void Start()
    {
        clockScript = Clock.GetComponentInChildren<SimpleGemsAnim>();

        clockScript.isRotating = false;
        clockScript.isFloating = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("[Goal] Door touched -> Win");
        clockScript.isRotating = true;
        clockScript.isFloating = true;
        StartCoroutine(WinRoutine());
    }

    IEnumerator WinRoutine()
    {
        yield return new WaitForSeconds(reloadDelay);
        GameManager.Instance.CompleteLevel();
    }
}
