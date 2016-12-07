using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Component
{
    public class UIRichTextBoxCaretInfo
    {
        public int Line;
        public int Column;
    }
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

        static public void RemoveLastChar(RichTextBox sender)
        {
            sender.Text = sender.Text.Substring(0, sender.Text.Length - 1);
        }

        static public UIRichTextBoxCaretInfo GetCaretPos(RichTextBox sender)
        {
            int line = sender.GetLineFromCharIndex(sender.SelectionStart);
            int column = sender.SelectionStart - sender.GetFirstCharIndexFromLine(line);
            UIRichTextBoxCaretInfo info = new UIRichTextBoxCaretInfo();
            info.Line = line;
            info.Column = column;
            //Debug.WriteLine(String.Format("line:{0} col:{1}", line, column));
            return info;
        }
        static public void PushBackSpaceToHead(RichTextBox sender)
        {
            // Move carat to end
            sender.Select(sender.Text.Length, 0);
            sender.ScrollToCaret();
            // Get Postion
            if (UIRichTextBox.GetCaretPos(sender).Column == 0)
                return;
            // Remove
            UIRichTextBox.RemoveLastChar(sender);
        }
    }
}
