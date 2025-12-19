using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopView.Behaviors
{
    public class ZeroPlaceholderBehavior : Behavior<Entry>
    {       
        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.BindingContextChanged += OnBindingContextChanged;
        }

        private void OnBindingContextChanged(object? sender, EventArgs e)
        {
            if (sender is Entry entry)
            {
                entry.TextChanged -= Entry_TextChanged;
                entry.TextChanged += Entry_TextChanged;
            }
        }

        private void Entry_TextChanged(object? sender, TextChangedEventArgs e)
        {
            if (sender is Entry entry)
            {
                if (entry.Text == "0")
                    entry.Text = string.Empty;
            }
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.BindingContextChanged -= OnBindingContextChanged;
        }
    }
}