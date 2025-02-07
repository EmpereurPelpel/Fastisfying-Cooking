using UnityEngine;
using System.Collections;
using TMPro;
using static RythmScript;

public class ObjectSpawner : MonoBehaviour
{
    #region External References
    [SerializeField] private GameObject[] objectPrefabs; // Prefab à instancier
    [SerializeField] private MHFixedCutter cutterScript; // Référence au script de découpe (attaché à "Mesh Slicer")
    [SerializeField] private RythmScript rythmScript;
    #endregion
    #region Variables
    private GameObject currentObject; // Stocke l’objet instancié
    private GameObject currentContainer;
    private GameObject lastObject;
    private GameObject lastContainer;

    private Vector3 startPos = new Vector3(-6, -0.25f, 0);
    private Vector3 medPos = new Vector3(0, -0.25f, 0);
    private Vector3 endPos = new Vector3(6, -0.25f, 0);

    private bool firstObjectIsSpawned = false;
    private bool objectIsMoving = false;
    #endregion

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
        currentContainer.transform.position = startPos;
        GameObject randomPrefab = objectPrefabs[Random.Range(0, objectPrefabs.Length)];
        currentObject = Instantiate(randomPrefab, startPos, Quaternion.identity); // Instancie un nouvel objet
        currentObject.transform.parent = currentContainer.transform;
        cutterScript.SetTargetObject(currentObject,numberOfBeats); // Associe l’objet instancié au cutter
        StartCoroutine(MoveNewFood(currentContainer, medPos, rythmScript.GetIntervalLength() ));
    }

    public void ResetObject()
    {
        if (lastObject != null)
        {
            StartCoroutine(MoveAndDestroy(lastContainer, endPos, rythmScript.GetIntervalLength()));
        }
    }

    private IEnumerator MoveNewFood(GameObject obj, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = obj.transform.position;
        float elapsedTime = 0f;
        objectIsMoving = true;
        while (elapsedTime < duration)
        {
            obj.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
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
            yield return null;
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
                part.transform.parent=currentContainer.transform;
            }
        }
    }

    private bool IsCuttingComplete()
    {
        return cutterScript != null && cutterScript.CurrentCutIndex >= cutterScript.NumberOfCuts;
    }
}
