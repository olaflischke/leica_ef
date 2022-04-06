using ChinookDal;
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

namespace ChinookExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ChinookContext context = new ChinookContext();

        public MainWindow()
        {
            InitializeComponent();

            var qGenres = context.Genres;

            foreach (var genre in qGenres)
            {
                TreeViewItem genreItem = new TreeViewItem() { Header = genre.Name, Tag = genre.GenreId };
                genreItem.Items.Add(new TreeViewItem());
                genreItem.Expanded += this.GenreItem_Expanded;
                trvArtists.Items.Add(genreItem);
            }
        }

        private void GenreItem_Expanded(object sender, RoutedEventArgs e)
        {
            if (sender is TreeViewItem genreItem)
            {
                genreItem.Items.Clear();

                int genreId = Convert.ToInt32(genreItem.Tag);
                var qArtists = context.Tracks
                                        .Where(tr => tr.GenreId == genreId)
                                        .Select(tr => tr.Album.Artist)
                                        .Distinct();

                foreach (Artist artist in qArtists)
                {
                    TreeViewItem artistItem = new TreeViewItem() { Header = artist.Name, Tag=artist.ArtistId };
                    genreItem.Items.Add(artistItem);
                }
            }
        }
    }
}
