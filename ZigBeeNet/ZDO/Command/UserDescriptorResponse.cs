using System;
using System.Text;
using ZigBeeNet.Transaction;
using ZigBeeNet.ZCL;
using ZigBeeNet.ZCL.Protocol;
using ZigBeeNet.ZDO.Field;

namespace ZigBeeNet.ZDO.Command
{
    /**
    * User Descriptor Response value object class.
    * 
    * The User_Desc_rsp is generated by a remote device in response to a
    * User_Desc_req directed to the remote device. This command shall be unicast to
    * the originator of the User_Desc_req command.
    * 
    */
    public class UserDescriptorResponse : ZdoResponse
    {
        /**
        * NWKAddrOfInterest command message field.
        */
        public int NwkAddrOfInterest { get; set; }

        /**
        * Length command message field.
        */
        public int Length { get; set; }

        /**
        * UserDescriptor command message field.
        */
        public UserDescriptor UserDescriptor { get; set; }

        /**
        * Default constructor.
        */
        public UserDescriptorResponse()
        {
            ClusterId = 0x8011;
        }

        public override void Serialize(ZclFieldSerializer serializer)
        {
            base.Serialize(serializer);

            serializer.Serialize(Status, ZclDataType.Get(DataType.ZDO_STATUS));
            serializer.Serialize(NwkAddrOfInterest, ZclDataType.Get(DataType.NWK_ADDRESS));
            serializer.Serialize(Length, ZclDataType.Get(DataType.UNSIGNED_8_BIT_INTEGER));
            serializer.Serialize(UserDescriptor, ZclDataType.Get(DataType.USER_DESCRIPTOR));
        }

        public override void Deserialize(ZclFieldDeserializer deserializer)
        {
            base.Deserialize(deserializer);

            Status = (ZdoStatus)deserializer.Deserialize(ZclDataType.Get(DataType.ZDO_STATUS));
            if (Status != ZdoStatus.SUCCESS)
            {
                // Don't read the full response if we have an error
                return;
            }
            NwkAddrOfInterest = (int)deserializer.Deserialize(ZclDataType.Get(DataType.NWK_ADDRESS));
            Length = (int)deserializer.Deserialize(ZclDataType.Get(DataType.UNSIGNED_8_BIT_INTEGER));
            UserDescriptor = (UserDescriptor)deserializer.Deserialize(ZclDataType.Get(DataType.USER_DESCRIPTOR));
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("UserDescriptorResponse [")
                   .Append(base.ToString())
                   .Append(", status=")
                   .Append(Status)
                   .Append(", nwkAddrOfInterest=")
                   .Append(NwkAddrOfInterest)
                   .Append(", length=")
                   .Append(Length)
                   .Append(", userDescriptor=")
                   .Append(UserDescriptor)
                   .Append(']');

            return builder.ToString();
        }
    }
}
