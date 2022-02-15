using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane : MonoBehaviour
{
    public ParticleSystem fire;
    public ParticleSystem smoke;
    public AudioSource explosion_sound;
    public Light fire_light;
    public GameObject blind_particles;
    public Light gameObjectLight;
    // Start is called before the first frame update
    private void OnEnable()
    {
        EventManager.AirplaneExplosion += ActivateExplosion;
        NetworkPlayerSpawner.DeafPlayerSpawned += setDeafPlayer;
    }

    private void OnDisable()
    {
        EventManager.AirplaneExplosion -= ActivateExplosion;
        NetworkPlayerSpawner.DeafPlayerSpawned -= setDeafPlayer;
    }

    private void setDeafPlayer()
    {
        if (explosion_sound != null)
        {
            explosion_sound.mute = true;
        }
    }

    private void ActivateExplosion()
    {
        explosion_sound.Play();
        

        GameObject thisObjCopy = UtilFunctions.CreateCopyWithTransformAndMesh(this.gameObject, this.gameObject.transform.parent);
        UtilFunctions.ChangeLayersRecursively(thisObjCopy.transform, "Blind Layer");

        GameObject thisObjectLight = Instantiate(gameObjectLight.gameObject, thisObjCopy.transform);
        GameObject inst_particales = Instantiate(blind_particles, this.transform.position, Quaternion.identity, thisObjCopy.transform);
        UtilFunctions.ChangeLayersRecursively(inst_particales.transform, "Blind Layer");

        StartCoroutine(WaitAndDestroy(explosion_sound.clip.length, thisObjCopy, thisObjectLight));

        fire.Play();
        fire_light.enabled = true;
        smoke.Play();
    }

    private IEnumerator WaitAndDestroy(float waitTime, GameObject thisObject, GameObject thisObjectLight)
    {
        yield return new WaitForSeconds(waitTime);
        explosion_sound.Stop();
        Destroy(thisObjectLight);
        Destroy(thisObject);
    }
}
