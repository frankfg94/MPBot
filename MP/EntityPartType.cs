namespace BT.MP
{


        public enum EntityPartType
        {

        /// <summary>
        /// Zone Personnage
        /// </summary>
        Leg,
        Knee,
        Eye,
        Ear,
        Nose,
        Heart,
        Shoulder,
        Head,
        Chest,
        Hand,
        Arm,
        Foot,
        LeftWing, // ex: Dragon
        RightWing,
        Neck, // ex : Dragon, alien à long cou
        Tail, // ex : Dragon

        // ZONES SENSIBLES +18
        Balls,
        Penis,
        Boobs,
        Vagina,

        Global, // Ex: champ de force de Waver
        EquippedWeapon, // Pour désarmer ou détruire l'arme
        HolsterWeapon, // Pistolet rangé
        BackWeapon, // Arme rangée

        /// <summary>
        ///  Zone Vehicule
        /// </summary>
        Track, // Chenille d'un Tank
        Wheel,
        Canon,
        TransportArea, // Zone arriere d'un camion
        FuelContainer,
        Mirror, // Rétroviseur
        Turret, // Haut d'un tank
        MainMachineGun,
        SubMachineGun,
        Door,
        Motor,
        Windshield, // pare-brise
        Reactor,
        Propeller, // Hélice principale
        BackPropeller, // Hélice arrière
        Antenna,
        }
}
