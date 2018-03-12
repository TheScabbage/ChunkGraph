

public class TrixelChunk
{
	int hashCode;
	public TrixelChunk(int hashCode)
	{
		this.hashCode = hashCode;
	}

	public override int GetHashCode()
	{
		return hashCode;
	}
}	
