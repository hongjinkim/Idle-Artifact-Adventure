using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TilemapManager : MonoBehaviour
{
    private static TilemapManager _Instance;
    public static TilemapManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<TilemapManager>();
            }
            return _Instance;
        }
    }
    public Tilemap tilemap;
    private List<Vector3> tileCenters = new List<Vector3>();
    public List<Vector3> currentPath;
    private Vector3 cellSize;

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (tilemap != null)
        {
            cellSize = tilemap.cellSize;
            CalculateTileCenters();
        }
    }

    void CalculateTileCenters()
    {
        BoundsInt bounds = tilemap.cellBounds;

        for (int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                if (tilemap.HasTile(tilePosition))
                {
                    Vector3 centerPosition = tilemap.GetCellCenterWorld(tilePosition);
                    centerPosition.z = 0;
                    tileCenters.Add(centerPosition);
                }
            }
        }
    }
    public virtual bool IsValidMovePosition(Vector3 position)
    {
        for (int i = 0; i < tileCenters.Count; i++)
        {
            if (Vector3.Distance(tileCenters[i], position) < cellSize.x * 0.1f)
            {
                return true;
            }
        }
        return false;
    }

    public virtual Vector3 GetNearestValidPosition(Vector3 position)
    {
        Vector3 nearest = Vector3.zero;
        float minDistance = float.MaxValue;

        for (int i = 0; i < tileCenters.Count; i++)
        {
            float distance = Vector3.Distance(tileCenters[i], position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = tileCenters[i];
            }
        }

        if (IsValidMovePosition(position) && Vector3.Distance(position, nearest) < cellSize.x * 0.1f)
        {
            return position;
        }
        return nearest;
    }

    public virtual bool IsObstacle(Vector3 position)
    {
        return false;
    }

    public virtual List<Vector3> GetNeighbors(Vector3 position)
    {
        List<Vector3> neighbors = new List<Vector3>();
        for (int i = 0; i < tileCenters.Count; i++)
        {
            if (Vector3.Distance(tileCenters[i], position) <= cellSize.x * 1.5f && !IsObstacle(tileCenters[i]))
            {
                neighbors.Add(tileCenters[i]);
            }
        }
        return neighbors;
    }

    public virtual float GetDistance(Vector3 a, Vector3 b)
    {
        float dx = Mathf.Abs(a.x - b.x) / cellSize.x;
        float dy = Mathf.Abs(a.y - b.y) / cellSize.y;

        return Mathf.Max(dx, dy) + (Mathf.Sqrt(2) - 1) * Mathf.Min(dx, dy);
    }

    public virtual List<Vector3> FindPath(Vector3 start, Vector3 goal)
    {
        List<Vector3> openSet = new List<Vector3> { start };
        List<Vector3> closedSet = new List<Vector3>();
        Dictionary<Vector3, Vector3> cameFrom = new Dictionary<Vector3, Vector3>();
        Dictionary<Vector3, float> gScore = new Dictionary<Vector3, float> { { start, 0 } };
        Dictionary<Vector3, float> fScore = new Dictionary<Vector3, float> { { start, GetDistance(start, goal) } };

        while (openSet.Count > 0)
        {
            Vector3 current = GetLowestFScore(openSet, fScore);

            if (current == goal || IsObstacle(goal))
            {
                currentPath = ReconstructPath(cameFrom, current);
                return currentPath;
            }

            openSet.Remove(current);
            closedSet.Add(current);

            List<Vector3> neighbors = GetNeighbors(current);
            for (int i = 0; i < neighbors.Count; i++)
            {
                Vector3 neighbor = neighbors[i];
                if (closedSet.Contains(neighbor)) continue;

                float tentativeGScore = gScore[current] + GetDistance(current, neighbor);

                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
                else if (tentativeGScore >= gScore.GetValueOrDefault(neighbor, float.MaxValue))
                {
                    continue;
                }

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + GetDistance(neighbor, goal);
            }
        }
        return null;
    }

    private Vector3 GetLowestFScore(List<Vector3> openSet, Dictionary<Vector3, float> fScore)
    {
        Vector3 lowest = openSet[0];
        float lowestScore = float.MaxValue;

        for (int i = 0; i < openSet.Count; i++)
        {
            if (fScore.TryGetValue(openSet[i], out float score) && score < lowestScore)
            {
                lowest = openSet[i];
                lowestScore = score;
            }
        }

        return lowest;
    }

    private Vector3 RoundToHalf(Vector3 v)
    {
        return new Vector3(
            Mathf.Round(v.x * 2) / 2,
            Mathf.Round(v.y * 2) / 2,
            v.z
        );
    }

    private List<Vector3> ReconstructPath(Dictionary<Vector3, Vector3> cameFrom, Vector3 current)
    {
        var path = new List<Vector3> { RoundToHalf(current) };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(RoundToHalf(current));
        }
        path.Reverse();
        return path;
    }

    public void SetDebugPath(List<Vector3> debugPath)
    {
        currentPath = debugPath;
    }
}