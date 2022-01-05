using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph
{
    class Vertex
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public Vertex(int number, string name)
        {
            Number = number;
            Name = name;
        }
    }
    class Edge
    {
        public Vertex From { get; set; }
        public Vertex To { get; set; }
        public int Weight { get; set; }

        public Edge(Vertex from, Vertex to, int weight = 1)
        {
            From = from;
            To = to;
            Weight = weight;

        }
    }
    class Graph
    {
        List<Vertex> Vertexes = new List<Vertex>();
        List<Edge> Edges = new List<Edge>();
        int[,] matrix;
        int[] color;
        public int VertexCount => Vertexes.Count;
        public int EdgeCount => Edges.Count;
        public void AddVertex(Vertex vertex)
        {
            Vertexes.Add(vertex);
        }
        public void AddEdge(Vertex from, Vertex to)
        {
            var edge = new Edge(from, to);
            Edges.Add(edge);
        }
        public int[,] GetMatrix()
        {
            matrix = new int[VertexCount, VertexCount];
            foreach (var edge in Edges)
            {
                var row = edge.From.Number - 1;
                var column = edge.To.Number - 1;

                matrix[row, column] = edge.Weight;
                matrix[column, row] = matrix[row, column];
            }
            return matrix;
        }
        public int indexOfName(string NameSearch)
        {
            int c = 0;
            foreach (var Vertex in Vertexes)
            {
                if (Vertex.Name == NameSearch)
                    return c;
                c++;
            }

            return -1;
        }
        public int First(string v)
        {
            int[,] matrix = GetMatrix();
            for (int i = 0; i < VertexCount; i++)
                if (matrix[indexOfName(v), i] == 1) return i + 1;
            return -1;
        }
        public int Next(string v, int iFrom)
        {
            int[,] matrix = GetMatrix();
            for (int i = iFrom; i < VertexCount; i++)
                if (matrix[indexOfName(v), i] == 1) return i;
            return -1;
        }
        public int Vertex(int ii)
        {
            var matrix = GetMatrix();
            for (int i = ii; i < VertexCount; i++)
                for (int j = 0; j < VertexCount; j++)
                    if (matrix[i, j] != 0)
                        return j + 1;
            return -1;
        }
        public void DelVertex(Vertex name)
        {
            Vertexes.Remove(name);
        }
        public void DelEdge(Vertex from, Vertex to)
        {
            foreach (var edge in Edges)
                if (edge.From == from && edge.To == to)
                    Edges.Remove(edge);
        }
        public void EditVertex(string name, string newName)
        {
            foreach (var v in Vertexes)
                if (v.Name == name)
                    v.Name = newName;
        }
        public void EditEdge(Vertex from, Vertex to, int newWeight)
        {
            foreach (var edge in Edges)
                if (edge.From == from && edge.To == to)
                    edge.Weight = newWeight;
        }
        public bool isSafe(int v, int[,] matrix, int[] color, int c)
        {
            for (int i = 0; i < VertexCount; i++)
                if (matrix[v, i] == 1 && c == color[i])
                    return false;
            return true;
        }

        public bool graphColoringUtil(int[,] matrix, int m, int[] color, int v)
        {
            if (v == VertexCount)
                return true;

            for (int c = 1; c <= m; c++)
            {
                if (isSafe(v, matrix, color, c))
                {
                    color[v] = c;

                    if (graphColoringUtil(matrix, m, color, v + 1))
                        return true;

                    color[v] = 0;
                }
            }

            return false;
        }

        public bool graphColoring(int[,] matrix, int m)
        {
            color = new int[VertexCount];
            for (int i = 0; i < VertexCount; i++)
                color[i] = 0;

            if (!graphColoringUtil(matrix, m, color, 0))
            {
                Console.WriteLine("Решение не существует");
                return false;
            }

            printSolution(color);
            return true;
        }

        public void printSolution(int[] color)
        {
            int c = 0;
            foreach (var i in Vertexes)
            {
                Console.WriteLine($"Вершина {i.Name} окрашена в {color[c]} цвет");
                c++;
            }

            Console.WriteLine();
            Console.WriteLine($"Минимальное число красок, которыми можно раскрасить граф: {color.Max()}");
        }
        public void Print()
        {
            var matrix = new int[Vertexes.Count, Vertexes.Count];

            foreach (var edge in Edges)
            {
                var row = edge.From.Number - 1;
                var column = edge.To.Number - 1;

                matrix[row, column] = edge.Weight;
                matrix[column, row] = matrix[row, column];
            }
            Console.WriteLine("Матрица смежности");
            Console.WriteLine();
            Console.Write("   ");
            for (int i = 0; i < VertexCount; i++)
            {
                Console.Write(i + 1 + " ");
            }
            Console.WriteLine();
            for (int i = 0; i < VertexCount; i++)
            {
                Console.Write(" " + (i + 1) + " ");
                for (int j = 0; j < VertexCount; j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            {
                Console.WriteLine("Граф 1");
                Console.WriteLine();
                var graph = new Graph();

                var a = new Vertex(1, "a");
                var b = new Vertex(2, "b");
                var c = new Vertex(3, "c");
                var d = new Vertex(4, "d");
                var e = new Vertex(5, "e");

                graph.AddVertex(a);
                graph.AddVertex(b);
                graph.AddVertex(c);
                graph.AddVertex(d);
                graph.AddVertex(e);

                graph.AddEdge(a, b);
                graph.AddEdge(a, d);
                graph.AddEdge(b, c);
                graph.AddEdge(c, a);
                graph.AddEdge(c, e);
                graph.AddEdge(d, a);
                graph.AddEdge(d, b);
                graph.AddEdge(d, c);
                graph.AddEdge(d, e);
                graph.AddEdge(e, a);

                graph.Print();
                int m = graph.VertexCount; 
                graph.graphColoring(graph.GetMatrix(), m);
                Console.WriteLine();
                Console.WriteLine();

            } 
            {
                Console.WriteLine("Граф 2");
                Console.WriteLine();
                var graph = new Graph();

                var a = new Vertex(1, "a");
                var b = new Vertex(2, "b");
                var c = new Vertex(3, "c");
                var d = new Vertex(4, "d");
                var e = new Vertex(5, "e");


                graph.AddVertex(a);
                graph.AddVertex(b);
                graph.AddVertex(c);
                graph.AddVertex(d);
                graph.AddVertex(e);

                graph.AddEdge(a, b);
                graph.AddEdge(a, c);
                graph.AddEdge(a, d);
                graph.AddEdge(b, c);
                graph.AddEdge(b, e);
                graph.AddEdge(c, d);
                graph.AddEdge(d, e);


                graph.Print();
                int m = graph.VertexCount; 
                graph.graphColoring(graph.GetMatrix(), m);
            }



            Console.ReadLine();
        }
    }
}
