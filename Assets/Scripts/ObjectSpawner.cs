using UnityEngine;
using System.Collections;
using TMPro;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Prefab et Cutter")]
    [SerializeField] private GameObject objectPrefab; // Prefab à instancier
    [SerializeField] private MHFixedCutter cutterScript; // Référence au script de découpe (attaché à "Mesh Slicer")

    private GameObject currentObject; // Stocke l’objet instancié
    private GameObject currentContainer;
    private GameObject lastObject;
    private GameObject lastContainer;

    private bool firstObjectIsSpawned = false;
    private bool objectIsMoving = false;

    private void Start()
    {

    }

    private void Update()
    {
       
    }
    public bool IsCutAllowed()
    {
        if (firstObjectIsSpawned && !objectIsMoving)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void SpawnNewObject(int numberOfBeats)
    {
        if (firstObjectIsSpawned)
        {
            lastObject = currentObject;
            lastContainer = currentContainer;
        }
        firstObjectIsSpawned = true;
        currentContainer = new GameObject("FoodAndSlicesContainer");
        currentContainer.transform.position = new Vector3(-6,0,0);
        currentObject = Instantiate(objectPrefab, new Vector3(-6,0,0), Quaternion.identity); // Instancie un nouvel objet
        currentObject.transform.parent = currentContainer.transform;
        cutterScript.SetTargetObject(currentObject,numberOfBeats); // Associe l’objet instancié au cutter
        StartCoroutine(MoveNewFood(currentContainer, new Vector3(0, 0, 0), 0.5f));
    }

    public void ResetObject()
    {
        if (lastObject != null)
        {
            StartCoroutine(MoveAndDestroy(lastContainer, new Vector3(6, 0, 0), 0.5f));
        }
    }

    private IEnumerator MoveNewFood(GameObject obj, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = obj.transform.position;
        float elapsedTime = 0f;
        objectIsMoving = true;
        while (elapsedTime < duration)
        {
            Debug.Log("CACAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            obj.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Attendre la prochaine frame
        }
        objectIsMoving = false;
        obj.transform.position = targetPosition;
    }

    private IEnumerator MoveAndDestroy(GameObject obj, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = obj.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            obj.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Attendre la prochaine frame
        }

        obj.transform.position = targetPosition; // S'assurer qu'il atteint bien la position cible
        Destroy(lastContainer); // Supprimer l’objet
    }

    public void ContainSlicedParts()
    {
        GameObject[] slicedParts = GameObject.FindGameObjectsWithTag("Sliced");
        if (slicedParts.Length == 0)
        {
            Debug.Log("Aucun objet 'Sliced' trouvé !");
        }
        else
        {
            foreach (GameObject part in slicedParts)
            {
                Debug.Log(part.name);
                part.transform.parent=currentContainer.transform;
            }
        }
    }

    private bool IsCuttingComplete()
    {
        return cutterScript != null && cutterScript.CurrentCutIndex >= cutterScript.NumberOfCuts;
    }
}
