using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DoorGoal : MonoBehaviour
{
    public Renderer doorRenderer;           
    public Color idleColor = Color.red;     
    public Color openColor = Color.green;   
    public float reloadDelay = 0.6f;        

    void Reset()
    {
       
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    void Start()
    {
        if (!doorRenderer) doorRenderer = GetComponentInChildren<Renderer>();
        if (doorRenderer) doorRenderer.material.color = idleColor;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("[Goal] Door touched -> Win");
        if (doorRenderer) doorRenderer.material.color = openColor;
        StartCoroutine(WinRoutine());
    }

    IEnumerator WinRoutine()
    {
        yield return new WaitForSeconds(reloadDelay);
        GameManager.Instance.CompleteLevel();
    }
}
