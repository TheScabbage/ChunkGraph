using System;
using System.Collections.Generic;

public class Harness
{
	static List<ChunkNode> nodes = new List<ChunkNode>();
	public static Random rng = new Random(1337);

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
						if(input.Length == 3)
						{
							ChunkNode.Side s;
							int c = int.Parse(input[1]);
							s = ParseSide(input[2]);
							
							Link(c, s);
						}else
						{
							Console.WriteLine("Invalid length. Should be in the format 'link [index] [side]");
						}
						break;
					case "display":
						Display();
						break;
					case "delete":
						Delete(int.Parse(input[1]), ParseSide(input[2]));
						break;
					case "subdivide":
						if(input.Length == 2)
						{
							int index;
							if(int.TryParse(input[1], out index))
							{
								Linker.Subdivide(nodes[index]);
							}else
							{
								Console.WriteLine("Invalid index.");
							}
						}
						break;
				}
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
					nn++;
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
	
	static void Link(int chunkIndex, ChunkNode.Side side)
	{
		Console.WriteLine("Linking nodes to side " + chunkIndex + " of chunk " + side);
		List<ChunkNode> newNeighbours = new List<ChunkNode>();
		Console.WriteLine("Enter node indices. Enter non integer string to finish.");
		bool parsing = true;
		while(parsing)
		{
			String input = Console.ReadLine();
			int index;
			if(int.TryParse(input, out index))
			{
				newNeighbours.Add(nodes[index]);
			}else
			{
				parsing = false;
			}
		}
		nodes[chunkIndex].SetNeighbours(newNeighbours.ToArray(), side);
		Console.WriteLine("Added " + newNeighbours.Count + " neighbours to side " + side + " of node " + chunkIndex);
	}

}
