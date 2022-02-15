using UnityEngine;

public static class UtilFunctions
{
    public static void ChangeLayersRecursively(Transform trans, string layerName)
    {
        foreach (Transform child in trans.gameObject.GetComponentInChildren<Transform>())
        {
            ChangeLayersRecursively(child, layerName);
        }
        trans.gameObject.layer = LayerMask.NameToLayer(layerName);
    }

    public static GameObject CreateCopyWithTransformAndMesh(GameObject gameObject, Transform copyParent)
    {
        GameObject copiedObj = null;
        if ((gameObject.tag != "Copy"))
        {
            copiedObj = new GameObject(gameObject.name + "(Clone)");
            copiedObj.transform.parent = copyParent;
            copiedObj.transform.position = gameObject.transform.position;
            copiedObj.transform.rotation = gameObject.transform.rotation;
            copiedObj.transform.localScale = gameObject.transform.localScale; // since both objects will be under the same parent
            if (gameObject.GetComponent<MeshFilter>() != null) 
            {
                copiedObj.AddComponent<MeshFilter>().mesh = gameObject.GetComponent<MeshFilter>().mesh;
            }
            if (gameObject.GetComponent<MeshRenderer>() != null)
            {
                copiedObj.AddComponent<MeshRenderer>().material = gameObject.GetComponent<MeshRenderer>().material;
            }
            copiedObj.tag = "Copy";

            foreach (Transform child in gameObject.GetComponentInChildren<Transform>())
            {
                CreateCopyWithTransformAndMesh(child.gameObject, copiedObj.transform);
            }
        }
        return copiedObj;
    }
}
