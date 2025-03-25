using System;
using UnityEngine;

namespace MinD.Structs {

[Serializable]
public struct SpiritAffinity {

	[Range(0, 100)] public int flame; // 불
	[Range(0, 100)] public int storm; // 폭풍
	[Range(0, 100)] public int live; // 생
	[Range(0, 100)] public int ice; // 얼음
	[Range(0, 100)] public int metal; // 금속
	[Range(0, 100)] public int flux; // 플럭스

}

}