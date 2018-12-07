﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ZigBeeNet.CC.Packet.ZDO
{
    /// <summary>
    /// This command is generated to inquire about the Node Descriptor information of the destination device
    /// </summary>
    public class ZDO_NODE_DESC_REQ : ZToolPacket
    {
        /// <summary>
        /// Specifies NWK address of the device generating the inquiry
        /// </summary>
        public ZigBeeAddress16 DstAddr { get; private set; }

        /// <summary>
        /// Specifies NWK address of the destination device being queried
        /// </summary>
        public ZigBeeAddress16 NwkAddrOfInterest { get; private set; }

        public ZDO_NODE_DESC_REQ(ZigBeeAddress16 dstAddr, ZigBeeAddress16 nwkAddrOfinterest)
        {
            DstAddr = dstAddr;
            NwkAddrOfInterest = nwkAddrOfinterest;

            byte[] framedata = new byte[4];
            framedata[0] = DstAddr.DoubleByte.Lsb;
            framedata[1] = DstAddr.DoubleByte.Msb;
            framedata[2] = NwkAddrOfInterest.DoubleByte.Lsb;
            framedata[3] = NwkAddrOfInterest.DoubleByte.Msb;

            BuildPacket(new DoubleByte(ZToolCMD.ZDO_NODE_DESC_REQ), framedata);
        }
    }
}
