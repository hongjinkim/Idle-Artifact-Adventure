using System.Collections.Generic;
using UnityEngine;

public class CustomTilemapManager : TilemapManager
{
    private TilemapManager baseTilemapManager;
    private CharacterMovement character;
    private const float PositionTolerance = 0.1f;
    private const float PositionToleranceSquared = PositionTolerance * PositionTolerance;
    private List<CharacterMovement> allCharacters;

    public CustomTilemapManager(TilemapManager baseTilemapManager, CharacterMovement character)
    {
        this.baseTilemapManager = baseTilemapManager;
        this.character = character;
        CacheCharacters();
    }

    private void CacheCharacters()
    {
        allCharacters = new List<CharacterMovement>(Object.FindObjectsOfType<CharacterMovement>());
    }

    public override bool IsObstacle(Vector3 position)
    {
        for (int i = 0; i < allCharacters.Count; i++)
        {
            if (allCharacters[i] != character && (allCharacters[i].transform.position - position).sqrMagnitude < PositionToleranceSquared)
            {
                return true;
            }
        }
        return false;
    }

    public override List<Vector3> GetNeighbors(Vector3 position)
    {
        var baseNeighbors = baseTilemapManager.GetNeighbors(position);
        var validNeighbors = new List<Vector3>(baseNeighbors.Count);
        for (int i = 0; i < baseNeighbors.Count; i++)
        {
            if (!IsObstacle(baseNeighbors[i]))
            {
                validNeighbors.Add(baseNeighbors[i]);
            }
        }
        return validNeighbors;
    }

    public override bool IsValidMovePosition(Vector3 position)
    {
        Vector3 nearestValidPosition = GetNearestValidPosition(position);
        return (position - nearestValidPosition).sqrMagnitude < PositionToleranceSquared;
    }

    public override Vector3 GetNearestValidPosition(Vector3 position)
    {
        Vector3 nearest = baseTilemapManager.GetNearestValidPosition(position);
        if (!IsObstacle(nearest))
        {
            return nearest;
        }
        var neighbors = GetNeighbors(nearest);
        Vector3 nearestNeighbor = nearest;
        float minSqrDistance = float.MaxValue;
        for (int i = 0; i < neighbors.Count; i++)
        {
            float sqrDistance = (neighbors[i] - position).sqrMagnitude;
            if (sqrDistance < minSqrDistance)
            {
                minSqrDistance = sqrDistance;
                nearestNeighbor = neighbors[i];
            }
        }
        return nearestNeighbor;
    }

    public override float GetDistance(Vector3 a, Vector3 b)
    {
        return (a - b).magnitude;
    }

    public override List<Vector3> FindPath(Vector3 start, Vector3 goal)
    {
        var openSet = new HashSet<Vector3> { start };
        var closedSet = new HashSet<Vector3>();
        var cameFrom = new Dictionary<Vector3, Vector3>();
        var gScore = new Dictionary<Vector3, float> { { start, 0 } };
        var fScore = new Dictionary<Vector3, float> { { start, GetHeuristicCost(start, goal) } };

        while (openSet.Count > 0)
        {
            var current = GetLowestFScoreNode(openSet, fScore);
            if ((current - goal).sqrMagnitude < 0.01f)
            {
                return ReconstructPath(cameFrom, current);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (var neighbor in GetNeighbors(current))
            {
                if (closedSet.Contains(neighbor)) continue;

                var tentativeGScore = gScore[current] + GetDistance(current, neighbor);

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
                fScore[neighbor] = gScore[neighbor] + GetHeuristicCost(neighbor, goal);
            }
        }
        return null;
    }

    private float GetHeuristicCost(Vector3 start, Vector3 goal)
    {
        return (goal - start).magnitude;
    }

    private Vector3 GetLowestFScoreNode(HashSet<Vector3> openSet, Dictionary<Vector3, float> fScore)
    {
        Vector3 lowestNode = Vector3.zero;
        float lowestFScore = float.MaxValue;
        foreach (var node in openSet)
        {
            if (fScore.TryGetValue(node, out float score) && score < lowestFScore)
            {
                lowestFScore = score;
                lowestNode = node;
            }
        }
        return lowestNode;
    }

    private List<Vector3> ReconstructPath(Dictionary<Vector3, Vector3> cameFrom, Vector3 current)
    {
        var path = new List<Vector3> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }
        path.Reverse();
        return path;
    }
}