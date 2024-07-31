using UnityEngine;

public class PortalActivator : MonoBehaviour
{
    public GameObject portal;

    void Start()
    {
        portal.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        portal.SetActive(true);
    }

}


