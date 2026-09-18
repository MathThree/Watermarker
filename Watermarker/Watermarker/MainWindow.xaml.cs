using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Watermarker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ImageFile imageFile;
        public MainWindow()
        {
            InitializeComponent();
            imageFile = new ImageFile();
        }

        private void Display_Watermarked_Image(Image<Rgba32> watermarkedImage)
        {
            using (var memoryStream = new System.IO.MemoryStream())
            {
                watermarkedImage.SaveAsPng(memoryStream);

                memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
                BitmapImage bitmapImage = new BitmapImage();

                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                WatermarkedImage.Source = bitmapImage;
            }
        }

        private void Button_Click_Watermark1(object sender, RoutedEventArgs e)
        {
            if (imageFile.FilePath != null && imageFile.FilePath != string.Empty)
            {
                string watermarkText = WatermarkTextBox.Text;
                using (Image<Rgba32> watermarkedImage = imageFile.GetWatermarkedImageType1(watermarkText, (float)(AlphaSlider.Value / 100)))
                {
                    Display_Watermarked_Image(watermarkedImage);
                }
            }
            else
            {
                MessageBox.Show("Please select an image file first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_Watermark2(object sender, RoutedEventArgs e)
        {
            if (imageFile.FilePath != null && imageFile.FilePath != string.Empty)
            {
                string watermarkText = WatermarkTextBox.Text;
                using (Image<Rgba32> watermarkedImage = imageFile.GetWatermarkedImageType2(watermarkText, (float)(AlphaSlider.Value / 100)))
                {
                    Display_Watermarked_Image(watermarkedImage);
                }
            }
            else
            {
                MessageBox.Show("Please select an image file first.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click_Open_File(object sender, RoutedEventArgs e)
        {
            // Open a file explorer dialog to select an image file
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                imageFile.FilePath = filePath;
                Image<Rgba32> image = SixLabors.ImageSharp.Image.Load<Rgba32>(filePath);
                // Display the image in the target image control
                TargetImage.Source = new BitmapImage(new Uri(filePath));
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void AlphaSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (AlphaValueTextBlock == null) return;
            AlphaValueTextBlock.Text = ((float)(AlphaSlider.Value / 100)).ToString("0.00");
        }
    }
}