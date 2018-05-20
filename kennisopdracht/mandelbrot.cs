using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kennisopdracht
{
    public class mandelbrot : Form
    {
        double midden_X;
        double midden_Y;
        double schaal;
        Bitmap Bm;
        TextBox textbox_midden_X;
        TextBox textbox_midden_Y;
        TextBox textbox_schaal;
      

        //constructor ofwel methode die object in elkaar zet
        public mandelbrot()
        {
            this.Size = new Size(1000, 1000);
            this.Location = new Point(0, 0);
            this.Text = "Mandelbrot";

            #region label declaraties

            Label label_midden_X = new Label();
            label_midden_X.Text = "Midden x:";
            label_midden_X.Location = new Point(65, 20);
            label_midden_X.Size = new Size(100, 40);
            label_midden_X.Font = new Font("Arial", 14);

            Label label_midden_Y = new Label();
            label_midden_Y.Text = "Midden y:";
            label_midden_Y.Location = new Point(350, 20);
            label_midden_Y.Size = new Size(100, 40);
            label_midden_Y.Font = new Font("Arial", 14);

            Label label_schaal = new Label();
            label_schaal.Text = "Schaal:";
            label_schaal.Location = new Point(65, 80);
            label_schaal.Size = new Size(100, 40);
            label_schaal.Font = new Font("Arial", 14);

            Label label_max = new Label();
            label_max.Text = "Max:";
            label_max.Location = new Point(350, 80);
            label_max.Size = new Size(100, 40);
            label_max.Font = new Font("Arial", 14);
            #endregion

            #region textbox declaraties
            textbox_midden_X = new TextBox();
            textbox_midden_X.Text = "0";
            textbox_midden_X.Location = new Point(175, 20);
            textbox_midden_X.Size = new Size(60, 40);

            textbox_midden_Y = new TextBox();
            textbox_midden_Y.Text = "0";
            textbox_midden_Y.Location = new Point(460, 20);
            textbox_midden_Y.Size = new Size(60, 40);

            textbox_schaal = new TextBox();
            textbox_schaal.Text = "0.01";
            textbox_schaal.Location = new Point(175, 80);
            textbox_schaal.Size = new Size(60, 40);

            TextBox textbox_max = new TextBox();
            textbox_max.Text = "100";
            textbox_max.Location = new Point(460, 80);
            textbox_max.Size = new Size(60, 40);
            #endregion

            #region Ok knop declaratie
            Button Ok_knop = new Button();
            Ok_knop.Text = "Ok";
            Ok_knop.Location = new Point(600, 20);
            Ok_knop.Size = new Size(80, 30);
            Ok_knop.Click += this.knop_OK_Click;
            #endregion

            #region control declaraties

            this.Controls.Add(label_midden_X);
            this.Controls.Add(label_midden_Y);
            this.Controls.Add(label_schaal);
            this.Controls.Add(label_max);

            this.Controls.Add(textbox_midden_X);
            this.Controls.Add(textbox_midden_Y);
            this.Controls.Add(textbox_schaal);
            this.Controls.Add(textbox_max);

            this.Controls.Add(Ok_knop);
            #endregion

            Bm = new Bitmap(400, 400);

            this.Paint += this.teken_bitmap;
        }

        private void knop_OK_Click(object sender, EventArgs e)
        {
            //update de x en y waarden voor de mandelbrot op het moment dat je Ok klikt
            midden_X = double.Parse(textbox_midden_X.Text);
            midden_Y = double.Parse(textbox_midden_Y.Text);
            schaal = double.Parse(textbox_schaal.Text);
            

            // bereken een nieuwe bitmap.
            this.bereken_bitmap();

            // teken alles opnieuw.
            this.Invalidate();
        }

        // mandelbrot berekenen niet in paint event doen, maar het tekenen wel. 
        // paint event alleen gebruiken om te tekenen.


        private void teken_bitmap(Object obj, PaintEventArgs pea)
        {
            Point bitmaplocatie = new Point(200, 200);
            pea.Graphics.DrawImage(Bm, bitmaplocatie);
        }
 
        private void bereken_bitmap()
        {
            for (int x = 0; x < Bm.Width; x++)
            {
                for (int y = 0; y < Bm.Height; y++)
                {
                    double eenX;
                    eenX = BepaalGrafiekX((double)x, schaal);

                    double eenY;
                    eenY = BepaalGrafiekY((double)y, schaal);

                    // roep methode aan om mandelgetal te berekenen
                    int mandelgetal;
                    mandelgetal = mandelgetallen(eenX, eenY);

                    if (mandelgetal % 2 == 0)
                    {
                        Bm.SetPixel(x, y, Color.White);
                    }
                    else
                    {
                        Bm.SetPixel(x, y, Color.Black);
                    }
                }
            }
        }

        private int mandelgetallen(double x, double y)
        {
            double a = 0;
            double b = 0;
            double oudeA = 0;
            double oudeB = 0;
            bool getalgevonden = false;
            int mandelgetal = 0;
            int maximumcounter = 0;

            while (!getalgevonden && maximumcounter < 100)
            {
                a = oudeA * oudeA - oudeB * oudeB + x;
                b = 2 * oudeA * oudeB + y;
                oudeA = a;
                oudeB = b;
                mandelgetal += 1;
                maximumcounter += 1;

                // Als berekenDeAfstand een waarde geeft groter dan 2, stop deze methode.
                double afstand = berekenDeAfstand(a, b);
                if (afstand >= 2)
                {
                    getalgevonden = true;
                }
            }

            if (getalgevonden)
                return mandelgetal;
            else
                return 1;

        }
       // bereken de afstand tussen a,b en x,y
        private double berekenDeAfstand(double a, double b)
        {
            double afstand;
            afstand = Math.Sqrt(Math.Pow(a - 0, 2) + Math.Pow(b - 0, 2));
            return afstand;
        }

       // Methodes met formules om coordinaten van het assenstelsel te bepalen. Helperfuncties van methode om bitmap te tekenen. 
        private double BepaalGrafiekX(double pixel_x, double schaal)
        {
            double wiskundige_x = ((pixel_x - 200) * schaal);

            wiskundige_x += midden_X;

             return wiskundige_x;

        }

        private double BepaalGrafiekY(double pixel_y, double schaal)
        {
            double wiskundige_y = ((200 - pixel_y) * schaal);

            wiskundige_y += midden_Y;

            return wiskundige_y;
         }

    }

}