using ZigBeeNet.ZCL.Protocol;

namespace ZigBeeNet.ZCL
{
    public class ZclRawCommand : ZclCommand
    {
        internal override void Serialize(ZclFieldSerializer serializer)
        {
            if (Fields != null)
            {
                serializer.Serialize(Fields, ZclDataType.Get(DataType.UNSIGNED_8_BIT_INTEGER_ARRAY));
            }
        }

        internal override void Deserialize(ZclFieldDeserializer deserializer)
        {
            Fields = deserializer.Deserialize<byte[]>(ZclDataType.Get(DataType.UNSIGNED_8_BIT_INTEGER_ARRAY));
        }
    }
}
