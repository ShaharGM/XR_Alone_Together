using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Rendering;
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
    public Terrain deafTerrain;
    public Terrain blindTerrain;
    public Terrain deafBeachTerrain;
    public Terrain blindBeachTerrain;
    public GameObject sunExposureController;

    public delegate void EventReaction();
    public static event EventReaction DeafPlayerSpawned;

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        
        if (PhotonNetwork.IsMasterClient)
        {
            deafTerrain.gameObject.SetActive(false);
            deafBeachTerrain.gameObject.SetActive(false);
            worldLight.enabled = false;
            RenderSettings.skybox = (null);
            RenderSettings.fog = false;
            Camera.main.cullingMask = ((1 << LayerMask.NameToLayer("Blind Layer")) | (1 << LayerMask.NameToLayer("UI")));
            Camera.main.clearFlags = CameraClearFlags.SolidColor;
            Camera.main.backgroundColor = new Color(backgroundRVal, backgroundGVal, backgroundBVal, backgroundAVal);

            playerRig.transform.position = blind_spawn.position;

            UI.transform.position = playerRig.transform.position + new Vector3(0f, 0f, 4f);

            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player Blind", blind_spawn.position, playerRig.transform.rotation);
            Light personalLightInstante = Instantiate(personalLight, cameraOffset.transform);
            Instantiate(trailEffect, cameraOffset.transform);
            UtilFunctions.ChangeLayersRecursively(cameraOffset.transform, "Blind Layer");
        } 
        else
        {
            blindTerrain.gameObject.SetActive(false);
            blindBeachTerrain.gameObject.SetActive(false);
            playerRig.transform.position = deaf_spawn.position;

            UI.transform.position = playerRig.transform.position + new Vector3(0f, -1.5f, 4f);

            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", deaf_spawn.position, playerRig.transform.rotation);
            Instantiate(sunExposureController);
            if (DeafPlayerSpawned != null)
            {
                DeafPlayerSpawned();
            }
            Camera.main.cullingMask ^= (1 << LayerMask.NameToLayer("Blind Layer"));
        }
        
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(spawnedPlayerPrefab);
    }
}
