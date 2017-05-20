using System;
using System.Windows;
using System.Threading;

using Neptune.Models;
using MahApps.Metro.Controls;

namespace Neptune.Views
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        // Twitter クラスインスタンス
        private Twitter _twitter;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // IcePlayer のウィンドウ移動(ドラッグ) を可能にする
            this.MouseLeftButtonDown += (sender, e) => this.DragMove();

            _twitter = new Twitter();

            _twitter.Initialize();
            _twitter.Tweet();
        }
    }
}
