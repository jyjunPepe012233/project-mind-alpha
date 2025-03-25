namespace MinD.Enums {

public enum InstantEffectType {
	TakeHealthDamage,
	Ignite // 최대체력의 7%와 스턴
}

public enum StaticEffectType {
	IncreaseDamage
}

public enum TimedEffectType {

}

public enum StackingEffectType {
	Burn, // 축적치에 따라 (0.1~1% 사이의 데미지를 받음 )

	// 축적 경감 속도 빠름
	// Full 상태에서 최대 체력의 7%만큼의 데미지를 줌
	Poison, // 독뎀 (초당 0.5%, 60초)
	DeadlyPoison, // 맹독뎀 (0.8%, 50초)
	Frostbite, // 경감률 감소 및 데미지(7%) 1회

	Fear // 마나 사용량 증가 및 최대치 감소(10%) 및 마나 데미지(10% 최대치 감소 전 반영)
	// 최대치 감소 후 마나 데미지
}

}
// '광기' 이펙트 : (공격력 상승, 마나 소모량 증가, (마나 최대치 감소))