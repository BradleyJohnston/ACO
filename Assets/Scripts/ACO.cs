using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ACO : MonoBehaviour
{
    // Distance variables
    float[,] distanceMatrix;
    public float distancePower;

    // Phermone variables
    float[,] pheromoneValues;
    public float pheromonePower;
    public float initialPheromone;
    public float pheromoneDecay;

    // General variables
    public int numberOfAnts;
    public int numberOfTests;
    public List<int> shortestAnt = new List<int>();
    public float shortestPathDistance;

    public SpawnPoints pointHolder;
    private List<Vector2> points;
    private int nodeCount;

    public float getAverageDistance()
    {
        return shortestPathDistance / shortestAnt.Count;
    }
    public void Solve()
    {
        // Initialize variables
        points = new List<Vector2>(pointHolder.points);
        shortestAnt = new List<int>();
        shortestPathDistance = Mathf.Infinity;
        nodeCount = points.Count;
        List<List<int>> antList = new List<List<int>>();
        List<int> bufferAnt = new List<int>();
        List<float> antGrades = new List<float>();
        bool[] visitArray = new bool[nodeCount];

        distanceMatrix = new float[nodeCount, nodeCount];
        pheromoneValues = new float[nodeCount, nodeCount];
        

        // Distance Matrix calculation & pheromone value setting
        for (int i = 0; i < nodeCount; i++)
        {
            visitArray[i] = false;
            shortestAnt.Add(i);
            for (int j = 0; j < nodeCount; j++)
            {
                distanceMatrix[i, j] = Vector2.Distance(points[i], points[j]);
                pheromoneValues[i, j] = initialPheromone;
            }
        }

        // Run each test
        for (int i = 0; i < numberOfTests; i++)
        {
            // Generate Random Solution for each ant
            for (int j = 0; j < numberOfAnts; j++)
            {
                // Reset variables for ant
                for (int k = 0; k < nodeCount; k++)
                {
                    visitArray[k] = false;
                }
                bufferAnt = new List<int>();

                // Choose first point
                int index = (int)Random.Range(0, nodeCount);

                bufferAnt.Add(index);
                visitArray[index] = true;

                // Choose subsequent points
                while (bufferAnt.Count < nodeCount)
                {
                    // Determine summed weights for each Possible Edge
                    float edgeWeightSum = 0;
                    for (int k = 0; k < nodeCount; k++)
                    {
                        // If we already have visited this index then skip it
                        if (visitArray[k] == true)
                        {
                            continue;
                        }

                        float desirability = Mathf.Pow(1 / distanceMatrix[bufferAnt[bufferAnt.Count - 1], k], distancePower);
                        desirability *= Mathf.Pow(pheromoneValues[bufferAnt[bufferAnt.Count - 1], k], pheromonePower);

                        edgeWeightSum += desirability;
                    }

                    // Calculate random value and compare against desirability of each edge
                    float randomValue = Random.Range(0, edgeWeightSum);
                    for (int k = 0; k < nodeCount; k++)
                    {
                        if (visitArray[k] == true)
                        {
                            continue;
                        }
                        float desirability = Mathf.Pow(1 / distanceMatrix[bufferAnt[bufferAnt.Count - 1], k], distancePower);
                        desirability *= Mathf.Pow(pheromoneValues[bufferAnt[bufferAnt.Count - 1], k], pheromonePower);

                        randomValue -= desirability;
                        if (randomValue <= 0)
                        {
                            visitArray[k] = true;
                            bufferAnt.Add(k);
                            break;
                        }
                    }
                }

                antList.Add(bufferAnt);
            }

            // Grade each ant, and modify the pheromone values from that
            for (int x = 0; x < nodeCount; x++)
            {
                for (int y = 0; y < nodeCount; y++)
                {
                    pheromoneValues[x,y] *= pheromoneDecay;
                }
            }

            antGrades = new List<float>();
            float grade = 0;
            for (int j = 0; j < numberOfAnts; j++)
            {
                grade = 0;

                for (int k = 0; k < nodeCount; k++)
                {
                    grade += distanceMatrix[antList[j][k], antList[j][(k + 1) % nodeCount]];
                }

                if (grade < shortestPathDistance)
                {
                    shortestAnt = new List<int>(antList[j]);
                    shortestPathDistance = grade;
                }

                antGrades.Add(grade);
            }

            for (int j = 0; j < numberOfAnts; j++)
            {
                for (int k = 0; k < nodeCount; k++)
                {
                    pheromoneValues[k, (k + 1) % nodeCount] += (shortestPathDistance) / (antGrades[j] * numberOfAnts);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (shortestAnt.Count > 0)
        {
            for (int i = 0; i < shortestAnt.Count; i++)
            {
                Gizmos.DrawLine(points[shortestAnt[i]], points[shortestAnt[(i + 1) % shortestAnt.Count]]);
            }
        }
    }
}
