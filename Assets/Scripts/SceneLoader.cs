using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject player;
    public GameObject exitPortal;

    private void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(ChangeLocation());
        SceneManager.LoadScene("FirstBattle");
    }
    private IEnumerator ChangeLocation()
    {
        yield return new WaitForSeconds(1);
        player.transform.position = exitPortal.transform.position + new Vector3(0, 1, 0);
    }
}
