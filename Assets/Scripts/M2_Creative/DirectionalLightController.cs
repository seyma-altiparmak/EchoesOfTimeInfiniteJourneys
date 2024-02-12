using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalLightController : MonoBehaviour
{
    private Light directionalLight;
    private void Awake()
    {
        directionalLight = GetComponent<Light>();
        StartCoroutine(RandomLightColor());
    }

    IEnumerator RandomLightColor()
    {
        while (true)
        {
            Color randomColor = new Color(Random.value, Random.value, Random.value);
            directionalLight.color = randomColor;
            yield return new WaitForSeconds(5f);
        }
    }
}
