using UnityEngine;

[CreateAssetMenu(
    fileName = "HeroCardIconLibrary",
    menuName = "Heroes Landing/Icon Libraries/Hero Card Icons"
)]
public class HeroCardIconLibrary : ScriptableObject
{
    [Header("Role Icons")]
    public Sprite vanguardIcon;
    public Sprite tankIcon;
    public Sprite rangerRoleIcon;

    [Header("Class Icons")]
    public Sprite barbarianClassIcon;
    public Sprite knightClassIcon;
    public Sprite fighterClassIcon;
    public Sprite mageClassIcon;
    public Sprite priestClassIcon;
    public Sprite rangerClassIcon;
    public Sprite gunnerClassIcon;
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

    public Sprite GetRoleIcon(HeroCardRole selectedRole)
    {
        switch (selectedRole)
        {
            case HeroCardRole.Vanguard:
                return vanguardIcon;

            case HeroCardRole.Tank:
                return tankIcon;

            case HeroCardRole.Ranger:
                return rangerRoleIcon;

            default:
                return null;
        }
    }

    public Sprite GetClassIcon(HeroCardClass selectedClass)
    {
        switch (selectedClass)
        {
            case HeroCardClass.Barbarian:
                return barbarianClassIcon;

            case HeroCardClass.Knight:
                return knightClassIcon;

            case HeroCardClass.Fighter:
                return fighterClassIcon;

            case HeroCardClass.Mage:
                return mageClassIcon;

            case HeroCardClass.Priest:
                return priestClassIcon;

            case HeroCardClass.Ranger:
                return rangerClassIcon;

            case HeroCardClass.Gunner:
                return gunnerClassIcon;

            case HeroCardClass.Mech:
                return mechClassIcon;

            default:
                return null;
        }
    }

    public Sprite GetWeaponIcon(HeroWeaponClass selectedWeaponClass)
    {
        switch (selectedWeaponClass)
        {
            case HeroWeaponClass.Swordsman:
                return swordsmanIcon;

            case HeroWeaponClass.Axeman:
                return axemanIcon;

            case HeroWeaponClass.Spearman:
                return spearmanIcon;

            case HeroWeaponClass.Marksman:
                return marksmanIcon;

            case HeroWeaponClass.Archer:
                return archerIcon;

            case HeroWeaponClass.Fighter:
                return fighterWeaponIcon;

            case HeroWeaponClass.Assassin:
                return assassinIcon;

            default:
                return null;
        }
    }
}