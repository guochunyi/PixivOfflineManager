using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace PixivOfflineManager
{
	public partial class MainWindow : Window
	{
		public ObservableCollection<PicInfo> IlustList
		{
			get { return (ObservableCollection<PicInfo>)GetValue(IlustListProperty); }
			set { SetValue(IlustListProperty, value); }
		}

		// Using a DependencyProperty as the backing store for IlustList.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty IlustListProperty =
			DependencyProperty.Register("IlustList", typeof(ObservableCollection<PicInfo>), typeof(MainWindow), new PropertyMetadata(null));
		public MainWindow()
		{
			//PicInfo info = new PicInfo("34655446");
			IlustList = new ObservableCollection<PicInfo>();
			InitializeComponent();
		}

		private void AddFolderClick(object sender, RoutedEventArgs e)
		{
			System.Windows.Forms.FolderBrowserDialog fdb = new System.Windows.Forms.FolderBrowserDialog();
			fdb.ShowDialog();
			if (fdb.SelectedPath != string.Empty)
			{
				FileManager file = new FileManager(fdb.SelectedPath);
				IlustList = new ObservableCollection<PicInfo>(file.Files);
			}
		}
	}
}
