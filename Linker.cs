using System;
using System.Collections.Generic;

public class Linker
{
	
	public static void Subdivide(ChunkNode a)
	{
		// Create 8 new chunks to replace the current
		// In production the child chunks would be instantiated with 
		// some calculated coordinates using the last chunks bounds
		ChunkNode[] newNodes = new ChunkNode[8];
		for(int ii = 0; ii < 8; ii++)
		{
			newNodes[ii] = new ChunkNode(new TrixelChunk(Harness.rng.Next()));
		}

		
	}

	
}
