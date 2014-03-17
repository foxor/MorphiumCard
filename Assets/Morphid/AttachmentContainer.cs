using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;
using ProtoBuf.Meta;

[Serializable]
[ProtoContract]
public class AttachmentContainer {
    
    [SerializeField]
    [ProtoMember(1)]
    public Attachment[] Attachments;

    public void Damage(int damage) {
        foreach (Attachment attachment in Attachments) {
            if (attachment != null) {
                attachment.Damage(damage);
            }
        }
    }

    public void Attach(Slot slot, Attachment attachment) {
        int slotIndex = slot.Order();
        if (Attachments[slotIndex] != null) {
            Attachments[slotIndex].OnDestroy();
        }
        Attachments[slotIndex] = attachment;
    }

    public void Setup () {
        Slot[] Slots = Enum.GetValues(typeof(Slot)).Cast<Slot>().OrderBy(x => x.Order()).ToArray();
        Attachments = new Attachment[Slots.Count()];
        for (int i = 0; i < Slots.Count(); i++) {
            Attachments[i] = null;
        }
    }

    public void Retemplate() {
        foreach (Attachment attachment in Attachments) {
            if (attachment != null) {
                attachment.Template();
            }
        }
    }
}