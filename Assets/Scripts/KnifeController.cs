using System.Collections;
using UnityEngine;

public class KnifeController : MonoBehaviour
{
    #region External References
    [SerializeField] private Transform knifeTransform;
    #endregion
    #region Variables
    Animator m_animator;
    private Vector3 startPosition;
    private Coroutine currentCutCoroutine;
    private bool isCutting = false;
    #endregion

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
                m_animator.ResetTrigger("Cut");
                m_animator.Play("Idle", 0, 0f);
            }
            else
            {
                m_animator.ResetTrigger("Cut");
                m_animator.Play("Idle", 0, 0f);
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
        // D�place uniquement en X, laisse Y et Z g�r�s par l'Animator
        Vector3 targetPosition = new Vector3(cutPoint.x, knifeTransform.position.y, knifeTransform.position.z);

        float elapsedTime = 0;
        float moveDuration = 0.05f;
        Vector3 initialPosition = knifeTransform.position;

        // D�placement rapide en X vers la position de coupe
        while (elapsedTime < moveDuration)
        {
            knifeTransform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        knifeTransform.position = targetPosition;

        // Pause pendant la coupe
        yield return new WaitForSeconds(0.2f);

        // Retour � la position de d�part
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
    }

    private void StopCutAnimation()
    {
        StopCoroutine(currentCutCoroutine);
        m_animator.ResetTrigger("Cut");
        m_animator.Play("Idle", 0, 0f);
        isCutting = false;
    }
}
