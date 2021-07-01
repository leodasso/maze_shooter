using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("fractions", "hearts")]
	public class ES3UserType_Hearts : ES3Type
	{
		public static ES3Type Instance = null;

		public ES3UserType_Hearts() : base(typeof(Hearts)){ Instance = this; priority = 1;}


		public override void Write(object obj, ES3Writer writer)
		{
			var instance = (Hearts)obj;
			
			writer.WriteProperty("fractions", instance.fractions, ES3Type_int.Instance);
			writer.WriteProperty("hearts", instance.hearts, ES3Type_int.Instance);
		}

		public override object Read<T>(ES3Reader reader)
		{
			var instance = new Hearts();
			string propertyName;
			while((propertyName = reader.ReadPropertyName()) != null)
			{
				switch(propertyName)
				{
					
					case "fractions":
						instance.fractions = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "hearts":
						instance.hearts = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
			return instance;
		}
	}


	public class ES3UserType_HeartsArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_HeartsArray() : base(typeof(Hearts[]), ES3UserType_Hearts.Instance)
		{
			Instance = this;
		}
	}
}