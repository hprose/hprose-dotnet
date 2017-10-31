/* DataMemberAttribute class.
* This library is free. You can redistribute it and/or modify it.
*/

#if UNITY_WEBPLAYER

using System;
namespace System.Runtime.Serialization {
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public sealed class DataMemberAttribute : Attribute {
		private string name;
		private bool isNameSetExplicit;
		private int order = -1;
		private bool isRequired;
		private bool emitDefaultValue = true;
		public string Name {
			get {
				return this.name;
			}
			set {
				this.name = value;
				this.isNameSetExplicit = true;
			}
		}
		internal bool IsNameSetExplicit {
			get {
				return this.isNameSetExplicit;
			}
		}
		public int Order {
			get {
				return this.order;
			}
			set {
				if (value < 0) {
					value = -1;
				}
				this.order = value;
			}
		}
		public bool IsRequired {
			get {
				return this.isRequired;
			}
			set {
				this.isRequired = value;
			}
		}
		public bool EmitDefaultValue {
			get {
				return this.emitDefaultValue;
			}
			set {
				this.emitDefaultValue = value;
			}
		}
	}
}

#endif
