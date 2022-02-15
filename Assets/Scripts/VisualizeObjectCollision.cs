using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class VisualizeObjectCollision : MonoBehaviour
{
    public AudioSource sound;
    public GameObject particles;

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

    private void OnCollisionEnter(Collision collision)
    {
        sound.Play();

        GameObject thisObjCopy = CreateCopyWithTransformAndMesh(this.gameObject);
        UtilFunctions.ChangeLayersRecursively(thisObjCopy.transform, "Blind Layer");

        GameObject collidedObjCopy = CreateCopyWithTransformAndMesh(collision.gameObject);
        UtilFunctions.ChangeLayersRecursively(collidedObjCopy.transform, "Blind Layer");

        StartCoroutine(WaitAndDestroy(2, thisObjCopy, collidedObjCopy));
        GameObject inst_particales = Instantiate(particles, this.transform.position, Quaternion.identity);
        UtilFunctions.ChangeLayersRecursively(inst_particales.transform, "Blind Layer");
    }

    private IEnumerator WaitAndDestroy(float waitTime, GameObject thisObject, GameObject collidingObject)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(thisObject);
        Destroy(collidingObject);
    }

    private GameObject CreateCopyWithTransformAndMesh(GameObject gameObject)
    {
        GameObject copiedObj = new GameObject(gameObject.name + "(Clone)");
        copiedObj.transform.position = gameObject.transform.position;
        copiedObj.transform.rotation = gameObject.transform.rotation;
        copiedObj.transform.localScale = Vector3.one;
        copiedObj.AddComponent<MeshFilter>().mesh = gameObject.GetComponent<MeshFilter>().mesh;
        copiedObj.AddComponent<MeshRenderer>().material = gameObject.GetComponent<MeshRenderer>().material;
        return copiedObj;
    }
}
