using MinD.Structs;

namespace MinD.Runtime.Entity {

public abstract class BaseEntityAttributeHandler : EntityOwnedHandler {
	
	public abstract int MaxHp { get; set; }
	
	public abstract DamageNegation DamageNegation { get; set; }
	
	public abstract int PoiseBreakResistance { get; set; }
	
}

}