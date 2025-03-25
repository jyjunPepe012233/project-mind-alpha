using MinD.SO.EnemySO;

namespace MinD.Runtime.Entity {

public class EnemyStateMachine : EntityOwnedHandler {

	public void SetState(EnemyState nextState) {

		if (nextState == null) {
			return;
		}
		
		if (nextState != ((Enemy)owner).currentState) {
			((Enemy)owner).currentState = nextState;
		}
	}
	
	public void HandleState() {

		if (((Enemy)owner).currentState == null) {
			return;
		}

		SetState(((Enemy)owner).currentState.Tick(((Enemy)owner)));
	}
	
}

}