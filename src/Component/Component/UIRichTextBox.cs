using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Component
{
    public class UIRichTextBox
    {
        static public void AppendText(RichTextBox sender, String text, Color color)
        {
            sender.SelectionStart = sender.TextLength;
            sender.SelectionLength = 0;

            sender.SelectionColor = color;
            sender.AppendText(text);
            sender.SelectionColor = sender.ForeColor;
        }

        static public String CopyWithColor(RichTextBox sender)
        {
            return sender.Rtf;
        }

        static public void ScrollToEnd(RichTextBox sender)
        {
            sender.SelectionStart = sender.Text.Length;
            sender.ScrollToCaret();
        }
    }
}
