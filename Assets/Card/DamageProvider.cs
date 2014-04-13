using UnityEngine;
using System;
using System.Collections.Generic;

public class DamageProvider {

    public static Func<string, string, int, bool, int> DamageBoost = (string damagedGuid, string damagerGuid, int damage, bool realDamage) => {return 0;};

    protected int baseDamage;
    protected Func<int> dynamicDamage;

    public DamageProvider(int baseDamage = 0, Func<int> dynamicDamage = null) {
        this.baseDamage = baseDamage;
        this.dynamicDamage = dynamicDamage;
    }
    
    public virtual string Damager {
        get; set;
    }

    public virtual string Damaged {
        get; set;
    }

    public virtual int Provider() {
        int damage = baseDamage;
        if (dynamicDamage != null) {
            damage += dynamicDamage();
        }

        foreach (Func<string, string, int, bool, int> boost in DamageBoost.GetInvocationList()) {
            damage += boost(Damaged, Damager, damage, !TemplateStatus.Templating);
            damage = Mathf.Max(0, damage);
        }
        return damage;
    }

    public void Apply(string damaged = null, string damager = null) {
        if (damaged != null) {
            Damaged = damaged;
        }
        if (damager != null) {
            Damager = damager;
        }

        GameState.DamageGuid(Damaged, Damager, Provider());
    }
}

public class ActiveMorphidDamageProvider : DamageProvider {
    public ActiveMorphidDamageProvider(int baseDamage = 0, Func<int> dynamicDamage = null) : base(baseDamage, dynamicDamage) {}
    
    public override string Damager {
        get {
            if (GameState.Singleton.Morphids != null && GameState.ActiveMorphid != null) {
                return GameState.ActiveMorphid.GUID;
            }
            return null;
        }
        set {}
    }
}

public class MinionDamageProvider : DamageProvider {
    public Minion minion;

    public MinionDamageProvider(Minion minion) : base(0) {
        this.minion = minion;
    }
    
    public override string Damager {
        get {
            if (minion == null) {
                return null;
            }
            return minion.GUID;
        }
        set {}
    }
    
    public override int Provider() {
        if (minion == null) {
            return 0;
        }
        this.baseDamage = Mathf.Max(minion.InitialAttack - (minion.AffectedByTerrain(TerrainType.Flooded) ? 2 : 0), 0);
        return base.Provider();
    }
}