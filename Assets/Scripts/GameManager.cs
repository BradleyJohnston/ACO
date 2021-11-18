using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SpawnPoints pointsSpawner;
    public int numberOfPoints;
    public ACO aco;

    public string fileName;


    public float startDistancePower;
    public float startPheromonePower;
    public float startPheromoneDecay;
    public float startInitialPheromone;
    public int startNumberOfAnts;
    public int startNumberOfTests;
    public int startNumberOfPoints;
    public float endDistancePower;
    public float endPheromonePower;
    public float endPheromoneDecay;
    public float endInitialPheromone;
    public int endNumberOfAnts;
    public int endNumberOfTests;
    public int endNumberOfPoints;
    public float stepDistancePower;
    public float stepPheromonePower;
    public float stepPheromoneDecay;
    public float stepInitialPheromone;
    public int numberOfTestsPerVariableCase;

    public struct dataHolder
    {

        public float distancePower { get; set; }
        public float pheromonePower { get; set; }
        public float pheromoneDecay { get; set; }
        public float initialPheromone { get; set; }
        public int numberOfAnts { get; set; }
        public int numberOfTests { get; set; }
        public int numberOfPoints { get; set; }

        public float averageDistancePerPoint;

        public dataHolder(dataHolder rhs)
        {
            distancePower = rhs.distancePower;
            pheromonePower = rhs.pheromonePower;
            pheromoneDecay = rhs.pheromoneDecay;
            initialPheromone = rhs.initialPheromone;
            numberOfAnts = rhs.numberOfAnts;
            numberOfTests = rhs.numberOfTests;
            numberOfPoints = rhs.numberOfPoints;
            averageDistancePerPoint = rhs.averageDistancePerPoint;

        }
    }

    List<dataHolder> dataList = new List<dataHolder>();

    void displayDataHolder(dataHolder buffer)
    {
        Debug.Log("DISPLAYING NEW SOLVE STATS");
        Debug.Log(buffer.distancePower);
        Debug.Log(buffer.pheromonePower);
        Debug.Log(buffer.pheromoneDecay);
        Debug.Log(buffer.initialPheromone);
        Debug.Log(buffer.numberOfAnts);
        Debug.Log(buffer.numberOfTests);
        Debug.Log(buffer.numberOfPoints);
        Debug.Log(buffer.averageDistancePerPoint);
    }

    // Start is called before the first frame update
    void Start()
    {
        /*float startDistancePower;
        float startPheromonePower;
        float startPheromoneDecay;
        float startInitialPheromone;

        int startNumberOfAnts;
        int startNumberOfTests;

        int startNumberOfPoints;
*/
        dataHolder buffer = new dataHolder();
        for (int numberOfPoints = startNumberOfPoints;
            numberOfPoints < endNumberOfPoints;
            numberOfPoints++)
        {
            pointsSpawner.numberOfPoints = numberOfPoints;
            buffer.numberOfPoints = numberOfPoints;
            for (float distancePower = startDistancePower;
                distancePower < endDistancePower;
                distancePower += stepDistancePower)
            {
                aco.distancePower = distancePower;
                buffer.distancePower = distancePower;
                for (float pheromonePower = startPheromonePower;
                    pheromonePower < endPheromonePower;
                    pheromonePower += stepPheromonePower)
                {
                    aco.pheromonePower = pheromonePower;
                    buffer.pheromonePower = pheromonePower;
                    for (float pheromoneDecay = startPheromoneDecay;
                        pheromoneDecay < endPheromoneDecay;
                        pheromoneDecay += stepPheromoneDecay)
                    {
                        aco.pheromoneDecay = pheromoneDecay;
                        buffer.pheromoneDecay = pheromoneDecay;
                        for (float initialPheromone = startInitialPheromone;
                            initialPheromone < endInitialPheromone;
                            initialPheromone += stepInitialPheromone)
                        {
                            aco.initialPheromone = initialPheromone;
                            buffer.initialPheromone = initialPheromone;
                            for (int numberOfAnts = startNumberOfAnts;
                                numberOfAnts < endNumberOfAnts;
                                numberOfAnts++)
                            {
                                aco.numberOfAnts = numberOfAnts;
                                buffer.numberOfAnts = numberOfAnts;
                                for (int numberOfTests = startNumberOfTests;
                                    numberOfTests < endNumberOfTests;
                                    numberOfTests++)
                                {
                                    aco.numberOfTests = numberOfTests;
                                    buffer.numberOfTests = numberOfTests;
                                    for (int i = 0; 
                                        i < numberOfTestsPerVariableCase;
                                        i++)
                                    {
                                        pointsSpawner.Spawn();
                                        aco.Solve();
                                        buffer.averageDistancePerPoint = aco.getAverageDistance();
                                        dataList.Add(new dataHolder(buffer));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            displayDataHolder(dataList[i]);
        }
    }

}
