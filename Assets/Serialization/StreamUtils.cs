using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Linq;

using ProtoBuf;
using ProtoBuf.Meta;

public static class StreamUtils {
	
	public static Encoding ENCODING = System.Text.Encoding.UTF8;
	
	public class BitStreamEnvelope {
		protected BitStream target;
		public BitStreamEnvelope(BitStream target) {
			this.target = target;
		}
		
		public void WriteBytes(byte[] value) {
			int len = value.Length;
			target.Serialize(ref len);
			foreach (byte b in value) {
				short B = (short)b;
				target.Serialize(ref B);
			}
		}
		
		public byte[] ReadBytes() {
			int len = 0;
			target.Serialize(ref len);
			byte[] bytes = new byte[len];
			for (int i = 0; len > 0; len--) {
				short b = 0;
				target.Serialize(ref b);
				bytes[i++] = (byte)b;
			}
			return bytes;
		}
	}
	
	public static void SerializeProto<T>(this BitStream bits, ref T val) {
		BitStreamEnvelope s = new BitStreamEnvelope(bits);
		if (bits.isWriting) {
			s.WriteBytes(val.SerializeProtoBytes<T>());
		}
		else {
			val = s.ReadBytes().DeserializeProtoBytes<T>();
		}
	}
	
	public static byte[] SerializeProtoBytes<T>(this T val) {
		MemoryStream s = new MemoryStream();
		Serializer.Serialize<T>(s, val);
		return s.GetBuffer().Take((int)s.Position).ToArray();
	}
	
	public static string SerializeProtoString<T>(this T val) {
		return ENCODING.GetString(val.SerializeProtoBytes<T>());
	}
	
	public static T DeserializeProtoBytes<T>(this byte[] val) {
		MemoryStream s = new MemoryStream(val);
		return Serializer.Deserialize<T>(s);
	}
	
	public static T DeserializeProtoString<T>(this string val) {
		return ENCODING.GetBytes(val).DeserializeProtoBytes<T>();
	}
}