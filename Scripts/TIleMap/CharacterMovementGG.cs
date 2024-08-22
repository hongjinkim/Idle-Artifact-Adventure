using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterMovementGG : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public float moveSpeed = 5f;
    private List<Vector3> path;
    private int currentPathIndex;
    public bool isSelected = false;
    public Detection detection;
    public bool autoMove = false;
    public float updatePathInterval = 1f;
    private float lastPathUpdateTime = 0f;

    private CustomTilemapManagerGG customTilemapManager;
    private const float PositionTolerance = 0.1f;
    private const float PositionToleranceSquared = PositionTolerance * PositionTolerance;

    private Vector3 targetPosition;
    private Vector3 currentPosition;
    private Vector3 nearestValidPosition;

    private List<Vector3> previewPath;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        detection = GetComponent<Detection>();

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.yellow;
    }

    void Start()
    {
        //customTilemapManager = new CustomTilemapManagerGG(TilemapManagerGG.Instance, this);
        transform.position = customTilemapManager.GetNearestValidPosition(transform.position);
        StartCoroutine(AutoMoveCoroutine());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            autoMove = !autoMove;
        }
        MoveAlongPath();
        if (path == null || path.Count == 0)
        {
            SnapToNearestTileCenter();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isSelected = true;
        lineRenderer.positionCount = 0;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 endPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        endPosition.z = 0;
        Vector3 nearestValidEndPosition = customTilemapManager.GetNearestValidPosition(endPosition);

        if (customTilemapManager.IsValidMovePosition(nearestValidEndPosition))
        {
            Vector3 start = customTilemapManager.GetNearestValidPosition(transform.position);
            previewPath = customTilemapManager.FindPath(start, nearestValidEndPosition);
            DrawPath(previewPath);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        HandleSelectionAndMovement();
        isSelected = false;

        lineRenderer.positionCount = 0;
    }

    private void DrawPath(List<Vector3> pathToDraw)
    {
        if (pathToDraw != null && pathToDraw.Count > 0)
        {
            lineRenderer.positionCount = pathToDraw.Count;
            lineRenderer.SetPositions(pathToDraw.ToArray());
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }

    void UpdatePath()
    {
        if (detection != null)
        {
            GameObject closestObject = detection.GetClosestObject();
            if (closestObject != null)
            {
                targetPosition = closestObject.transform.position;
                currentPosition = customTilemapManager.GetNearestValidPosition(transform.position);
                if ((currentPosition - targetPosition).sqrMagnitude > PositionToleranceSquared)
                {
                    List<Vector3> surroundingPositions = GetSurroundingPositions(targetPosition);
                    Vector3? bestPosition = FindBestPosition(surroundingPositions, currentPosition);

                    if (bestPosition.HasValue)
                    {
                        SetNewPath(bestPosition.Value);
                    }
                    else
                    {
                        path = null;
                    }
                }
                else
                {
                    transform.position = currentPosition;
                    path = null;
                }
            }
        }
        lastPathUpdateTime = Time.time;
    }

    private Vector3? FindBestPosition(List<Vector3> positions, Vector3 currentPosition)
    {
        Vector3? bestPosition = null;
        float minSqrDistance = float.MaxValue;

        for (int i = 0; i < positions.Count; i++)
        {
            if (customTilemapManager.IsValidMovePosition(positions[i]))
            {
                float sqrDistance = (currentPosition - positions[i]).sqrMagnitude;
                if (sqrDistance < minSqrDistance)
                {
                    minSqrDistance = sqrDistance;
                    bestPosition = positions[i];
                }
            }
        }

        return bestPosition;
    }

    private List<Vector3> GetSurroundingPositions(Vector3 targetPosition)
    {
        List<Vector3> surroundingPositions = new List<Vector3>(8);
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                surroundingPositions.Add(new Vector3(targetPosition.x + x, targetPosition.y + y, targetPosition.z));
            }
        }
        return surroundingPositions;
    }

    public void HandleSelectionAndMovement()
    {
        Vector3 endPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        endPosition.z = 0;
        nearestValidPosition = customTilemapManager.GetNearestValidPosition(endPosition);
        if (customTilemapManager.IsValidMovePosition(nearestValidPosition))
        {
            SetNewPath(nearestValidPosition);
            autoMove = false;
        }
    }

    private void MoveAlongPath()
    {
        if (path != null && currentPathIndex < path.Count)
        {
            Vector3 targetPosition = path[currentPathIndex];
            if (!customTilemapManager.IsValidMovePosition(targetPosition))
            {
                UpdatePath();
                return;
            }
            if ((transform.position - targetPosition).sqrMagnitude > PositionToleranceSquared)
            {
                Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                transform.position = newPosition;
            }
            else
            {
                transform.position = targetPosition;
                currentPathIndex++;
            }
            if (currentPathIndex >= path.Count)
            {
                path = null;
                SnapToNearestTileCenter();
                
                if (autoMove)
                {
                    StartCoroutine(WaitAndFindNewPath());
                }
            }
        }
    }

    private void SnapToNearestTileCenter()
    {
        nearestValidPosition = customTilemapManager.GetNearestValidPosition(transform.position);
        if ((transform.position - nearestValidPosition).sqrMagnitude < PositionToleranceSquared)
        {
            transform.position = nearestValidPosition;
        }
    }

    void SetNewPath(Vector3 target)
    {
        Vector3 start = customTilemapManager.GetNearestValidPosition(transform.position);
        path = customTilemapManager.FindPath(start, target);
        currentPathIndex = 0;

        if (path != null && path.Count > 0)
        {
            TilemapManager.Instance.SetDebugPath(path);
        }
    }

    IEnumerator WaitAndFindNewPath()
    {
        yield return new WaitForSeconds(2f);
        if (detection != null)
        {
            GameObject closestObject = detection.GetClosestObject();
            if (closestObject != null)
            {
                Vector3 targetPosition = closestObject.transform.position;
                SetNewPath(targetPosition);
            }
        }
    }

    IEnumerator AutoMoveCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(updatePathInterval);
        while (true)
        {
            yield return wait;
            if (autoMove)
            {
                UpdatePath();
            }
        }
    }
}