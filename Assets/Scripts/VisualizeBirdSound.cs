using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class VisualizeBirdSound : MonoBehaviour
{
    public AudioSource sound;
    public GameObject particles;
    public Light gameObjectLight;
    public float min_seconds_between_plays = 5f;
    public float max_seconds_between_plays = 20f;
    public float seconds_audio_plays = 3f;
    // Start is called before the first frame update

    private void OnEnable()
    {
        NetworkPlayerSpawner.DeafPlayerSpawned += setDeafPlayer;
    }

    private void OnDisable()
    {
        NetworkPlayerSpawner.DeafPlayerSpawned -= setDeafPlayer;
    }

    private void setDeafPlayer()
    {
        if (sound != null)
        {
            sound.mute = true;
        }
    }

    IEnumerator Start()
    {
        while (true)
        {
            float seconds_between_plays = Random.Range(min_seconds_between_plays, max_seconds_between_plays);
            yield return new WaitForSeconds(seconds_between_plays);
            PlayAndVisualize();
        }
    }

    private void PlayAndVisualize()
    {
        sound.Play();

        GameObject thisObjCopy = UtilFunctions.CreateCopyWithTransformAndMesh(this.gameObject, this.gameObject.transform.parent);
        UtilFunctions.ChangeLayersRecursively(thisObjCopy.transform, "Blind Layer");

        GameObject thisObjectLight = Instantiate(gameObjectLight.gameObject, thisObjCopy.transform);
        GameObject inst_particales = Instantiate(particles, this.transform.position, Quaternion.identity, thisObjCopy.transform);
        UtilFunctions.ChangeLayersRecursively(inst_particales.transform, "Blind Layer");

        StartCoroutine(WaitAndDestroy(seconds_audio_plays, thisObjCopy, thisObjectLight));
    }

    private IEnumerator WaitAndDestroy(float waitTime, GameObject thisObject, GameObject thisObjectLight)
    {
        yield return new WaitForSeconds(waitTime);
        sound.Stop();
        Destroy(thisObjectLight);
        Destroy(thisObject);
    }
}
