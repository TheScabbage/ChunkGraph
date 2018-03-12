using System;
using System.Collections.Generic;

public class Linker
{
	
	public static IEnumerable<List<ChunkNode>> GetLoops(ChunkNode origin)
	{
		Dictionary<ChunkNode, ChunkNode> tree = new Dictionary<ChunkNode, ChunkNode>();
		List<ChunkNode> discovered = new List<ChunkNode>();
		discovered.Add(origin);
		
		while(discovered.Count > 0)
		{
			// discover neighbours of the current node
			foreach(ChunkNode neighbour in discovered[0].GetNeighbours())
			{
				// add neighbour to the discovered set if we havent found it already
				if(!tree.ContainsKey(neighbour))
				{
					discovered.Add(neighbour);
					tree.Add(neighbour, discovered[0]);
				}else
				{
					// We found an already discovered node, find the loop from the tree
					List<ChunkNode> loop = GetLoop(tree, discovered[0], neighbour);
					yield return loop;
				}
			}
			discovered.RemoveAt(0);
		}
		yield return null;
	}

	private static List<ChunkNode> GetLoop(Dictionary<ChunkNode, ChunkNode> tree, ChunkNode start, ChunkNode end)
	{
		List<ChunkNode> loop = new List<ChunkNode>();
		bool commonFound = false;
		ChunkNode parA, parB;
		bool flip = true;
		int insertPos = 0;
		while(!commonFound)
		{
			// Get the child nodes
			tree.TryGetValue(start, out parA);
			tree.TryGetValue(end, out parB);
			if(parA == parB)
			{
				commonFound = true;
				// add the final 3 nodes to the loop
				if(flip)
				{
					loop.Insert(insertPos, start);
					loop.Add(end);
					insertPos++;
				}else
				{
					loop.Add(start);
					loop.Add(end);
				}
				loop.Insert(insertPos, parA);
				insertPos++;
			}else
			{
				// Add start to the loop, go to child
				if(flip)
					loop.Insert(insertPos, start);
				else
					loop.Add(start);
				insertPos++;
				flip = !flip;
				start = end;
				end = parA;
			}
		}
		return loop;
	}
}
