﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace ITCC.NetworkView.Utils
{
    /// <summary>
    /// This class contains helper functions for dealing with WPF.
    /// </summary>
    public static class WpfUtils
    {
        /// <summary>
        /// Search up the element tree to find the Parent window for 'element'.
        /// Returns null if the 'element' is not attached to a window.
        /// </summary>
        public static Window FindParentWindow(FrameworkElement element)
        {
            if (element.Parent == null)
            {
                return null;
            }

            var window = element.Parent as Window;
            if (window != null)
            {
                return window;
            }

            var parentElement = element.Parent as FrameworkElement;
            if (parentElement != null)
            {
                return FindParentWindow(parentElement);
            }

            return null;
        }

        public static FrameworkElement FindParentWithDataContextAndName<TDataContextT>(FrameworkElement childElement, string name)
            where TDataContextT : class
        {
            var parent = (FrameworkElement)childElement.Parent;
            if (parent != null)
            {
                var data = parent.DataContext as TDataContextT;
                if (data != null)
                {
                    if (parent.Name == name)
                    {
                        return parent;
                    }
                }

                parent = FindParentWithDataContextAndName<TDataContextT>(parent, name);
                if (parent != null)
                {
                    return parent;
                }
            }

            parent = (FrameworkElement)childElement.TemplatedParent;
            if (parent != null)
            {
                var data = parent.DataContext as TDataContextT;
                if (data != null)
                {
                    if (parent.Name == name)
                    {
                        return parent;
                    }
                }

                parent = FindParentWithDataContextAndName<TDataContextT>(parent, name);
                if (parent != null)
                {
                    return parent;
                }
            }

            return null;
        }

        public static FrameworkElement FindParentWithDataContext<TDataContextT>(FrameworkElement childElement)
            where TDataContextT : class
        {
            if (childElement.Parent != null)
            {
                var data = ((FrameworkElement)childElement.Parent).DataContext as TDataContextT;
                if (data != null)
                {
                    return (FrameworkElement)childElement.Parent;
                }

                var parent = FindParentWithDataContext<TDataContextT>((FrameworkElement)childElement.Parent);
                if (parent != null)
                {
                    return parent;
                }
            }

            if (childElement.TemplatedParent != null)
            {
                var data = ((FrameworkElement)childElement.TemplatedParent).DataContext as TDataContextT;
                if (data != null)
                {
                    return (FrameworkElement)childElement.TemplatedParent;
                }

                var parent = FindParentWithDataContext<TDataContextT>((FrameworkElement)childElement.TemplatedParent);
                if (parent != null)
                {
                    return parent;
                }
            }

            return null;
        }

        public static TParentT FindVisualParentWithType<TParentT>(FrameworkElement childElement)
            where TParentT : class
        {
            var parentElement = (FrameworkElement)VisualTreeHelper.GetParent(childElement);
            if (parentElement != null)
            {
                var parent = parentElement as TParentT;
                if (parent != null)
                {
                    return parent;
                }

                return FindVisualParentWithType<TParentT>(parentElement);
            }

            return null;
        }

        public static TParentT FindParentWithType<TParentT>(FrameworkElement childElement)
            where TParentT : class
        {
            if (childElement.Parent != null)
            {
                var parent = childElement.Parent as TParentT;
                if (parent != null)
                {
                    return parent;
                }

                parent = FindParentWithType<TParentT>((FrameworkElement)childElement.Parent);
                if (parent != null)
        {
                    return parent;
                }
            }

            if (childElement.TemplatedParent != null)
            {
                var parent = childElement.TemplatedParent as TParentT;
                if (parent != null)
                {
                    return parent;
                }

                parent = FindParentWithType<TParentT>((FrameworkElement)childElement.TemplatedParent);
                if (parent != null)
                {
                    return parent;
                }
            }

            var parentElement = (FrameworkElement)VisualTreeHelper.GetParent(childElement);
            if (parentElement != null)
            {
                var parent = parentElement as TParentT;
                if (parent != null)
                {
                    return parent;
                }

                return FindParentWithType<TParentT>(parentElement);
            }

            return null;
        }

        public static TParentT FindParentWithTypeAndDataContext<TParentT>(FrameworkElement childElement, object dataContext)
            where TParentT : FrameworkElement
        {
            if (childElement.Parent != null)
            {
                var parent = childElement.Parent as TParentT;
                if (parent != null)
                {
                    if (parent.DataContext == dataContext)
                    {
                        return parent;
                    }
                }

                parent = FindParentWithTypeAndDataContext<TParentT>((FrameworkElement)childElement.Parent, dataContext);
                if (parent != null)
                {
                    return parent;
                }
            }

            if (childElement.TemplatedParent != null)
            {
                var parent = childElement.TemplatedParent as TParentT;
                if (parent != null)
                {
                    if (parent.DataContext == dataContext)
                    {
                        return parent;
                    }
                }

                parent = FindParentWithTypeAndDataContext<TParentT>((FrameworkElement)childElement.TemplatedParent, dataContext);
                if (parent != null)
                {
                    return parent;
                }
            }

            var parentElement = (FrameworkElement)VisualTreeHelper.GetParent(childElement);
            if (parentElement != null)
            {
                var parent = parentElement as TParentT;
                if (parent != null)
                {
                    return parent;
                }

                return FindParentWithType<TParentT>(parentElement);
            }

            return null;
        }

        /// <summary>
        /// Hit test against the specified element for a child that has a data context
        /// of the specified type.
        /// Returns 'null' if nothing was 'hit'.
        /// Return the highest level element that matches the hit test.
        /// </summary>
        public static T HitTestHighestForDataContext<T>(FrameworkElement rootElement, Point point)
            where T : class
        {
            FrameworkElement hitFrameworkElement = null;
            return HitTestHighestForDataContext<T>(rootElement, point, out hitFrameworkElement);
        }

        /// <summary>
        /// Hit test against the specified element for a child that has a data context
        /// of the specified type.
        /// Returns 'null' if nothing was 'hit'.
        /// Return the highest level element that matches the hit test.
        /// </summary>
        public static T HitTestHighestForDataContext<T>(FrameworkElement rootElement,
                                                  Point point, out FrameworkElement hitFrameworkElement)
            where T : class
        {
            hitFrameworkElement = null;

            FrameworkElement hitElement = null;
            var hitData = HitTestForDataContext<T, FrameworkElement>(rootElement, point, out hitElement);
            if (hitData == null)
            {
                return null;
            }

            hitFrameworkElement = hitElement;

            //
            // Find the highest level parent below root element that still matches the data context.
            while (hitElement != null && hitElement != rootElement &&
                   hitElement.DataContext == hitData)
            {
                hitFrameworkElement = hitElement;

                if (hitElement.Parent != null)
                {
                    hitElement = hitElement.Parent as FrameworkElement;
                    continue;
                }

                if (hitElement.TemplatedParent != null)
                {
                    hitElement = hitElement.TemplatedParent as FrameworkElement;
                    continue;
            }

                break;
        }

            return hitData;
        }


        /// <summary>
        /// Hit test for a specific data context and name.
        /// </summary>
        public static TDataContextT HitTestForDataContextAndName<TDataContextT, TElementT>(FrameworkElement rootElement, 
                                          Point point, string name, out TElementT hitFrameworkElement)
            where TDataContextT : class
            where TElementT : FrameworkElement
        {
            TDataContextT data = null;
            hitFrameworkElement = null;
            TElementT frameworkElement = null;

            VisualTreeHelper.HitTest(
                    rootElement,
                    // Hit test filter.
                    null,
                    // Hit test result.
                    delegate(HitTestResult result)
                    {
                        frameworkElement = result.VisualHit as TElementT;
                        if (frameworkElement != null)
                        {
                            data = frameworkElement.DataContext as TDataContextT;
                            if (data != null)
        {
                                if (frameworkElement.Name == name)
            {
                                    return HitTestResultBehavior.Stop;
                                }
                            }
                        }

                        return HitTestResultBehavior.Continue;
                    },

                    new PointHitTestParameters(point));

            hitFrameworkElement = frameworkElement;
            return data;
            }


        /// <summary>
        /// Hit test against the specified element for a child that has a data context
        /// of the specified type.
        /// Returns 'null' if nothing was 'hit'.
        /// </summary>
        public static TDataContextT HitTestForDataContext<TDataContextT, TElementT>(FrameworkElement rootElement, 
                                          Point point, out TElementT hitFrameworkElement)
            where TDataContextT : class
            where TElementT : FrameworkElement
        {
            TDataContextT data = null;
            hitFrameworkElement = null;
            TElementT frameworkElement = null;

            VisualTreeHelper.HitTest(
                    rootElement,
                    // Hit test filter.
                    null,
                    // Hit test result.
                    delegate(HitTestResult result)
                    {
                        frameworkElement = result.VisualHit as TElementT;
                        if (frameworkElement != null)
            {
                            data = frameworkElement.DataContext as TDataContextT;
                            return data != null ? HitTestResultBehavior.Stop : HitTestResultBehavior.Continue;
            }

                        return HitTestResultBehavior.Continue;
                    },

                    new PointHitTestParameters(point));

            hitFrameworkElement = frameworkElement;
            return data;
        }

        /// <summary>
        /// Find the ancestor of a particular element based on the type of the ancestor.
        /// </summary>
        public static T FindAncestor<T>(FrameworkElement element) where T : class
        {
            if (element.Parent != null)
            {
                var ancestor = element.Parent as T;
                if (ancestor != null)
                {
                    return ancestor;
                }

                var parent = element.Parent as FrameworkElement;
                if (parent != null)
                {
                    return FindAncestor<T>(parent);
                }
            }

            if (element.TemplatedParent != null)
            {
                var ancestor = element.TemplatedParent as T;
                if (ancestor != null)
                {
                    return ancestor;
                }

                var parent = element.TemplatedParent as FrameworkElement;
                if (parent != null)
                    {
                    return FindAncestor<T>(parent);
                    }
                }

            var visualParent = VisualTreeHelper.GetParent(element);
            if (visualParent != null)
            {
                var visualAncestor = visualParent as T;
                if (visualAncestor != null)
                {
                    return visualAncestor;
            }

                var visualElement = visualParent as FrameworkElement;
                if (visualElement != null)
            {
                    return FindAncestor<T>(visualElement);
                }
            }

                return null;
            }

        /// <summary>
        /// Transform a point to an ancestors coordinate system.
        /// </summary>
        public static Point TransformPointToAncestor<T>(FrameworkElement element, Point point) where T : Visual
        {
            var ancestor = FindAncestor<T>(element);
            if (ancestor == null)
            {
                throw new ApplicationException("Find to find '" + typeof(T).Name + "' for element '" + element.GetType().Name + "'.");
            }

            return TransformPointToAncestor(ancestor, element, point);
        }

        /// <summary>
        /// Transform a point to an ancestors coordinate system.
        /// </summary>
        public static Point TransformPointToAncestor(Visual ancestor, FrameworkElement element, Point point) => element.TransformToAncestor(ancestor).Transform(point);

        /// <summary>
        /// Find the framework element with the specified name.
        /// </summary>
        public static TElementT FindElementWithName<TElementT>(Visual rootElement, string name)
            where TElementT : FrameworkElement
        {
            var rootFrameworkElement = rootElement as FrameworkElement;
            if (rootFrameworkElement != null)
            {
                rootFrameworkElement.UpdateLayout();
            }

            var numChildren = VisualTreeHelper.GetChildrenCount(rootElement);
            for (var i = 0; i < numChildren; ++i)
            {
                var childElement = (Visual)VisualTreeHelper.GetChild(rootElement, i);

                var typedChildElement = childElement as TElementT;
                if (typedChildElement != null)
                {
                    if (typedChildElement.Name == name)
                {
                        return typedChildElement;
                    }
                }

                var foundElement = FindElementWithName<TElementT>(childElement, name);
                if (foundElement != null)
                {
                    return foundElement;
                }
            }

            return null;
        }

        /// <summary>
        /// Find the framework element for the specified connector.
        /// </summary>
        public static TElementT FindElementWithDataContextAndName<TDataContextT, TElementT>(Visual rootElement, TDataContextT data, string name)
            where TDataContextT : class
            where TElementT : FrameworkElement
        {
            Trace.Assert(rootElement != null);

            var rootFrameworkElement = rootElement as FrameworkElement;
            if (rootFrameworkElement != null)
        {
                rootFrameworkElement.UpdateLayout();
        }

            var numChildren = VisualTreeHelper.GetChildrenCount(rootElement);
            for (var i = 0; i < numChildren; ++i)
            {
                var childElement = (Visual)VisualTreeHelper.GetChild(rootElement, i);

                var typedChildElement = childElement as TElementT;
                if (typedChildElement != null &&
                    typedChildElement.DataContext == data)
                {
                    if (typedChildElement.Name == name)
                {
                        return typedChildElement;
                    }
                }

                var foundElement = FindElementWithDataContextAndName<TDataContextT, TElementT>(childElement, data, name);
                if (foundElement != null)
                {
                    return foundElement;
                }
            }

            return null;
        }

        /// <summary>
        /// Find the framework element for the specified connector.
        /// </summary>
        public static TElementT FindElementWithType<TElementT>(Visual rootElement)
            where TElementT : FrameworkElement
        {
            if (rootElement == null)
            {
                throw new ArgumentNullException(nameof(rootElement));
            }            

            var rootFrameworkElement = rootElement as FrameworkElement;
            if (rootFrameworkElement != null)
        {
                rootFrameworkElement.UpdateLayout();
            }

            //
            // Check each child.
            //
            var numChildren = VisualTreeHelper.GetChildrenCount(rootElement);
            for (var i = 0; i < numChildren; ++i)
            {
                var childElement = (Visual)VisualTreeHelper.GetChild(rootElement, i);

                var typedChildElement = childElement as TElementT;
                if (typedChildElement != null)
                    {
                    return typedChildElement;
                    }
                }

            //
            // Check sub-trees.
            //
            for (var i = 0; i < numChildren; ++i)
            {
                var childElement = (Visual)VisualTreeHelper.GetChild(rootElement, i);

                var foundElement = FindElementWithType<TElementT>(childElement);
                if (foundElement != null)
                {
                    return foundElement;
                }
            }

            return null;
        }

        /// <summary>
        /// Find the framework element for the specified connector.
        /// </summary>
        public static TElementT FindElementWithDataContext<TDataContextT, TElementT>(Visual rootElement, TDataContextT data) 
            where TDataContextT : class
            where TElementT : FrameworkElement
        {
            if (rootElement == null)
            {
                throw new ArgumentNullException(nameof(rootElement));
            }
            
            var rootFrameworkElement = rootElement as FrameworkElement;
            if (rootFrameworkElement != null)
            {
                rootFrameworkElement.UpdateLayout();
            }

            var numChildren = VisualTreeHelper.GetChildrenCount(rootElement);
            for (var i = 0; i < numChildren; ++i)
            {
                var childElement = (Visual)VisualTreeHelper.GetChild(rootElement, i);

                var typedChildElement = childElement as TElementT;
                if (typedChildElement != null &&
                    typedChildElement.DataContext == data)
                {
                    return typedChildElement;
                }

                var foundElement = FindElementWithDataContext<TDataContextT, TElementT>(childElement, data);
                if (foundElement != null)
                {
                    return foundElement;
                }
            }

            return null;
        }

        /// <summary>
        /// Walk up the visual tree and find a template for the specified type.
        /// Returns null if none was found.
        /// </summary>
        public static TDataTemplateT FindTemplateForType<TDataTemplateT>(Type type, FrameworkElement element)
            where TDataTemplateT : class
                    {
            var resource = element.TryFindResource(new DataTemplateKey(type));
            var dataTemplate = resource as TDataTemplateT;
            if (dataTemplate != null)
                        {
                return dataTemplate;
                        }

            if (type.BaseType != null &&
                type.BaseType != typeof(object))
            {
                dataTemplate = FindTemplateForType<TDataTemplateT>(type.BaseType, element);
                if (dataTemplate != null)
                {
                    return dataTemplate;
                    }
                }

            foreach (var interfaceType in type.GetInterfaces())
            {
                dataTemplate = FindTemplateForType<TDataTemplateT>(interfaceType, element);
                if (dataTemplate != null)
                {
                    return dataTemplate;
                }
            }

            return null;
        }

        /// <summary>
        /// Search the visual tree for template and instance it.
        /// </summary>
        public static FrameworkElement CreateVisual(Type type, FrameworkElement element, object dataContext)
        {
            var template = FindTemplateForType<DataTemplate>(type, element);
            if (template == null)
            {
                throw new ApplicationException("Failed to find DataTemplate for type " + type.Name);
            }

            var visual = (FrameworkElement)template.LoadContent();
            visual.Resources = element.Resources;
            visual.DataContext = dataContext;
            return visual;
        }

        /// <summary>
        /// Layout, measure and arrange the specified element.
        /// </summary>
        public static void InitaliseElement(FrameworkElement element)
        {
            element.UpdateLayout();
            element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            element.Arrange(new Rect(0, 0, element.DesiredSize.Width, element.DesiredSize.Height));
        }


        /// <summary>
        /// Finds a particular type of UI element int he visual tree that has the specified data context.
        /// </summary>
        public static ICollection<T> FindTypedElements<T>(DependencyObject rootElement) where T : DependencyObject
        {
            var foundElements = new List<T>();
            FindTypedElements(rootElement, foundElements);
            return foundElements;
        }

        /// <summary>
        /// Finds a particular type of UI element int he visual tree that has the specified data context.
        /// </summary>
        private static void FindTypedElements<T>(DependencyObject rootElement, List<T> foundElements) where T : DependencyObject
        {
            var numChildren = VisualTreeHelper.GetChildrenCount(rootElement);
            for (var i = 0; i < numChildren; ++i)
            {
                var childElement = VisualTreeHelper.GetChild(rootElement, i);
                if (childElement is T)
                {
                    foundElements.Add((T)childElement);
                    continue;
                }

                FindTypedElements(childElement, foundElements);
                }
            }

        /// <summary>
        /// Recursively dump out all elements in the visual tree.
        /// </summary>
        public static void DumpVisualTree(Visual root) => DumpVisualTree(root, 0);

        /// <summary>
        /// Recursively dump out all elements in the visual tree.
        /// </summary>
        private static void DumpVisualTree(Visual root, int indentLevel)
        {
            var indentStr = new string(' ', indentLevel * 2);
            Trace.Write(indentStr);
            Trace.Write(root.GetType().Name);

            var rootElement = root as FrameworkElement;
            if (rootElement != null)
        {
                if (rootElement.DataContext != null)
            {
                    Trace.Write(" [");
                    Trace.Write(rootElement.DataContext.GetType().Name);
                    Trace.Write("]");
            }
            }

            Trace.WriteLine("");

            var numChildren = VisualTreeHelper.GetChildrenCount(root);
            if (numChildren > 0)
            {
                Trace.Write(indentStr);
                Trace.WriteLine("{");

                for (var i = 0; i < numChildren; ++i)
                {
                    var child = (Visual)VisualTreeHelper.GetChild(root, i);
                    DumpVisualTree(child, indentLevel + 1);
            }

                Trace.Write(indentStr);
                Trace.WriteLine("}");
            }
        }
    }
}
