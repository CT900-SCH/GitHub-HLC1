using UnityEngine;

[CreateAssetMenu(
    fileName = "UnitCardIconLibrary",
    menuName = "Heroes Landing/Icon Libraries/Unit Card Icons"
)]
public class UnitCardIconLibrary : ScriptableObject
{
    [Header("Role Icons")]

    public Sprite vanguardIcon;
    public Sprite tankIcon;
    public Sprite rangerRoleIcon;

    [Header("Class Icons")]

    public Sprite gunnerClassIcon;
    public Sprite fighterClassIcon;
    public Sprite rangerClassIcon;
    public Sprite barbarianClassIcon;
    public Sprite knightClassIcon;
    public Sprite mageClassIcon;
    public Sprite priestClassIcon;
    public Sprite mechClassIcon;

    [Header("Weapon Class Icons")]

    public Sprite swordsmanIcon;
    public Sprite axemanIcon;
    public Sprite spearmanIcon;
    public Sprite marksmanIcon;
    public Sprite archerIcon;
    public Sprite fighterWeaponIcon;
    public Sprite assassinIcon;

    [Header("Weapon Level Box")]

    public Sprite whiteLevelBox;

    public Sprite GetRoleIcon(
        UnitCardRole selectedRole
    )
    {
        switch (selectedRole)
        {
            case UnitCardRole.Vanguard:
                return vanguardIcon;

            case UnitCardRole.Tank:
                return tankIcon;

            case UnitCardRole.Ranger:
                return rangerRoleIcon;

            default:
                return null;
        }
    }

    public Sprite GetClassIcon(
        UnitCardClass selectedClass
    )
    {
        switch (selectedClass)
        {
            case UnitCardClass.Gunner:
                return gunnerClassIcon;

            case UnitCardClass.Fighter:
                return fighterClassIcon;

            case UnitCardClass.Ranger:
                return rangerClassIcon;

            case UnitCardClass.Barbarian:
                return barbarianClassIcon;

            case UnitCardClass.Knight:
                return knightClassIcon;

            case UnitCardClass.Mage:
                return mageClassIcon;

            case UnitCardClass.Priest:
                return priestClassIcon;

            case UnitCardClass.Mech:
                return mechClassIcon;

            default:
                return null;
        }
    }

    public Sprite GetWeaponIcon(
        UnitWeaponClass selectedWeaponClass
    )
    {
        switch (selectedWeaponClass)
        {
            case UnitWeaponClass.Swordsman:
                return swordsmanIcon;

            case UnitWeaponClass.Axeman:
                return axemanIcon;

            case UnitWeaponClass.Spearman:
                return spearmanIcon;

            case UnitWeaponClass.Marksman:
                return marksmanIcon;

            case UnitWeaponClass.Archer:
                return archerIcon;

            case UnitWeaponClass.Fighter:
                return fighterWeaponIcon;

            case UnitWeaponClass.Assassin:
                return assassinIcon;

            default:
                return null;
        }
    }
}