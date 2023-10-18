using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlayer : MonoBehaviour
{
    public float health = 100;
    public Material playerOutfit = null;
    public Collider playerCollider = null;
    public float rivivalInterval = 2f;
    public bool isRivive = false;

    private IEnumerator RivivePlayer()
    {
        if(!isRivive)
        {
            isRivive = true;

            playerOutfit.color = Color.red;
            playerCollider.enabled = false;

            yield return new WaitForSeconds(rivivalInterval);

            health = 100;
            playerOutfit.color = Color.white;
            playerCollider.enabled = true;

            isRivive = false;
        }
    }

    private void Awake()
    {
        playerOutfit = GetComponent<Renderer>().material;
        playerCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (health <= 0f)
            StartCoroutine(RivivePlayer());
    }
}
