using System;
using System.Collections.Generic;

public class Harness
{
	static List<ChunkNode> nodes = new List<ChunkNode>();
	static Random rng = new Random(1337);

	public static void Main(String[] args)
	{
		bool enabled = true;

		while(enabled)
		{
			String[] input = Console.ReadLine().Split(' ');
			if(input.Length > 0)
			{
				switch(input[0])
				{
					case "exit":
						enabled = false;
						Console.WriteLine(":)");
						break;
					case "add":
						AddNode();
						break;
					case "link":
						if(input.Length == 5)
						{
							int c1, c2;
							ChunkNode.Side s1, s2;
							c1 = int.Parse(input[1]);
							c2 = int.Parse(input[2]);
							s1 = ParseSide(input[3]);
							s2 = ParseSide(input[4]);
							
							Link(c1, c2, s1, s2);
						}else
						{
							Console.WriteLine("Invalid length. Should be in the format 'link [n1] [n2] [s1] [s2]");
						}
						break;
					case "display":
						Display();
						break;
					case "delete":
						Delete(int.Parse(input[1]), ParseSide(input[2]));
						break;
					case "loop":
						Loop(int.Parse(input[1]));
						break;
				}
			}
		}
	}

	static void Loop(int origin)
	{
		foreach(List<ChunkNode> loop in Linker.GetLoops(nodes[origin]))
		{
			if(loop != null)
			{
				Console.WriteLine("Found loop of length " + loop.Count);
			}
		}
	}

	static void Delete(int node, ChunkNode.Side side)
	{
		nodes[node].DeleteNeighbours(side);
		Console.WriteLine("Deleted neighbours on side " + side + " of chunk " + node + " (" + nodes[node].GetHashCode() + ")");
		
	}
	
	static void Display()
	{
		for(int ii = 0; ii < nodes.Count; ii++)
		{
			Console.WriteLine("Node " + ii + ": " + nodes[ii].GetHashCode());
			for(int ss = 0; ss < 5; ss++)
			{
				int nn = 0;
				bool first = true;
				foreach(ChunkNode neighbour in nodes[ii].GetNeighbours((ChunkNode.Side)ss))
				{
					if(first)
					{
						first = false;
						Console.WriteLine("  Side " + (ChunkNode.Side)(ss) + ": ");
					}
					Console.WriteLine("    Neighbour " + nn + ": " + neighbour.GetHashCode());
				}
			}
		}
	}

	static ChunkNode.Side ParseSide(String side)
	{
		switch(side.ToLower())
		{
			case "x":
				return ChunkNode.Side.X;
			case "z":
				return ChunkNode.Side.Z;
			case "w":
				return ChunkNode.Side.W;
			case "u":
				return ChunkNode.Side.U;
			case "d":
				return ChunkNode.Side.D;
		}
		throw new ArgumentException("Invalid side string.");
	}

	static void AddNode()
	{
		TrixelChunk newChunk = new TrixelChunk(rng.Next());
		nodes.Add(new ChunkNode(newChunk));
		Console.WriteLine("Added node " + (nodes.Count - 1) + " with id " + nodes[nodes.Count - 1].GetHashCode());
	}
	
	static void Link(int c1, int c2, ChunkNode.Side side1, ChunkNode.Side side2)
	{
		nodes[c1].AddNeighbour(nodes[c2], side1);
		nodes[c2].AddNeighbour(nodes[c1], side2);
		Console.WriteLine("Linked node " + c1 + " to node " + c2 + ", side1: " + side1 + ", side2: " + side2);
	}

}
