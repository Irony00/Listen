using System.Drawing;
using System.Windows.Forms;

namespace Listen
{
    public class Dialog : Form
    {
        public Dialog(string track)
        {
            Controls.Add(new Button {Width = 150, Height = 70, ForeColor = Color.Black, Text = track});
        }
    }
}