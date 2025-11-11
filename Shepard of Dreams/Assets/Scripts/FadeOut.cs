/**
  * Author: Benjamin Albeyta
  * Project Members: Caroline Jia, Benjamin Albeyta, Sophia Qian
  * Date Created: 11/10/2025
  * Date Last Updated: 11/10/2025
  * Summary: Gradually fades out an object 
  */


using UnityEngine;

public class FadeOut : MonoBehaviour
{
    private Material mat;
    private Color startColor;
    public float duration = 0.4f;
    private float t;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        startColor = mat.color;
    }

    void Update()
    {
        t += Time.deltaTime / duration;
        Color c = startColor;
        c.a = Mathf.Lerp(startColor.a, 0, t);
        mat.color = c;

        if (t >= 1f)
            Destroy(gameObject);
    }
}
