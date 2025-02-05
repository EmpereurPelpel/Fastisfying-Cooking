using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Prefab et Cutter")]
    [SerializeField] private GameObject objectPrefab; // Prefab à instancier
    [SerializeField] private MHFixedCutter cutterScript; // Référence au script de découpe (attaché à "Mesh Slicer")

    private GameObject currentObject; // Stocke l’objet instancié
    private bool firstObjectIsSpawned = false;

    private void Start()
    {

    }

    private void Update()
    {
       
    }
    public bool FirstObjectIsSpawned()
    {
        return firstObjectIsSpawned;
    }
    public void SpawnNewObject(int numberOfBeats)
    {
        firstObjectIsSpawned = true;
        currentObject = Instantiate(objectPrefab, Vector3.zero, Quaternion.identity); // Instancie un nouvel objet
        cutterScript.SetTargetObject(currentObject,numberOfBeats); // Associe l’objet instancié au cutter
    }

    public void ResetObject()
    {
        cutterScript.DestroySlicedParts();
        Destroy(currentObject); // Supprime l’objet coupé
    }

    private bool IsCuttingComplete()
    {
        return cutterScript != null && cutterScript.CurrentCutIndex >= cutterScript.NumberOfCuts;
    }
}
