using ColossalFramework;
using ICities;
using UnityEngine;

namespace CargoInfoMod
{
    public class ModInfo : IUserMod
    {
        public const string Namespace = "com.github.rumkex.cargomod";
        public const string NamespaceV1 = "com.github.rumkex.cargomod.vlk";
        public readonly int version = 1;

        public string Name => "Cargo Info";

        public string Description => "Displays statistics panel for Cargo Stations service view and allows monitoring cargo dynamics";

        internal CargoData data;

        internal Options Options = new Options();

        internal static bool CleanCurrentData { get; set; }

        public void OnSettingsUI(UIHelperBase helper)
        {
            var trucksCounterGroup = helper.AddGroup("Trucks counter");
            trucksCounterGroup.AddCheckbox("Use months instead of weeks", Options.UseMonthlyValues, state => Options.UseMonthlyValues.value = state);

            var dataUpdateGroup = helper.AddGroup("Update statistics interval\n\n" +
                "Note: the original mod uses mounthly update in the \"Cargo Statistics\" window.\n" +
                "Seems too long for the \"Real Time\" mod users.");
            dataUpdateGroup.AddCheckbox("Use hourly update", Options.UpdateHourly, state => Options.UpdateHourly.value = state);

            var settingsGroup = helper.AddGroup("Data Cleaning");
            //helper.AddSpace(10);
            settingsGroup.AddButton("Clean old data", () => { data.RemoveData(Namespace); });
            settingsGroup.AddButton("Clean current data", () => { data.RemoveData(NamespaceV1); CleanCurrentData = true; });
        }
    }

    internal class Options
    {
        internal SavedBool UseMonthlyValues = new SavedBool("useMonthlyCargoValues", Settings.gameSettingsFile);
        internal SavedBool UpdateHourly = new SavedBool("updateHourly", Settings.gameSettingsFile);
    }
}
