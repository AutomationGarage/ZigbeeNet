﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZigBeeNet
{

    public enum ProfileType
    {
        /**
         * ZigBee Home Automation
         */
        ZIGBEE_HOME_AUTOMATION,

        /**
         * ZigBee Smart Energy
         */
        ZIGBEE_SMART_ENERGY,

        /**
         * ZigBee Green Power
         */
        ZIGBEE_GREEN_POWER,

        /**
         * Manufacturer Telegesis
         */
        MANUFACTURER_TELEGESIS,

        /**
         * ZigBee Light Link
         */
        ZIGBEE_LIGHT_LINK,

        /**
         * Manufacturer Digi
         */
        MANUFACTURER_DIGI,
    }

    public class ZigBeeProfileType
    {
        private static readonly Dictionary<int, ZigBeeProfileType> _idMap;

        public int Key { get; private set; }

        public ProfileType ProfileType { get; private set; }


        private ZigBeeProfileType(int key, ProfileType profileType)
        {
            this.Key = key;
        }

        static ZigBeeProfileType()
        {
            _idMap = new Dictionary<int, ZigBeeProfileType>
            {
                { 0x0104, new ZigBeeProfileType(0x0104, ProfileType.ZIGBEE_HOME_AUTOMATION) },
                { 0x0109, new ZigBeeProfileType(0x0109, ProfileType.ZIGBEE_SMART_ENERGY) },
                { 0xA10E, new ZigBeeProfileType(0xA10E, ProfileType.ZIGBEE_GREEN_POWER) },
                { 0xC059, new ZigBeeProfileType(0xA10E, ProfileType.MANUFACTURER_TELEGESIS) },
                { 0xC05E, new ZigBeeProfileType(0xC05E, ProfileType.ZIGBEE_LIGHT_LINK) },
                { 0xC105, new ZigBeeProfileType(0xC105, ProfileType.MANUFACTURER_DIGI) },
            };
        }

        public static ZigBeeProfileType Get(int value)
        {
            return _idMap[value];
        }

        public static ZigBeeProfileType Get(ProfileType profileType)
        {
            return _idMap.Values.Single(pt => pt.ProfileType == profileType);
        }
    }
}
