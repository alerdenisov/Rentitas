using System;
using Uniful;
using UnityEngine;

namespace UnityTypeReference
{
    public enum TypesAllowed : byte
    {
        Class           = 1 << 0,
        Interface       = 1 << 1,
        Struct          = 1 << 2,
        Abstract        = 1 << 3,

        All             = Class | Interface | Struct | Abstract
    }

    /// <summary>
    /// Indicates how selectable classes should be collated in drop-down menu.
    /// </summary>
    public enum ClassGrouping
    {
        /// <summary>
        /// No grouping, just show type names in a list; for instance, "Some.Nested.Namespace.SpecialClass".
        /// </summary>
        None,

        /// <summary>
        /// Group classes by namespace and show foldout menus for nested namespaces; for
        /// instance, "Some > Nested > Namespace > SpecialClass".
        /// </summary>
        ByNamespace,

        /// <summary>
        /// Group classes by namespace; for instance, "Some.Nested.Namespace > SpecialClass".
        /// </summary>
        ByNamespaceFlat,

        /// <summary>
        /// Group classes in the same way as Unity does for its component menu. This
        /// grouping method must only be used for <see cref="MonoBehaviour"/> types.
        /// </summary>
        ByAddComponentMenu,
    }

    /// <summary>
    /// Base class for class selection constraints that can be applied when selecting
    /// a <see cref="ClassTypeReference"/> with the Unity inspector.
    /// </summary>
    public abstract class ClassTypeConstraintAttribute : PropertyAttribute
    {
        protected TypesAllowed _allowed = TypesAllowed.All;
        protected ClassGrouping _grouping = ClassGrouping.ByNamespace;

        /// <summary>
        /// Gets or sets grouping of selectable classes. Defaults to <see cref="ClassGrouping.ByNamespaceFlat"/>
        /// unless explicitly specified.
        /// </summary>
        public virtual ClassGrouping Grouping { get { return _grouping; } }
        public virtual TypesAllowed Allowed { get { return _allowed;  } }

        /// <summary>
        /// Gets or sets whether abstract classes can be selected from drop-down.
        /// Defaults to a value of <c>false</c> unless explicitly specified.
        /// </summary>
        public bool AllowAbstract => Allowed.HasFlag((byte)TypesAllowed.Abstract);

        public bool AllowInterfaces => Allowed.HasFlag((byte) TypesAllowed.Interface);

        public bool AllowStruct => Allowed.HasFlag((byte) TypesAllowed.Struct);

        public bool AllowClasses => Allowed.HasFlag((byte)TypesAllowed.Class);


        /// <summary>
        /// Determines whether the specified <see cref="Type"/> satisfies filter constraint.
        /// </summary>
        /// <param name="type">Type to test.</param>
        /// <returns>
        /// A <see cref="bool"/> value indicating if the type specified by <paramref name="type"/>
        /// satisfies this constraint and should thus be selectable.
        /// </returns>
        public virtual bool IsConstraintSatisfied(Type type)
        {
            return ValidateAbstract(type) && 
                   ValidateIntefaces(type) && 
                   ValidateClass(type);
        }

        private bool ValidateClass(Type type)
        {
            return AllowClasses == type.IsClass;
        }

        private bool ValidateIntefaces(Type type)
        {
            return AllowInterfaces == type.IsInterface;
        }

        private bool ValidateAbstract(Type type)
        {
            return AllowAbstract == type.IsAbstract;
        }
    }

    public class TypeAllowedAttribute : ClassTypeConstraintAttribute
    {
        public TypeAllowedAttribute(TypesAllowed allowed)
        {
            _allowed = allowed;
        }
    }

    /// <summary>
    /// Constraint that allows selection of classes that extend a specific class when
    /// selecting a <see cref="ClassTypeReference"/> with the Unity inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class ClassExtendsAttribute : TypeAllowedAttribute
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassExtendsAttribute"/> class.
        /// </summary>
        public ClassExtendsAttribute(TypesAllowed allowed = TypesAllowed.All) : base(allowed)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassExtendsAttribute"/> class.
        /// </summary>
        /// <param name="baseType">Type of class that selectable classes must derive from.</param>
        public ClassExtendsAttribute(Type baseType, TypesAllowed allowed = TypesAllowed.All) : base(allowed)
        {
            BaseType = baseType;
        }

        /// <summary>
        /// Gets the type of class that selectable classes must derive from.
        /// </summary>
        public Type BaseType { get; private set; }

        /// <inheritdoc/>
        public override bool IsConstraintSatisfied(Type type)
        {
            return base.IsConstraintSatisfied(type)
                   && BaseType.IsAssignableFrom(type) && type != BaseType;
        }

    }

    /// <summary>
    /// Constraint that allows selection of classes that implement a specific interface
    /// when selecting a <see cref="ClassTypeReference"/> with the Unity inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class ClassImplementsAttribute : TypeAllowedAttribute
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassImplementsAttribute"/> class.
        /// </summary>
        public ClassImplementsAttribute(TypesAllowed allowed = TypesAllowed.All) : base(allowed)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassImplementsAttribute"/> class.
        /// </summary>
        /// <param name="interfaceType">Type of interface that selectable classes must implement.</param>
        public ClassImplementsAttribute(Type interfaceType, TypesAllowed allowed = TypesAllowed.All) : base(allowed)
        {
            InterfaceType = interfaceType;
        }

        /// <summary>
        /// Gets the type of interface that selectable classes must implement.
        /// </summary>
        public Type InterfaceType { get; private set; }

        /// <inheritdoc/>
        public override bool IsConstraintSatisfied(Type type)
        {
            if (base.IsConstraintSatisfied(type))
            {
                foreach (var interfaceType in type.GetInterfaces())
                    if (interfaceType == InterfaceType)
                        return true;
            }
            return false;
        }

    }
}