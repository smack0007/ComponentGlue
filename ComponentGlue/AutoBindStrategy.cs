using System;
using System.Collections.Generic;
using System.Linq;

namespace ComponentGlue
{
    public class AutoBindStrategy : IAutoBindStrategy
    {
        Type defaultComponentAttributeType;

        /// <summary>
        /// The attribute type which indicates injection.
        /// </summary>
        public Type DefaultComponentAttributeType
        {
            get { return this.defaultComponentAttributeType; }

            set
            {
                if (!typeof(Attribute).IsAssignableFrom(value))
                    throw new InvalidOperationException(string.Format("{0} is not an Attribute type.", value));

                if (!typeof(IDefaultComponentAttribute).IsAssignableFrom(value))
                    throw new InvalidOperationException("DefaultComponentAttributeType must be assignable from IDefaultComponentAttribute.");

                this.defaultComponentAttributeType = value;
            }
        }

        /// <summary>
        /// The bind type to use for components binded with the strategy.
        /// </summary>
        public ComponentBindType BindType
        {
            get;
            set;
        }

        /// <summary>
        /// A filter through which all interface types will be run through.
        /// </summary>
        public Func<Type, bool> InterfaceTypeFilter
        {
            get;
            set;
        }

        /// <summary>
        /// A filter through which all component types will be run through.
        /// </summary>
        public Func<Type, bool> ComponentTypesFilter
        {
            get;
            set;
        }

        /// <summary>
        /// Instructs the strategy to throw an exception when a component cannot be found for an interface because
        /// no component implements the interface or because multiple components implement the interface and none is
        /// marked as the default component.
        /// </summary>
        public bool ThrowOnUnresolved
        {
            get;
            set;
        }

        public AutoBindStrategy()
        {
            this.defaultComponentAttributeType = typeof(DefaultComponentAttribute);
            this.BindType = ComponentBindType.Transient;
        }

        private Type FindDefaultComponentByAttribute(Type interfaceType, IEnumerable<Type> componentTypes)
        {
            Type defaultComponentType = null;

            foreach (Type componentType in componentTypes)
            {
                foreach (IDefaultComponentAttribute attribute in componentType.GetCustomAttributes(this.defaultComponentAttributeType, false))
                {
                    if (attribute.ComponentType == interfaceType)
                    {
                        if (defaultComponentType != null)
                            throw new InvalidOperationException(string.Format("More than one component is marked as the default component for type {0}.", interfaceType));

                        defaultComponentType = componentType;
                    }
                }
            }

            return defaultComponentType;
        }

        private void UnresolvedComponent(Type interfaceType, bool multiple)
        {
            if (this.ThrowOnUnresolved)
            {
                throw new AutoBindException(
                    string.Format(
                        "Unable to auto bind interface {0} because there are {1} implementors.",
                        interfaceType,
                        multiple ? "multiple" : "no"));
            }
        }

        public void Bind(ComponentContainer container, Type interfaceType, IList<Type> componentTypes)
        {            
            if (container == null)
                throw new ArgumentNullException("container");

            if (interfaceType == null)
                throw new ArgumentNullException("interfaceType");

            if (componentTypes == null)
                throw new ArgumentNullException("componentTypes");

            if (this.InterfaceTypeFilter != null && !this.InterfaceTypeFilter(interfaceType))
                return;

            if (componentTypes.Count == 0)
                this.UnresolvedComponent(interfaceType, false);

            if (!container.HasBinding(interfaceType))
            {
                var componentTypesEnumerable = componentTypes.AsEnumerable();

                if (this.ComponentTypesFilter != null)
                    componentTypesEnumerable = componentTypesEnumerable.Where(this.ComponentTypesFilter);

                if (componentTypesEnumerable.Count() == 1) // One implementor so we have the default binding
                {
                    container.Bind(interfaceType).To(componentTypesEnumerable.First()).As(this.BindType);
                }
                else
                {
                    Type defaultComponentType = this.FindDefaultComponentByAttribute(interfaceType, componentTypesEnumerable);

                    if (defaultComponentType != null)
                    {
                        container.Bind(interfaceType).To(defaultComponentType).As(this.BindType);
                    }
                    else
                    {
                        this.UnresolvedComponent(interfaceType, true);
                    }
                }
            }
        }
    }
}
