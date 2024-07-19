using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string location;

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
        audioSource.PlayOneShot(portalSound, portalWarpVolume);
    }

    private IEnumerator PortalWarp()
    {
        yield return new WaitForSeconds(1);

        audioSource.PlayOneShot(portalSound, portalWarpVolume);
        SceneManager.LoadScene(location);

    }
}
