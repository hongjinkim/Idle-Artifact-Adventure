using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterMovement : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public float moveSpeed = 5f;
    private List<Vector3> path;
    private int currentPathIndex;
    public bool isSelected = false;
    public Detection detection;
    public bool autoMove = false;
    public float updatePathInterval = 1f;
    private float lastPathUpdateTime = 0f;

    private CustomTilemapManager customTilemapManager;
    private float PositionTolerance;
    private float PositionToleranceSquared;

    private Vector3 targetPosition;
    private Vector3 currentPosition;
    private Vector3 nearestValidPosition;

    private List<Vector3> previewPath;
    private LineRenderer lineRenderer;

    private Vector3 lastTargetPosition;
    private const float MAX_TARGET_DISTANCE = 0.7f;
    private const float MIN_TARGET_DISTANCE = 0.5f;

    private void Awake()
    {
        detection = GetComponent<Detection>();

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.yellow;

        PositionTolerance = 0.1f;
        PositionToleranceSquared = PositionTolerance * PositionTolerance;
    }

    void Start()
    {
        customTilemapManager = new CustomTilemapManager(TilemapManager.Instance, this);
        transform.position = customTilemapManager.GetNearestValidPosition(transform.position);
        StartCoroutine(AutoMoveCoroutine());
        lastTargetPosition = Vector3.positiveInfinity; // 초기화
    }

    void Update()
    {
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
                currentPosition = transform.position;

                // 타겟이 이동했거나 현재 위치가 적절하지 않은 경우에만 새 경로 계산
                if ((targetPosition - lastTargetPosition).sqrMagnitude > 0.01f ||
                    !IsPositionSuitableForTarget(currentPosition, targetPosition))
                {
                    Vector3? bestPosition = FindBestPositionAroundTarget(targetPosition);

                    if (bestPosition.HasValue)
                    {
                        SetNewPath(bestPosition.Value);
                    }
                    else
                    {
                        path = null;
                    }

                    lastTargetPosition = targetPosition;
                }
            }
        }
        lastPathUpdateTime = Time.time;
    }
    private bool IsPositionSuitableForTarget(Vector3 position, Vector3 target)
    {
        float distance = Vector2.Distance(new Vector2(position.x, position.y), new Vector2(target.x, target.y));
        return distance <= MAX_TARGET_DISTANCE && distance >= MIN_TARGET_DISTANCE && !IsPositionOccupied(position);
    }

    private Vector3? FindBestPositionAroundTarget(Vector3 targetPosition)
    {
        List<Vector3> potentialPositions = GetSurroundingPositions(targetPosition);
        potentialPositions.Sort((a, b) =>
            Vector2.Distance(new Vector2(a.x, a.y), new Vector2(transform.position.x, transform.position.y))
            .CompareTo(Vector2.Distance(new Vector2(b.x, b.y), new Vector2(transform.position.x, transform.position.y))));

        foreach (Vector3 position in potentialPositions)
        {
            List<Vector3> pathToPosition = customTilemapManager.FindPath(transform.position, position);
            if (pathToPosition != null && pathToPosition.Count > 0)
            {
                return position;
            }
        }
        // 적합한 위치를 찾지 못한 경우, 가장 가까운 도달 가능한 위치 반환
        foreach (Vector3 position in potentialPositions)
        {
            List<Vector3> pathToPosition = customTilemapManager.FindPath(transform.position, position);
            if (pathToPosition != null && pathToPosition.Count > 0)
            {
                return position;
            }
        }

        return null;
    }
    private bool IsPositionOccupied(Vector3 position)
    {
        return Physics2D.OverlapCircle(position, 0.1f) != null;
    }
    private bool IsPositionOccupiedByOther(Vector3 position)
    {
        Collider2D collider = Physics2D.OverlapCircle(position, 0.1f);
        return collider != null && collider.gameObject != this.gameObject;
    }

    private List<Vector3> GetSurroundingPositions(Vector3 targetPosition)
    {
        List<Vector3> surroundingPositions = new List<Vector3>();
        Vector3 cellSize = TilemapManager.Instance.tilemap.cellSize;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                Vector3 offset = new Vector3(x * cellSize.x, y * cellSize.y, 0);
                Vector3 newPos = targetPosition + offset;
                if (customTilemapManager.IsValidMovePosition(newPos))
                {
                    surroundingPositions.Add(newPos);
                }
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
            if (!customTilemapManager.IsValidMovePosition(targetPosition) || IsPositionOccupiedByOther(targetPosition))
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