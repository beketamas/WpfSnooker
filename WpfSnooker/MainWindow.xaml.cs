using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.IO;
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

namespace WpfSnooker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Versenyzo> listaversenyzok = File.ReadAllLines("snooker.txt").Skip(1).Select(x => new Versenyzo(x)).ToList();
        public MainWindow()
        {
            InitializeComponent();
            cbOrszagok.ItemsSource = listaversenyzok.Select(x => x.Orszag).Distinct().OrderBy(x => x);
            dgTablazat.ItemsSource = listaversenyzok;
            cbOrszagok.SelectedIndex = 0;
            btnF3.Click += (s, e) => MessageBox.Show($"3. feladata: A világranglistán {listaversenyzok.Count} versenyző szerepel");
            btnF4.Click += (s, e) => MessageBox.Show($"4. feladata: A versenyzők átlagosan {listaversenyzok.Average(x => x.Nyeremeny):f2} fontot kerestek");
            btnF5.Click += (s, e) =>
            {
                var legjobbanKereso = listaversenyzok.Where(x => x.Orszag == cbOrszagok.SelectedItem.ToString()).MaxBy(x => x.Nyeremeny);
                lblHelyezes.Content = legjobbanKereso.Helyezes;
                lblNev.Content = legjobbanKereso.Nev;
                lblOrszag.Content = legjobbanKereso.Orszag;
                lblNyeremeny.Content = $"{legjobbanKereso.Nyeremeny * double.Parse(txtArfolyam.Text):n0} Ft";
            };
            btnF6.Click += (s, e) => MessageBox.Show($"6. feladat: A versenyzők között {(listaversenyzok.Exists(x => x.Orszag == txtVanIlyenOrszag.Text) ? "van" : "nincs" )}ilyen versenyző");

            btnF7.Click += (s, e) =>
            {
                lbStatisztika.Items.Clear();
                lbStatisztika.Items.Add($"7. feladat: Statisztika\nMinnimum: {lblminFo.Content} fő");
                listaversenyzok.GroupBy(x => x.Orszag).Where(x => x.Count() >= int.Parse(lblminFo.Content.ToString())).ToList().ForEach(x => lbStatisztika.Items.Add($"{x.Key} - {x.Count()} fő"));
            };

            lbStatisztika.MouseDoubleClick += (s, e) => dgTablazat.ItemsSource = listaversenyzok.Where(x => x.Orszag.Contains(lbStatisztika.SelectedItem.ToString().Split(" ")[0]));
        }

   
    }
}
