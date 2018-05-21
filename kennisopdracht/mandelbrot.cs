/* Dit programma tekent een mandelbrotfiguur. Aan de hand van twee wiskundige formules, die herhaaldelijk worden uitgevoerd, worden getallen gegenereerd, 
 * zogeheten mandelgetallen. Aan de hand van de mandelgetallen kan er een figuur getekend worden op een assenstelsel. In dit programma wordt het assenstelsel 
 * getekend op een bitmap. De waardes in de grafische userinterface kunnen aangepast worden, zodat er verder op het figuur ingezoomd kan worden. 
 * Deze opdracht is afkomstig van de cursus imperatief programmeren van de Universiteit Utrecht. 
 * Voor meer informatie zie: http://www.cs.uu.nl/docs/vakken/imp/ -> Practicum -> Opdracht 1 "Mandelbrot". 
 * */

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
        int max;
        Bitmap Bm;
        TextBox textbox_midden_X;
        TextBox textbox_midden_Y;
        TextBox textbox_schaal;
        TextBox textbox_max;
      

        //constructor, methode die object in elkaar zet
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

            textbox_max = new TextBox();
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

            Bm = new Bitmap(600, 600);
            schaal = 0.01;
            max = int.Parse(textbox_max.Text);

            this.Paint += this.teken_bitmap;
            this.bereken_bitmap();
        }

        private void knop_OK_Click(object sender, EventArgs e)
        {
            //update de x, y waarden, de schaal en max-waarde voor het mandelbrotfiguur op het moment dat je Ok klikt
            string middenX_text = textbox_midden_X.Text;
            middenX_text = middenX_text.Replace('.', ',');
            midden_X = Convert.ToDouble(middenX_text);

            string middenY_text = textbox_midden_Y.Text;
            middenY_text = middenY_text.Replace('.', ',');
            midden_Y = Convert.ToDouble(middenY_text);
           
            string schaal_text = textbox_schaal.Text;
            schaal_text = schaal_text.Replace('.', ',');
            schaal = Convert.ToDouble(schaal_text);

            max = int.Parse(textbox_max.Text);

            // bereken een nieuwe bitmap.
            this.bereken_bitmap();

            // teken alles opnieuw.
            this.Invalidate();
        }

        

        // methode om dit bitmap te tekenen.
        private void teken_bitmap(Object obj, PaintEventArgs pea)
        {
            Point bitmaplocatie = new Point(200, 200);
            pea.Graphics.DrawImage(Bm, bitmaplocatie);
        }

        // methode om het mandelbrotfiguur te tekenen. Roept methodes aan om mandelgetallen te berekenen. 
        private void bereken_bitmap()
        {
            for (int x = 0; x < Bm.Width; x++)
            {
                for (int y = 0; y < Bm.Height; y++)
                {
                    // roep methodes aan om x- en y-coördinaten te berekenen
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
        // methode om de mandelgetallen te berekenen.
        private int mandelgetallen(double x, double y)
        {
            double a = 0;
            double b = 0;
            double oudeA = 0;
            double oudeB = 0;
            bool getalgevonden = false;
            int mandelgetal = 0;

            while (!getalgevonden && mandelgetal < max)
            {
                a = oudeA * oudeA - oudeB * oudeB + x;
                b = 2 * oudeA * oudeB + y;
                oudeA = a;
                oudeB = b;
                mandelgetal += 1;

                // Roep methode aan om afstand tussen ccoördinaten te berekenen. Als berekenDeAfstand een waarde geeft groter dan 2, stop deze methode.
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
       // bereken de afstand tussen a,b en x,y. Helperfunctie van methode om mandelgetallen te berekenen. 
        private double berekenDeAfstand(double a, double b)
        {
            double afstand;
            afstand = Math.Sqrt(Math.Pow(a - 0, 2) + Math.Pow(b - 0, 2));
            return afstand;
        }

        // Methode om x-ccoördinaten van het assenstelsel te bepalen. Helperfunctie van methode om bitmap te tekenen. 
        private double BepaalGrafiekX(double pixel_x, double schaal)
        {
            double assenstelsel_X = ((pixel_x - (Bm.Width / 2)) * schaal);

            assenstelsel_X += midden_X;

             return assenstelsel_X;

        }

        // Methode om y-coördinaten van het assenstelsel te bepalen. Tweede helperfunctie van methode om bitmap te tekeken. 
        private double BepaalGrafiekY(double pixel_y, double schaal)
        {
            double assenstelsel_Y = (((Bm.Height/2) - pixel_y) * schaal);

            assenstelsel_Y += midden_Y;

            return assenstelsel_Y;
         }

    }

}