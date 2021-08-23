using System.Collections.ObjectModel;

namespace Bot_Test.MP.Scripts.Discord
{
    public class DeadEyeModifier
    {
        public readonly int precisionChange;
        public readonly string message;

        public static ObservableCollection<DeadEyeModifier> deadEyeModifiers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="precisionChange">Percentage of precision modification that will be applied to the weapon</param>
        /// <param name="message">Explain why this change</param>
        public DeadEyeModifier(int precisionChange, string message)
        {
            this.precisionChange = precisionChange;
            this.message = message;
            if (deadEyeModifiers == null)
                deadEyeModifiers = new ObservableCollection<DeadEyeModifier>();
            deadEyeModifiers.Add(this);
        }
    }
}