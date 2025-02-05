using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Prefab et Cutter")]
    [SerializeField] private GameObject objectPrefab; // Prefab � instancier
    [SerializeField] private MHFixedCutter cutterScript; // R�f�rence au script de d�coupe (attach� � "Mesh Slicer")

    private GameObject currentObject; // Stocke l�objet instanci�
    private bool isObjectDestroyed = false; // Permet de savoir si l'objet a �t� supprim�

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
                    // Premier clic apr�s la derni�re coupe -> Supprime l'objet
                    ResetObject();
                    isObjectDestroyed = true; // Marque l�objet comme supprim�
                }
                else
                {
                    // Deuxi�me clic -> Cr�e un nouvel objet pr�t � �tre coup�
                    SpawnNewObject();
                    isObjectDestroyed = false; // R�initialise l'�tat
                }
            }
        }
    }

    private void SpawnNewObject()
    {
        currentObject = Instantiate(objectPrefab, Vector3.zero, Quaternion.identity); // Instancie un nouvel objet
        cutterScript.SetTargetObject(currentObject); // Associe l�objet instanci� au cutter
    }

    private void ResetObject()
    {
        cutterScript.DestroySlicedParts();
        Destroy(currentObject); // Supprime l�objet coup�
    }

    private bool IsCuttingComplete()
    {
        return cutterScript != null && cutterScript.CurrentCutIndex >= cutterScript.NumberOfCuts;
    }
}
