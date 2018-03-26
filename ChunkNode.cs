using System.Collections.Generic;


// Manages the topology of a chunk graph.
// Has a 1:1 relationship with chunks, but encapsulates all graph operations away from
// the trixel chunks
// Note that any operations on neighbour relationships are local, and must be reflected by
// an equal update in any neighbour nodes to maintain an uncorrupted graph.
public class ChunkNode
{
	TrixelChunk chunk;
	// connections[i][j] denotes the jth neither of side i
	// The side enum is built to work with this array as an index
	Connection[][] connections;
	// All of the chunks that this chunk depends on to merge
	ChunkNode[] mergeDependencies;
	
	public bool Mergable
	{
		get
		{
			// For this chunk to be mergable, all merge dependencies must NOT be mergable.
			// Equivalently, this chunk cannot be merged if any depency is mergable.
			if(mergeDependencies == null)
			{
				return true;
			}
			foreach(ChunkNode node in mergeDependencies)
			{
				if(node.Mergable)
				{
					return false;
				}
			}
			return true;
		}	
	}

	public enum Side {X, Z, W, U, D}

	private class Connection
	{
		public ChunkNode neighbour;

		public Connection(ChunkNode neighbour)
		{
			this.neighbour = neighbour;
		}
	}

	public ChunkNode(TrixelChunk chunk)
	{
		this.chunk = chunk;
		connections = new Connection[5][];
	}
	
	// Gets all neighbours connected to the given side
	public IEnumerable<ChunkNode> GetNeighbours(Side side)
	{
		if(connections[(int)side] != null)
		{
			foreach(Connection c in connections[(int)side])
			{
				yield return c.neighbour;
			}
		}
	}
	
	// Gets all neighbours of the node
	public IEnumerable<ChunkNode> GetNeighbours()
	{
		for(int ii = 0; ii < 5; ii++)
		{
			if(connections[ii] != null)
			{
				foreach(Connection c in connections[ii])
				{
					yield return c.neighbour;
				}
			}
		}
	}

	// Sets the neighbours of the given side
	public void SetNeighbours(ChunkNode[] nodes, Side side)
	{
		int sideIndex = (int)side;
		Connection[] newConnections = new Connection[nodes.Length];
		for(int ii = 0; ii < nodes.Length; ii++)
		{
			newConnections[ii] = new Connection(nodes[ii]);
		} 
		connections[sideIndex] = newConnections;
	}

	// Removes all neighbours from the given side
	public void DeleteNeighbours(Side side)
	{
		connections[(int)side] = null;
	}

	public override int GetHashCode()
	{
		return chunk.GetHashCode();
	}
}
