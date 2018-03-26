

harness : Harness.cs ChunkNode.cs TrixelChunk.cs
	mcs *.cs -main:Harness -debug -out:harness

