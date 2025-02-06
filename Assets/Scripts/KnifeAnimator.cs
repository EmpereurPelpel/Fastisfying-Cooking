using System.Collections;
using UnityEngine;

public class KnifeAnimator : MonoBehaviour
{
    [SerializeField] private MHFixedCutter cutterScript;
    [SerializeField] private Transform knifeTransform;
    [SerializeField] private float cutDuration;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = knifeTransform.position;
    }

    public void AnimateCut(Vector3 cutPoint)
    {
        StartCoroutine(CutAnimation(cutPoint));
    }

    private IEnumerator CutAnimation(Vector3 cutPoint)
    {
        Vector3 targetPosition = new Vector3(cutPoint.x, knifeTransform.position.z, knifeTransform.position.z);

        //  Descente (0.1s)
        float elapsedTime = 0;
        float descentDuration = 0.05f;
        Vector3 initialPosition = knifeTransform.position;

        while (elapsedTime < descentDuration)
        {
            knifeTransform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / descentDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        knifeTransform.position = targetPosition;

        //  Pause après la coupe
        yield return new WaitForSeconds(0.2f);

        // Remontée (0.15s)
        elapsedTime = 0;
        float ascentDuration = 0.2f;

        while (elapsedTime < ascentDuration)
        {
            knifeTransform.position = Vector3.Lerp(targetPosition, startPosition, elapsedTime / ascentDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        knifeTransform.position = startPosition;
    }


}
