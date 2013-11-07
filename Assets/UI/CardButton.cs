using UnityEngine;
using System.Collections;

public class CardButton : Button {
	public TargetingRequirements TargetingRequirements;
	public int CardIndex;
	
	public CardButton(int CardIndex, Region source) : base(source) {
		this.CardIndex = CardIndex;
		Action = UI.Singleton.PickupCard(CardIndex);
	}
	
	protected override void DrawInner() {
		if (Morphid.Cards != null && Morphid.Cards[CardIndex] != null) {
			Text = Morphid.Cards[CardIndex].Name +
				" (" + Morphid.Cards[CardIndex].Cost + ")\n" +
				Morphid.Cards[CardIndex].Text;
		}
		else {
			Text = "Empty";
		}
		if (ContainsMouse() != null && Input.GetMouseButton(0) && Enabled) {
			Action();
			invalid = true;
		}
		base.DrawInner();
	}
}