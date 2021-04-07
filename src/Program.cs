#region Header
using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Extensions;
using VRageMath;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;

namespace SpaceEngineers.IngameScript.CargoWatch
{
    public sealed class Program : MyGridProgram
    {
        #endregion
        const string PROP_MASS_WATERM_VAL = "Mass.Watermark.Value";
        const string PROP_MASS_WATERM_ACT = "Mass.Watermark.Activate";
        const string PROP_VOL_WATERM_VAL = "Volume.Watermark.Value";
        const string PROP_VOL_WATERM_ACT = "Volume.Watermark.Activate";

        long massWatermarkValue = 0;
        IMyFunctionalBlock massWatermarkActivateBlock;
        string massWatermarkActivate;
        long volumeWatermarkValue = 0;
        IMyFunctionalBlock volumeWatermarkActivateBlock;
        string volumeWatermarkActivate;
        IMyTextSurface surface;

        public Program()
        {
            Dictionary<String, String> properties = ReadProperties(Me.CustomData);

            if (properties.ContainsKey(PROP_MASS_WATERM_VAL))
            {
                massWatermarkValue = Int64.Parse(properties[PROP_MASS_WATERM_VAL] as string);
            }

            if (properties.ContainsKey(PROP_MASS_WATERM_ACT))
            {
                massWatermarkActivate = properties[PROP_MASS_WATERM_ACT] as string;
                massWatermarkActivateBlock = GridTerminalSystem.GetBlockWithName(massWatermarkActivate) as IMyFunctionalBlock;
            }

            if (properties.ContainsKey(PROP_VOL_WATERM_VAL))
            {
                volumeWatermarkValue = Int64.Parse(properties[PROP_VOL_WATERM_VAL] as string);
            }

            if (properties.ContainsKey(PROP_VOL_WATERM_ACT))
            {
                volumeWatermarkActivate = properties[PROP_VOL_WATERM_ACT] as string;
                volumeWatermarkActivateBlock = GridTerminalSystem.GetBlockWithName(volumeWatermarkActivate) as IMyFunctionalBlock;
            }

            Runtime.UpdateFrequency = UpdateFrequency.Update100;

            surface = Me.GetSurface(0);
            surface.ContentType = ContentType.TEXT_AND_IMAGE;
        }

        public void Main(string argument, UpdateType updateSource)
        {
            surface.WriteText("");

            IMyInventory inventory;
            int currentMass = 0;
            int currentVolume = 0;
            int maxVolume = 0;
            List<IMyCubeBlock> inventoryEntities = new List<IMyCubeBlock>();

            // Filter: only functional inventories and on same grid
            GridTerminalSystem.GetBlocksOfType(inventoryEntities, entity => entity.CubeGrid == Me.CubeGrid && entity.HasInventory && entity.IsFunctional);

            foreach (IMyCubeBlock invEnt in inventoryEntities)
            {
                inventory = invEnt.GetInventory();
                currentMass += inventory.CurrentMass.ToIntSafe();
                currentVolume += inventory.CurrentVolume.ToIntSafe();
                maxVolume += inventory.MaxVolume.ToIntSafe();
            }

            bool massWatermarkValid = (massWatermarkValue != 0);
            bool massWatermarkReached = massWatermarkValid && (currentMass >= massWatermarkValue);

            if (massWatermarkReached)
            {
                Print($"WARNING! Watermark for mass reached: {currentMass}/{massWatermarkValue}\n", surface);
            }

            Print("MASS\n----------", surface);
            Print($"Current: {currentMass} kg", surface);
            if (massWatermarkValid)
            {
                Print($"Watermark: {massWatermarkValue} kg", surface);
            }
            else
            {
                Print("Watermark: n/a", surface);
            }
            if (massWatermarkActivateBlock != null)
            {
                Echo($"Watermark block: '{massWatermarkActivate}'");
                massWatermarkActivateBlock.Enabled = massWatermarkReached;
            }
            else if (massWatermarkActivate != null)
            {
                Echo($"Watermark block: '{massWatermarkActivate}' NOT FOUND!");
            }
            else
            {
                Echo($"Watermark block: n/a");
            }

            bool volumeWatermarkValid = (volumeWatermarkValue != 0);
            bool volumeWatermarkReached = volumeWatermarkValid && (currentVolume * 1000 >= volumeWatermarkValue);

            if (volumeWatermarkReached)
            {
                Print($"WARNING! Watermark for volume reached: {currentVolume * 1000}/{volumeWatermarkValue}\n", surface);
            }

            Print("\nVOLUME\n----------", surface);
            Print($"Current volume: {currentVolume * 1000} L", surface);
            Print($"Maximum volume: {maxVolume * 1000} L", surface);

            if (volumeWatermarkValid)
            {
                Print($"Watermark: {volumeWatermarkValue} L", surface);
            }
            else
            {
                Print("Watermark: n/a", surface);
            }
            if (volumeWatermarkActivateBlock != null)
            {
                Echo($"Watermark block: '{volumeWatermarkActivate}'");
                volumeWatermarkActivateBlock.Enabled = volumeWatermarkReached;
            }
            else if (volumeWatermarkActivate != null)
            {
                Echo($"Watermark block: '{volumeWatermarkActivate}' NOT FOUND!");
            }
            else
            {
                Echo($"Watermark block: n/a");
            }
        }

        void Print(string text, IMyTextSurface surface)
        {
            surface.WriteText($"{text}\n", true);
            Echo(text);
        }

        private Dictionary<String, String> ReadProperties(string source)
        {
            Dictionary<String, String> result = new Dictionary<String, String>();

            if (source == null || source.Length < 1)
            {
                return result;
            }

            string[] lines = source.Split('\n');
            string[] pair;

            foreach (var line in lines)
            {
                pair = line.Split('=');
                result.Add(pair[0], pair[1]);
            }

            return result;
        }
        #region Footer
    }
}
#endregion
