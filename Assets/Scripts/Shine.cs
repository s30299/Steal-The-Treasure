using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shine : MonoBehaviour
{
    private List<SpriteRenderer> renderers=new();
    [SerializeField] private float changeSpeed = 0.15f;
    [SerializeField] private float minChange = 0.7f;
    private float currentChange = 1;
    private bool goingDown = true;
    private Color baseColor;
    private bool isShining = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        renderers.Add(GetComponent<SpriteRenderer>());
        renderers.Add(GetComponentInChildren<SpriteRenderer>());
        baseColor = renderers[0].color;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShining)
        {
            if (goingDown)
            {
                currentChange -= changeSpeed * Time.deltaTime;
                if (currentChange <= minChange) { goingDown = false; }
            }
            else
            {
                currentChange += changeSpeed * Time.deltaTime;
                if (currentChange >= 1) { goingDown = true; }
            }
            foreach (SpriteRenderer renderer in renderers)
            {
                renderer.color = baseColor * currentChange;
            }
        }
    }
   private void StartShining()
    {
        isShining = true;
    }
    private void StopShining()
    {
        isShining = false;
        foreach (SpriteRenderer renderer in renderers)
        {
            renderer.color = baseColor;
        }
        currentChange = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartShining();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StopShining();
        }
    }
}
