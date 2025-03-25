using MinD.Runtime.Entity;

namespace MinD.SO.Item {

public abstract class Equipment : Item {

	public abstract void OnEquip(Player owner);

	public abstract void Execute(Player owner);

	public abstract void OnUnequip(Player owner);

}

}