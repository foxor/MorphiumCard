using UnityEngine;
using System.Collections;

public class AttackProcessor : MonoBehaviour {
	
	protected static AttackProcessor Singleton;
	
	protected Card processing;
	protected UI.Region root;
	
	public void Awake() {
		Singleton = this;
		root = new UI.Region();
		UI.Region top = root.Bisect(UI.Region.Side.Top, 200);
		UI.Region[] buttons = top.Split(UI.Region.Direction.Horizontal, 4);
		new UI.Button(buttons[0]) {
			Text = "Head",
			Action = () => {}
		};
		new UI.Button(buttons[1]) {
			Text = "Chest",
			Action = () => {}
		};
		new UI.Button(buttons[2]) {
			Text = "Legs",
			Action = () => {}
		};
		new UI.Button(buttons[3]) {
			Text = "Arm",
			Action = () => {}
		};
	}
	
	public static void Process(Card c) {
		if (c.Type == CardType.Attack) {
			this.processing = c;
		}
	}
	
	public void OnGUI() {
		if (processing != null) {
			root.Draw();
		}
	}
}