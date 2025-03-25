using UnityEngine;

namespace MinD.SO.Object {

[CreateAssetMenu(menuName = "MinD/Object/Guffins Anchor Info")]
public class GuffinsAnchorInformation : ScriptableObject {

	public string anchorName;

	[Space(10)]
	public bool canReadStory;
	[TextArea(5, 50)] public string anchorStory;
}

}