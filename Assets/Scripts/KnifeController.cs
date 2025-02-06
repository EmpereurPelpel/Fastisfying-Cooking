using System.Collections;
using UnityEngine;

public class KnifeController : MonoBehaviour
{
    Animator m_animator;
    [SerializeField] private Transform knifeTransform;
    private Vector3 startPosition;
    private Coroutine currentCutCoroutine;
    private bool isCutting = false;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        startPosition = knifeTransform.position; // Sauvegarde la position initiale
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isCutting)
            {
                StopCutAnimation();
            }
            else
            {
                m_animator.SetTrigger("Cut");
            }
        }
    }

    public IEnumerator StartCutAnimation(Vector3 cutPoint)
    {
        m_animator.SetTrigger("Cut");
        isCutting = true;
        currentCutCoroutine = StartCoroutine(MoveKnife(cutPoint));
        yield return currentCutCoroutine;
    }

    public IEnumerator MoveKnife(Vector3 cutPoint)
    {
        // Déplace uniquement en X, laisse Y et Z gérés par l'Animator
        Vector3 targetPosition = new Vector3(cutPoint.x, knifeTransform.position.y, knifeTransform.position.z);
        Debug.Log("Déplacement du couteau vers : " + targetPosition);

        float elapsedTime = 0;
        float moveDuration = 0.05f;
        Vector3 initialPosition = knifeTransform.position;

        // Déplacement rapide en X vers la position de coupe
        while (elapsedTime < moveDuration)
        {
            knifeTransform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        knifeTransform.position = targetPosition;

        // Pause pendant la coupe
        yield return new WaitForSeconds(0.2f);

        // Retour à la position de départ
        elapsedTime = 0;
        float returnDuration = 0.1f; // Retour un peu plus lent

        while (elapsedTime < returnDuration)
        {
            knifeTransform.position = Vector3.Lerp(targetPosition, startPosition, elapsedTime / returnDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        knifeTransform.position = startPosition;
        isCutting = false;
        Debug.Log("Couteau revenu à la position initiale : " + startPosition);
    }

    private void StopCutAnimation()
    {
        StopCoroutine(currentCutCoroutine);
        m_animator.ResetTrigger("Cut");
        m_animator.Play("Idle", 0, 0f);
        isCutting = false;
    }
}
