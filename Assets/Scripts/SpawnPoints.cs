using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    // Visualization tools
    public GameObject node;

    // Variables
    public float minDistance;

    public float xRange;
    public float yRange;
    public int maxDepth;
    public int numberOfPoints;

    private int xOffset;
    private int yOffset;

    private int xLength;
    private int yLength;

    private Vector2[,] grid;

    public List<Vector2> points;

    public void Spawn()
    {
        foreach (Transform child in transform.GetComponentInChildren<Transform>())
        {
            Destroy(child.gameObject);
        }

        //  Setup Variables
        xOffset = (int)(Mathf.Ceil(xRange));
        yOffset = (int)(Mathf.Ceil(yRange));

        xLength = (2 * xOffset);
        yLength = (2 * yOffset);

        grid = new Vector2[xLength, yLength];
        points = new List<Vector2>();

        // Set each grid value to something impossible
        for (int i = 0; i < -2 * xOffset; i++)
        {
            for (int j = 0; j < -2 * yOffset; j++)
            {
                grid[i, j] = Vector2.negativeInfinity;
            }
        }

        int count = 0;

        Vector2 buffer;
        int bufferX;
        int bufferY;
        bool flag;

        while (count < maxDepth &&
            points.Count < numberOfPoints)
        {
            count++;
            // Generate Random Point
            buffer = new Vector2(Random.Range(-xRange, xRange), Random.Range(-yRange, yRange));

            // Compare random point against grid
            bufferX = Mathf.FloorToInt(buffer.x) + xOffset;
            bufferY = Mathf.FloorToInt(buffer.y) + yOffset;
            flag = false;

            for (int i = -1; i <= 1; i++)
            {
                // If we would cause x to go out of range skip that loop
                if (bufferX == 0 && i == -1 ||
                    bufferX == xLength - 1 && i == 1)
                {
                    continue;
                }
                for (int j = -1; j <= 1; j++)
                {
                    // If we would cause y to go out of range skip that loop
                    if (bufferY == 0 && j == -1 ||
                        bufferY == yLength - 1 && j == 1)
                    {
                        continue;
                    }

                    if (Vector2.Distance(buffer, grid[bufferX + i, bufferY + j]) < minDistance)
                    {
                        flag = true;
                    }

                }
            }

            if (flag != true)
            {
                grid[bufferX, bufferY] = buffer;
                points.Add(buffer);
            }
        }

        for (int i = 0; i < points.Count; i++)
        {
            Instantiate(node, points[i], Quaternion.identity, transform);
        }
    }
}
