using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace InstagramClone.Renderers
{
    public class BorderlessEditor:Editor
    {
        public BorderlessEditor()
        {
            TextChanged += OnTextChanged;
        }
        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            InvalidateMeasure();
        }
    }
}
