using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject player;
    public GameObject exitPortal;

    private AudioSource audioSource;
    public AudioClip portalSound;

    public float portalWarpVolume;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(PortalWarp());
        StartCoroutine(ChangeLocation());
        audioSource.PlayOneShot(portalSound, portalWarpVolume);
    }
    private IEnumerator ChangeLocation()
    {
        yield return new WaitForSeconds(1);
        player.transform.position = exitPortal.transform.position + new Vector3(0, 1, 0);
    }
    private IEnumerator PortalWarp()
    {
        yield return new WaitForSeconds(1);

        audioSource.PlayOneShot(portalSound, portalWarpVolume);
        SceneManager.LoadScene("FirstBattle");

    }
}
