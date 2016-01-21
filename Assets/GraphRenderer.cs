using System.Collections.Generic;
using UnityEngine;

public class GraphRenderer : MonoBehaviour
{
    private const double SPACE_BETWEEN_BARS = 1.0;
    private const double BAR_WIDTH = 2.0;
    private const double CHART_HEIGHT = 100.0;
    private const double CHART_WIDTH = 100.0;
    private const double LITTLE_ABOVE = 3.0;              // A constant to show the hit text a little above the bar

    private double xStartingPoint;                  // Used to calculate the X Starting point
    private List<double> xInGraph;                  // Used to calculate x points to draw the bars
    private List<double> zInGraph;                  // Used to calculate z points to draw the bars
    private double yInGraph;                        // Used to store Y Starting point

    private void Start()
    {
        xInGraph = new List<double>();
        zInGraph = new List<double>();
        double[] numbers = new double[25];
        double[] numbers2 = new double[5];
        double[] numbers3 = new double[5];
        for (int i = 0; i < 25; i++)
        {
            numbers[i] = Random.Range(1, 15);
            if (i % 5 == 0)
            {
                numbers3[i / 5] = Random.Range(1, 15);
                numbers2[i / 5] = Random.Range(1, 15);
            }
        }
        Draw3DChart(numbers2, numbers, numbers3, BAR_WIDTH);
    }

    private void Draw3DChart(double[] XItems, double[] YItems, double[] ZItems, double BarWidth)
    {
        // If there are anything to draw then enter in this if block
        if (YItems.Length > 0 && XItems.Length > 0 && ZItems.Length > 0)
        {
            // Calculate the max Y point
            double MaxY = YItems[0];

            for (int Counter = 1; Counter < XItems.Length * ZItems.Length; Counter++)
            {
                if (MaxY < YItems[Counter])
                {
                    MaxY = YItems[Counter];
                }
            }

            // Calculate the Height of the longest bar and also calculate On Y Unit
            double Ht = CHART_HEIGHT / 30.0;

            double OneYUnit = ((CHART_HEIGHT / 10.0)) / MaxY;

            ComputeXZInGraph(XItems.Length, YItems.Length, BarWidth);

            OneYUnit = ((CHART_HEIGHT / 10.0) - (CHART_HEIGHT / (10 * Ht))) / MaxY;
            int CounterY = 0;

            // Draw the Bars one by one based on values computed in the method ComputeXZInGraph above
            for (int CounterZ = 0; CounterZ < ZItems.Length; CounterZ++)
            {
                for (int CounterX = 0; CounterX < XItems.Length; CounterX++)
                {
                    Draw3DBar(XItems[CounterX], YItems[CounterY], ZItems[CounterZ], new Vector3((float)(xInGraph[CounterX] - BarWidth / 2.0), (float)yInGraph, (float)(zInGraph[CounterZ] + BarWidth / 2.0)),
                                                               OneYUnit * YItems[CounterY], BarWidth);

                    CounterY++;
                }
            }
        }
    }

    private void Draw3DBar(double XItem, double YItem, double ZItem, Vector3 PointToStart, double Height,
                                              double Width)
    {
        Vector3 PtToWrite = new Vector3(PointToStart.x, (float)(PointToStart.y + LITTLE_ABOVE), (float)(PointToStart.z + Width / 2.0 - 0.3));

        GameObject myModelVisual = GameObject.CreatePrimitive(PrimitiveType.Cube);

        myModelVisual.transform.localScale = new Vector3(1, (float)Height, 1);
        Bounds _bounds = myModelVisual.GetComponent<MeshFilter>().sharedMesh.bounds;
        Vector3 lowerCenter = _bounds.center + new Vector3(0, -_bounds.extents.y * (float)Height, 0);
        PtToWrite = PtToWrite - lowerCenter;
        myModelVisual.transform.position = PtToWrite;


    }

    private void ComputeXZInGraph(int NoOfXItems, int NoOfZItems, double BarWidth)
    {
        double Width = CHART_WIDTH / 10.0;
        double Height = CHART_HEIGHT / 10.0;
        NoOfXItems++;
        NoOfZItems++;

        // Compute the starting X position and One X unit with the available Width/height to the control
        double StartX = -1.0 * Width / 2.0 + 0.5;
        double StartY = -1.0 * Height / 2.0 + 0.5;
        double OneUnitX = (Width - 0.5) / NoOfXItems;
        double StartXPosition = StartX + OneUnitX;

        xStartingPoint = StartX;
        // Fill in an array of values where the bars will be placed
        yInGraph = StartY;
        for (int Counter = 0; Counter < NoOfXItems - 1; Counter++)
        {
            xInGraph.Add(StartXPosition);
            StartXPosition += OneUnitX;
        }

        double OneUnitZ = BarWidth * SPACE_BETWEEN_BARS;

        // Fill in an array of values of Z positions to draw the bar
        double StartZPosition = 0 - OneUnitZ;
        for (int Counter = 0; Counter < NoOfZItems - 1; Counter++)
        {
            zInGraph.Add(StartZPosition);
            StartZPosition -= OneUnitZ;
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}