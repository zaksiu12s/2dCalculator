using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

class PolygonObject (System.Windows.Shapes.Polygon polygonElement)
{
    public float trojkat = 0;
    public double bok;
    private int rotation = 0;

    public SolidColorBrush fillColor = new BrushConverter().ConvertFromString("Black") as SolidColorBrush;
    public double opacity = 1;

    private readonly System.Windows.Shapes.Polygon polygonElement = polygonElement;

    System.Windows.Point pt1 = new(0, 0);
    System.Windows.Point pt2 = new(0, 0);
    System.Windows.Point pt3 = new(0, 0);
    System.Windows.Point pt4 = new(0, 0);

    public void RotatePolygon()
    {
        if (this.polygonElement.Width == 0 || this.polygonElement.Height == 0)
        {
            return;
        }
        this.rotation += 10;
        this.polygonElement.RenderTransformOrigin = new System.Windows.Point((this.bok / 2) / this.bok, (this.bok / 2) / this.bok);
        this.SetPolyRotation();
    }

    public void SetPolyRotation()
    {
        RotateTransform rotateTransform = new(this.rotation);
        this.polygonElement.RenderTransform = rotateTransform;
    }

    public void ClearPolygonData()
    {
        this.polygonElement.Width = 0;
        this.polygonElement.Height = 0;
        this.trojkat = 0;
        this.rotation = 0;
        this.opacity = 1;
        this.rotation = 0;
        this.fillColor = new BrushConverter().ConvertFromString("Black") as SolidColorBrush;

        this.SetPolyRotation();
    }

    public string CalculatePole()
    {
        if (this.trojkat == 0)
        {
            return (this.bok * this.bok).ToString();
        }
        return ((this.bok + this.bok - Math.Abs(this.trojkat) * 2) * this.bok / 2).ToString();
    }

    public string CalculateObwod()
    {
        if (this.trojkat == 0)
        {
            return (4 * this.bok).ToString();
        }
        double przekatna = Math.Sqrt(this.bok * this.bok + this.trojkat * this.trojkat);
        double obwod = 2 * przekatna + this.bok + this.bok - 2 * Math.Abs(this.trojkat);
       return obwod.ToString();
    }

    public void DrawPolygon()
    {
        this.polygonElement.Width = this.bok;
        this.polygonElement.Height = this.bok;
        this.polygonElement.Opacity = this.opacity;
        polygonElement.Fill = this.fillColor;

        float trojkatBottom, trojkatUp;

        if (this.trojkat < -this.bok / 2)
        {
            this.trojkat = (float)-this.bok / 2;
        }

        if (this.trojkat > this.bok / 2)
        {
            this.trojkat = (float)this.bok / 2;
        }

        if (this.trojkat >= 0)
        {
            trojkatUp = this.trojkat;
            trojkatBottom = 0;
        }
        else
        {
            trojkatBottom = this.trojkat;
            trojkatUp = 0;
        }

        PointCollection points = [];

        pt1.X = trojkatUp;

        pt2.X = this.bok - trojkatUp;

        pt3.X = this.bok + trojkatBottom;
        pt3.Y = this.bok;

        pt4.X = 0 - trojkatBottom;
        pt4.Y = this.bok;


        points.Add(pt1);
        points.Add(pt2);
        points.Add(pt3);
        points.Add(pt4);


        this.polygonElement.Points = points;
    }
}


namespace Cwiczenie_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PolygonObject polygonObject;

        public MainWindow()
        {
            InitializeComponent();
            polygonObject = new PolygonObject(polygon1);
        }


        private void txtBok_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(txtBok.Text, out polygonObject.bok) && polygonObject.bok >= 0)
            {
                lblKomunikat.Content = String.Empty;
                txtPole.Text = polygonObject.CalculatePole();
                txtObwod.Text = polygonObject.CalculateObwod();
            }
            else
            {
                txtPole.Text = String.Empty;
                txtObwod.Text = String.Empty;
                lblKomunikat.Foreground = new BrushConverter().ConvertFromString("Red") as SolidColorBrush;
                lblKomunikat.Content = "Wpisz liczbę dodatnią";
            }
        }

        private void btnWyczysc_Click(object sender, RoutedEventArgs e)
        {
            txtBok.Text = String.Empty;
            txtPole.Text = String.Empty;
            txtObwod.Text = String.Empty;
            lblKomunikat.Foreground = new BrushConverter().ConvertFromString("Gray") as SolidColorBrush;
            lblKomunikat.Content = "Wpisz wymiar boku";

            polygonObject.ClearPolygonData();
        }

        private void btnRysuj_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(txtBok.Text, out polygonObject.bok) && polygonObject.bok <= 380 && polygonObject.bok >= 0)
            {
                polygonObject.fillColor = new BrushConverter().ConvertFromString(cmbKolory.Text) as SolidColorBrush;
                polygonObject.opacity = (cbPrzezroczysty.IsChecked.Value ? 0.5 : 1);
                txtPole.Text = polygonObject.CalculatePole();
                txtObwod.Text = polygonObject.CalculateObwod();

                polygonObject.DrawPolygon();
            }
            else
            {
                lblKomunikat.Content = "Brak danych, liczba na minusie lub zbyt duży bok.";
            }
        }

        private void radioUkryj_Checked(object sender, RoutedEventArgs e)
        {
            polygon1.Opacity = 0;
        }

        private void radioPokaz_Checked(object sender, RoutedEventArgs e)
        {
            polygon1.Opacity = (cbPrzezroczysty.IsChecked.Value ? 0.5 : 1);
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            Double.TryParse(txtBok.Text, out polygonObject.bok);
            polygonObject.bok = polygonObject.bok < 380 ? polygonObject.bok + 1 : 380;
            txtBok.Text = polygonObject.bok.ToString();
            txtPole.Text = polygonObject.CalculatePole();
            txtObwod.Text = polygonObject.CalculateObwod();

            polygonObject.DrawPolygon();
        }

        private void substract_Click(object sender, RoutedEventArgs e)
        {
            Double.TryParse(txtBok.Text, out polygonObject.bok);
            polygonObject.bok = polygonObject.bok > 0 ? polygonObject.bok - 1 : 0;
            txtBok.Text = polygonObject.bok.ToString();
            txtPole.Text = polygonObject.CalculatePole();
            txtObwod.Text = polygonObject.CalculateObwod();

            polygonObject.DrawPolygon();
        }

        private void zatancz_Click(object sender, RoutedEventArgs e)
        {
            polygonObject.RotatePolygon();
        }

        private void trojkatoj_Click(object sender, RoutedEventArgs e)
        {
            polygonObject.trojkat++;

            txtPole.Text = polygonObject.CalculatePole();
            txtObwod.Text = polygonObject.CalculateObwod();
            polygonObject.DrawPolygon();
        }

        private void odtrojkatoj_Click(object sender, RoutedEventArgs e)
        {
            polygonObject.trojkat--;

            txtPole.Text = polygonObject.CalculatePole();
            txtObwod.Text = polygonObject.CalculateObwod();
            polygonObject.DrawPolygon();
        }
    }
}

