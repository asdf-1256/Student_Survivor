using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet_DataStructure : BulletBase
{
    LineRenderer lineRenderer;

    public int targetCount = 5;
    //private WaitForSeconds wait = new WaitForSeconds(0.15f);
    private WaitForFixedUpdate wait = new WaitForFixedUpdate();

    List<Transform> hitedTargets;
    Vector3[] positions;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        hitedTargets = new List<Transform>();
        positions = new Vector3[targetCount];
    }
    private void Update()
    {
        if (hitedTargets.Count < 1)
            return;
        for (int i = 0; i < targetCount; i++) {
            positions[i] = hitedTargets[Mathf.Min(i, hitedTargets.Count - 1)].position;
        }
        lineRenderer.SetPositions(positions);
    }
    private void OnEnable()
    {
        if (lineRenderer.positionCount != targetCount || positions.Length != targetCount)
        {
            Array.Resize(ref positions, targetCount);
        }
        if (GameManager.Instance.player.scanner.nearestTarget == null)
            return;
        hitedTargets.Add(GameManager.Instance.player.scanner.nearestTarget);
        transform.position = GameManager.Instance.player.scanner.nearestTarget.position;

        lineRenderer.positionCount = targetCount;

        StartCoroutine(RayRoutine(() => { gameObject.SetActive(false); }));
    }
    private void OnDisable()
    {
        hitedTargets.Clear();
        for (int i = 0; i < targetCount; i++) { positions[i] = Vector3.zero; }
        lineRenderer.positionCount = 0;
    }
    /*
    IEnumerator FirstRayRoutine()
    {
        Vector3[] firstLine = { transform.position, GameManager.Instance.player.transform.position };
        lineRenderer.SetPositions(firstLine);

        yield return wait;

        lineRenderer.positionCount = targetCount;

        yield return StartCoroutine(RayRoutine(() => { gameObject.SetActive(false); }));
    }*/
    IEnumerator RayRoutine(System.Action done)
    {
        for (int i = 1; i < targetCount; i++)
        {
            Transform nextTarget = GameManager.Instance.player.scanner.GetNearTargetFromNotHitedEnemy(hitedTargets);
            if (nextTarget != null)
            {
                hitedTargets.Add(nextTarget);
                transform.position = nextTarget.position;
            }
            yield return wait;
            yield return wait;
        }

        done.Invoke();
    }
}
