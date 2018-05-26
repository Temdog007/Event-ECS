using Event_ECS_WPF.Logger;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Event_ECS_WPF.Misc
{
    public class DataGridBehaviour
    {
        private static readonly Dictionary<DataGrid, Capture> Associations =
               new Dictionary<DataGrid, Capture>();

        public static bool GetScrollOnNewItem(DependencyObject obj)
        {
            return (bool)obj.GetValue(ScrollOnNewItemProperty);
        }

        public static void SetScrollOnNewItem(DependencyObject obj, bool value)
        {
            obj.SetValue(ScrollOnNewItemProperty, value);
        }

        public static readonly DependencyProperty ScrollOnNewItemProperty =
            DependencyProperty.RegisterAttached(
                "ScrollOnNewItem",
                typeof(bool),
                typeof(DataGridBehaviour),
                new UIPropertyMetadata(false, OnScrollOnNewItemChanged));

        public static void OnScrollOnNewItemChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            if (!(d is DataGrid dataGrid)) return;
            bool oldValue = (bool)e.OldValue, newValue = (bool)e.NewValue;
            if (newValue == oldValue) return;
            if (newValue)
            {
                dataGrid.Loaded += DataGrid_Loaded;
                dataGrid.Unloaded += DataGrid_Unloaded;
                var itemsSourcePropertyDescriptor = TypeDescriptor.GetProperties(dataGrid)["ItemsSource"];
                itemsSourcePropertyDescriptor.AddValueChanged(dataGrid, DataGrid_ItemsSourceChanged);
            }
            else
            {
                dataGrid.Loaded -= DataGrid_Loaded;
                dataGrid.Unloaded -= DataGrid_Unloaded;
                if (Associations.ContainsKey(dataGrid))
                    Associations[dataGrid].Dispose();
                var itemsSourcePropertyDescriptor = TypeDescriptor.GetProperties(dataGrid)["ItemsSource"];
                itemsSourcePropertyDescriptor.RemoveValueChanged(dataGrid, DataGrid_ItemsSourceChanged);
            }
        }

        private static void DataGrid_ItemsSourceChanged(object sender, EventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            if (Associations.ContainsKey(dataGrid))
                Associations[dataGrid].Dispose();
            Associations[dataGrid] = new Capture(dataGrid);
        }

        private static void DataGrid_Unloaded(object sender, RoutedEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            if (Associations.ContainsKey(dataGrid))
                Associations[dataGrid].Dispose();
            dataGrid.Unloaded -= DataGrid_Unloaded;
        }

        private static void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var dataGrid = (DataGrid)sender;
            if (!(dataGrid.Items is INotifyCollectionChanged incc)) return;
            dataGrid.Loaded -= DataGrid_Loaded;
            Associations[dataGrid] = new Capture(dataGrid);
        }

        private class Capture : IDisposable
        {
            private readonly DataGrid dataGrid;
            private readonly INotifyCollectionChanged incc;

            public Capture(DataGrid dataGrid)
            {
                this.dataGrid = dataGrid;
                incc = dataGrid.ItemsSource as INotifyCollectionChanged;
                if (incc != null)
                {
                    incc.CollectionChanged += Incc_CollectionChanged;
                }
            }

            private void Incc_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    try
                    {
                        dataGrid.ScrollIntoView(e.NewItems[0]);
                        dataGrid.SelectedItem = e.NewItems[0];
                    }
                    catch(Exception ex)
                    {
                        LogManager.Instance.Add(ex);
                    }
                }
            }

            public void Dispose()
            {
                if (incc != null)
                    incc.CollectionChanged -= Incc_CollectionChanged;
            }
        }
    }
}