using System.Collections.Generic;


public class ChunkNode
{
	int properties;
	TrixelChunk chunk;
	Connection[] sides;
	
	public bool Mergable
	{
		get
		{
			return SubdivisionIndex > 0;
		}
		
	}

	public int SubdivisionIndex
	{
		get
		{
			// Index is the lower byte of the properties array
			return properties & 0xFF;
		}
	}

	public int LOD
	{
		get
		{
			// LOD is the second byte of the properties array
			return (properties & 0xFF00) >> 8;
		}
		set
		{
			// Mask out lower byte
			int val = value & 0xFF;
			// Shift it left
			val = val << 8;
			// set the second byte of the properties array
			properties = properties & ~0xFF00;
			properties |= val;
		}
	}

	public enum Side {X, Z, W, U, D}

	private class Connection
	{
		public ChunkNode neighbour;
		public Connection next;

		public Connection(ChunkNode neighbour)
		{
			this.neighbour = neighbour;
		}
	}

	public ChunkNode(TrixelChunk chunk)
	{
		this.chunk = chunk;
		sides = new Connection[5];
	}
	
	public IEnumerable<ChunkNode> GetNeighbours(Side side)
	{
		Connection current = sides[(int)side];
		while(current != null)
		{
			yield return current.neighbour;
			current = current.next;
		}
	}
	

	public void AddNeighbour(ChunkNode node, Side side)
	{
		if(sides[(int)side] == null)
		{
			sides[(int)side] = new Connection(node);
		}else
		{
			Connection temp = sides[(int)side];
			sides[(int)side] = new Connection(node);
			sides[(int)side].next = temp;
		}
	}

	public void DeleteNeighbours(Side side)
	{
		sides[(int)side] = null;
	}

	public override int GetHashCode()
	{
		return chunk.GetHashCode();
	}
}
