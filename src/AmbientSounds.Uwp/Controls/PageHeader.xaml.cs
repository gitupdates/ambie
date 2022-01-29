using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AmbientSounds.Controls
{
    public sealed partial class PageHeader : UserControl
    {
        public event EventHandler InfoTextClosed;

        public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register(
            nameof(HeaderText),
            typeof(string),
            typeof(PageHeader),
            new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty GlyphCodeProperty = DependencyProperty.Register(
            nameof(GlyphCode),
            typeof(string),
            typeof(PageHeader),
            new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty InfoTextVisibleProperty = DependencyProperty.Register(
            nameof(InfoTextVisible),
            typeof(bool),
            typeof(PageHeader),
            new PropertyMetadata(false));

        public static readonly DependencyProperty InfoTextProperty = DependencyProperty.Register(
            nameof(InfoText),
            typeof(string),
            typeof(PageHeader),
            new PropertyMetadata(string.Empty));

        public PageHeader()
        {
            this.InitializeComponent();
        }

        public string HeaderText
        {
            get => (string)GetValue(HeaderTextProperty);
            set => SetValue(HeaderTextProperty, value);
        }

        public string GlyphCode
        {
            get => (string)GetValue(GlyphCodeProperty);
            set => SetValue(GlyphCodeProperty, value);
        }

        public bool InfoTextVisible
        {
            get => (bool)GetValue(InfoTextVisibleProperty);
            set => SetValue(InfoTextVisibleProperty, value);
        }

        public string InfoText
        {
            get => (string)GetValue(InfoTextProperty);
            set => SetValue(InfoTextProperty, value);
        }

        private void InfoBar_Closed(Microsoft.UI.Xaml.Controls.InfoBar sender, Microsoft.UI.Xaml.Controls.InfoBarClosedEventArgs args)
        {
            InfoTextClosed?.Invoke(this, EventArgs.Empty);
        }
    }
}
