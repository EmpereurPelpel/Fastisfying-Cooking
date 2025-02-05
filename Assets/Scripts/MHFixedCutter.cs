using com.marufhow.meshslicer.core;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MHFixedCutter : MonoBehaviour
{
    #region Configuration
    [SerializeField] private MHCutter mhCutter;
    [SerializeField] private GameObject targetObject;
    [SerializeField] private int numberOfCuts;
    [SerializeField] private Vector3 cutDirection = Vector3.right;
    [SerializeField] private float objectLength;
    #endregion
    #region Variables
    private int currentCutIndex = 0;
    private List<Vector3> cutPoints = new List<Vector3>();
    #endregion

    private void Start()
    {
        objectLength = GetObjectLengthX(targetObject);
        Debug.Log(objectLength);
        cutPoints = DetermineCutPoints(objectLength, numberOfCuts);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && (currentCutIndex < numberOfCuts))
        {
            Debug.Log(cutPoints[currentCutIndex]);
            MakeCut(cutPoints[currentCutIndex]);
            currentCutIndex++;
        }
    }
    public void SetTargetObject(GameObject newTarget)
    {
        targetObject = newTarget;
        currentCutIndex = 0;
        objectLength = GetObjectLengthX(targetObject);
        cutPoints = DetermineCutPoints(objectLength, numberOfCuts);
    }

    private float GetObjectLengthX(GameObject targetObject)
    {
        if (targetObject.TryGetComponent<Renderer>(out Renderer renderer))
        {
            return renderer.bounds.size.x; // Taille totale sur X
        }
        return 0f;
    }

    private List<Vector3> DetermineCutPoints(float objectLength, int numberOfCuts)
    {
        List<Vector3> cutPoints = new List<Vector3>();
        for (int i = 1; i <= numberOfCuts; i++)
        {
            float x = (objectLength / 2) - ((float)i / (numberOfCuts + 1) * objectLength);
            cutPoints.Add(new Vector3(x, 0f, 0f));
        }
        return cutPoints;
    }

    private void MakeCut(Vector3 cutPoint)
    {
        mhCutter.Cut(targetObject, cutPoint, cutDirection);
    }
    public void DestroySlicedParts()
    {
        GameObject[] slicedParts = GameObject.FindGameObjectsWithTag("Sliced");

        if (slicedParts.Length == 0)
        {
            Debug.Log("Aucun objet 'Sliced' trouv� !");
        }
        else
        {
            foreach (GameObject part in slicedParts)
            {
                Debug.Log("Suppression de : " + part.name);
                Destroy(part);
            }
        }
    }

    public int CurrentCutIndex => currentCutIndex;
    public int NumberOfCuts => numberOfCuts;

}