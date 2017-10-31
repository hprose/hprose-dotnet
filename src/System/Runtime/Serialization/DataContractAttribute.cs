/* DataContractAttribute class.
* This library is free. You can redistribute it and/or modify it.
*/

#if UNITY_WEBPLAYER

using System;
namespace System.Runtime.Serialization {
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, Inherited = false, AllowMultiple = false)]
	public sealed class DataContractAttribute : Attribute {
		private string name;
		private string ns;
		private bool isNameSetExplicit;
		private bool isNamespaceSetExplicit;
		private bool isReference;
		private bool isReferenceSetExplicit;
		public bool IsReference {
			get {
				return this.isReference;
			}
			set {
				this.isReference = value;
				this.isReferenceSetExplicit = true;
			}
		}
		internal bool IsReferenceSetExplicit {
			get {
				return this.isReferenceSetExplicit;
			}
		}
		public string Namespace {
			get {
				return this.ns;
			}
			set {
				this.ns = value;
				this.isNamespaceSetExplicit = true;
			}
		}
		internal bool IsNamespaceSetExplicit {
			get {
				return this.isNamespaceSetExplicit;
			}
		}
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
	}
}
#endif
