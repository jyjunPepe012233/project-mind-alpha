namespace MinD.Interfaces {

public interface IWorldIndexable {
	
	// MUST BE SERIALIZED TO CHANGE VALUE IN EDITOR
	public bool hasBeenIndexed { get; set; }
	public int worldIndex { get; set; }
}

}