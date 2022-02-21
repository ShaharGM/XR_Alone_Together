using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    private float backgroundRVal = 0.1482987f;
    private float backgroundGVal = 0.1081346f;
    private float backgroundBVal = 0.509434f;
    private float backgroundAVal = 0f;

    private GameObject spawnedPlayerPrefab;
    public TrailRenderer trailEffect;
    public GameObject cameraOffset;
    public Transform headTransform;
    public XRRig playerRig;
    public Transform blind_spawn;
    public Transform deaf_spawn;
    public Light worldLight;
    public Light personalLight;
    public Canvas UI;
    public Canvas heatWaveEffect;

    public delegate void EventReaction();
    public static event EventReaction DeafPlayerSpawned;

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        
        if (!PhotonNetwork.IsMasterClient)
        {
            worldLight.enabled = false;
            Camera.main.cullingMask = ((1 << LayerMask.NameToLayer("Blind Layer")) | (1 << LayerMask.NameToLayer("UI")));
            Camera.main.clearFlags = CameraClearFlags.SolidColor;
            Camera.main.backgroundColor = new Color(backgroundRVal, backgroundGVal, backgroundBVal, backgroundAVal);

            playerRig.transform.position = blind_spawn.position;

            UI.transform.position = playerRig.transform.position + new Vector3(0f, -1.5f, 4f);

            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player Blind", blind_spawn.position, playerRig.transform.rotation);
            Light personalLightInstante = Instantiate(personalLight, cameraOffset.transform);
            Instantiate(trailEffect, cameraOffset.transform);
            UtilFunctions.ChangeLayersRecursively(cameraOffset.transform, "Blind Layer");
        } 
        else
        {
            playerRig.transform.position = deaf_spawn.position;

            UI.transform.position = playerRig.transform.position + new Vector3(0f, -1.5f, 4f);

            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", deaf_spawn.position, playerRig.transform.rotation);
            if(DeafPlayerSpawned != null)
            {
                DeafPlayerSpawned();
            }
            Camera.main.cullingMask ^= (1 << LayerMask.NameToLayer("Blind Layer"));
            Instantiate(heatWaveEffect, Camera.main.transform);
        }
        
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(spawnedPlayerPrefab);
    }
}
