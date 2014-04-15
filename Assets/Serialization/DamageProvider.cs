using UnityEngine;
using System;
using System.Collections.Generic;

public class DamageProvider {

    public static Action<Damage> DamageBoost = (Damage damage) => {};

    protected int baseDamage;
    protected Func<int> dynamicDamage;

    public DamageProvider(int baseDamage = 0, Func<int> dynamicDamage = null) {
        this.baseDamage = baseDamage;
        this.dynamicDamage = dynamicDamage;
        this.Type = DamageType.Card;
    }
    
    public virtual string Source {
        get; set;
    }

    public virtual string Target {
        get; set;
    }

    public virtual DamageType Type {
        get; set;
    }

    public virtual Damage ProvideDamage() {
        Damage damage = new Damage() {
            Target = Target,
            Source = Source,
            Magnitude = baseDamage,
            Type = Type
        };

        if (dynamicDamage != null) {
            damage.Magnitude += dynamicDamage();
        }

        DamageBoost(damage);
        return damage;
    }

    public virtual int Provider() {
        return ProvideDamage().Magnitude;
    }

    public void Apply(string target = null, string source = null) {
        if (target != null) {
            Target = target;
        }
        if (source != null) {
            Source = source;
        }

        GameState.DamageGuid(ProvideDamage());
    }

    public void ApplyLaneDamage(string target = null, string source = null) {
        if (target != null) {
            Target = target;
        }
        if (source != null) {
            Source = source;
        }

        Damage damage = ProvideDamage();
        GameState.LaneDamage(new LaneDamage() {
            EnemyMorphid = GameState.InactiveMorphid.GUID,
            Magnitude = damage.Magnitude,
            Target = damage.Target,
            Source = damage.Source
        });
    }
}

public class ActiveMorphidDamageProvider : DamageProvider {
    public ActiveMorphidDamageProvider(int baseDamage = 0, Func<int> dynamicDamage = null) : base(baseDamage, dynamicDamage) {}
    
    public override string Source {
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
    
    public override string Source {
        get {
            if (minion == null) {
                return null;
            }
            return minion.GUID;
        }
        set {}
    }

    public override DamageType Type {
        get {
            return DamageType.Attack;
        }
        set {
        }
    }
    
    public override int Provider() {
        if (minion == null) {
            return 0;
        }
        this.baseDamage = Mathf.Max(minion.InitialAttack - (minion.AffectedByTerrain(TerrainType.Flooded) ? 2 : 0), 0);
        return base.Provider();
    }
}