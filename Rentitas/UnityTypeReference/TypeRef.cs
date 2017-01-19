using System;
using UnityEngine;

namespace UnityTypeReference
{
    [Serializable]
    public class TypeRef : ISerializationCallbackReceiver
    {
        public Type Type
        {
            get { return _type; }
            private set
            {
                _type = value;
                _typeRef = GetStringRef(value);
            }
        }

        /// <summary>
        /// Serializable field with string representation of type
        /// </summary>
        [SerializeField]
        private string _typeRef;

        private Type _type;

        public static string GetStringRef(Type type)
        {
            return type != null
                ? type.FullName + ", " + type.Assembly.GetName().Name
                : "";
        }

        public TypeRef(string assemblyQualifiedClassName)
        {
            Type = !string.IsNullOrEmpty(assemblyQualifiedClassName)
                ? Type.GetType(assemblyQualifiedClassName)
                : null;
        }

        public TypeRef(Type type)
        {
            Type = type;
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (!string.IsNullOrEmpty(_typeRef))
            {
                _type = System.Type.GetType(_typeRef);

                if (_type == null)
                    Debug.LogWarning($"'{_typeRef}' was referenced but class type was not found.");
            }
            else
            {
                _type = null;
            }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }


        public static implicit operator string(TypeRef typeRef)
        {
            return typeRef._typeRef;
        }

        public static implicit operator Type(TypeRef typeRef)
        {
            return typeRef.Type;
        }

        public static implicit operator TypeRef(Type type)
        {
            return new TypeRef(type);
        }

        public override string ToString()
        {
            return Type != null ? Type.FullName : "(None)";
        }

    }
}