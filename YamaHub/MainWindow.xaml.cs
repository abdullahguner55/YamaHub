using System.IO;
using System.Windows;
using Microsoft.Win32;
using System.Text.Json;
using System.Collections.Generic;

namespace YamaHub
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            YamalarıYukle();
        }
        List<Yama> yamalar;

        private void YamalarıYukle()
        {
            string json = File.ReadAllText("yamalar.json");
            yamalar = JsonSerializer.Deserialize<List<Yama>>(json);

            MessageBox.Show(yamalar.Count + " yama yüklendi");
        }

        Dictionary<string, string> steamOyunlari = new Dictionary<string, string>()
{
    { "FarCry", "Far Cry" },
    { "GTA5", "Grand Theft Auto V" }
};
        Dictionary<string, string> epicOyunlari = new Dictionary<string, string>()
{
    { "GTA5", "GTAV" },
    { "RDR2", "RedDeadRedemption2" }
};


        private void YamayiKur_Click(object sender, RoutedEventArgs e)
        {
            var buton = sender as System.Windows.Controls.Button;
            string oyunAdi = buton.Tag.ToString();

            // 1️⃣ Yama seç
            OpenFileDialog yamaSec = new OpenFileDialog();
            yamaSec.Title = oyunAdi + " yamasını seç";

            if (yamaSec.ShowDialog() != true)
                return;

            string hedefKlasor = "";

            // 2️⃣ STEAM KONTROL
            if (steamOyunlari.ContainsKey(oyunAdi))
            {
                string steamYolu = @"C:\Program Files (x86)\Steam\steamapps\common";
                string steamOyunKlasoru = Path.Combine(steamYolu, steamOyunlari[oyunAdi]);

                if (Directory.Exists(steamOyunKlasoru))
                    hedefKlasor = steamOyunKlasoru;
            }

            // 3️⃣ EPIC KONTROL
            if (hedefKlasor == "")
            {
                if (epicOyunlari.ContainsKey(oyunAdi))
                {
                    string epicYolu = @"C:\Program Files\Epic Games";
                    string epicOyunKlasoru = Path.Combine(epicYolu, epicOyunlari[oyunAdi]);

                    if (Directory.Exists(epicOyunKlasoru))
                        hedefKlasor = epicOyunKlasoru;
                }
            }

            // 4️⃣ HALA YOKSA EXE SOR
            if (hedefKlasor == "")
            {
                OpenFileDialog oyunSec = new OpenFileDialog();
                oyunSec.Title = "Oyunun exe dosyasını seç";
                oyunSec.Filter = "Uygulama (*.exe)|*.exe";

                if (oyunSec.ShowDialog() != true)
                    return;

                hedefKlasor = Path.GetDirectoryName(oyunSec.FileName);
            }

            // 5️⃣ KOPYALA
            string hedefYol = Path.Combine(
                hedefKlasor,
                Path.GetFileName(yamaSec.FileName)
            );

            File.Copy(yamaSec.FileName, hedefYol, true);

            MessageBox.Show(oyunAdi + " yaması başarıyla kuruldu 🎉");
        }

    }
}
