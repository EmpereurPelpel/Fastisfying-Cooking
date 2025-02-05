using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Prefab et Cutter")]
    [SerializeField] private GameObject objectPrefab; // Prefab à instancier
    [SerializeField] private MHFixedCutter cutterScript; // Référence au script de découpe (attaché à "Mesh Slicer")

    private GameObject currentObject; // Stocke l’objet instancié
    private bool isObjectDestroyed = false; // Permet de savoir si l'objet a été supprimé

    private void Start()
    {
        SpawnNewObject(); // Instancier le premier objet
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (cutterScript != null && IsCuttingComplete()) // Si toutes les coupes sont faites
            {
                if (!isObjectDestroyed)
                {
                    // Premier clic après la dernière coupe -> Supprime l'objet
                    ResetObject();
                    isObjectDestroyed = true; // Marque l’objet comme supprimé
                }
                else
                {
                    // Deuxième clic -> Crée un nouvel objet prêt à être coupé
                    SpawnNewObject();
                    isObjectDestroyed = false; // Réinitialise l'état
                }
            }
        }
    }

    private void SpawnNewObject()
    {
        currentObject = Instantiate(objectPrefab, Vector3.zero, Quaternion.identity); // Instancie un nouvel objet
        cutterScript.SetTargetObject(currentObject); // Associe l’objet instancié au cutter
    }

    private void ResetObject()
    {
        cutterScript.DestroySlicedParts();
        Destroy(currentObject); // Supprime l’objet coupé
    }

    private bool IsCuttingComplete()
    {
        return cutterScript != null && cutterScript.CurrentCutIndex >= cutterScript.NumberOfCuts;
    }
}
