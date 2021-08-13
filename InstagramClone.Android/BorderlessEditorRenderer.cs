using System;
using Xamarin.Forms;
using XamarinFormsEditor.Droid.Helpers;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;
using InstagramClone.Renderers;
using Android.Content;

[assembly: ExportRenderer(typeof(BorderlessEditor), typeof(BorderlessEditorRenderer))]
namespace XamarinFormsEditor.Droid.Helpers
{
    public class BorderlessEditorRenderer : EditorRenderer
    {
        public BorderlessEditorRenderer(Context context) : base(context) { AutoPackage = false; }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                this.Control.SetBackground(null);
            }
        }
    }
}