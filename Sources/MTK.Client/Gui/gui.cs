using System;
using System.Drawing;

namespace mtkclient
{
    public class gui
    {
        public static void logs(string text, bool newline)
        {
            if (Main.SharedUI.log.InvokeRequired)
            {
                if (newline)
                {
                    Main.SharedUI.log.Invoke(
                        (Action)(() => Main.SharedUI.log.AppendText(text + "\r\n"))
                    );
                }
                else
                {
                    Main.SharedUI.log.Invoke((Action)(() => Main.SharedUI.log.AppendText(text)));
                }
            }
            else
            {
                if (newline)
                {
                    Main.SharedUI.log.AppendText(text + "\r\n");
                }
                else
                {
                    Main.SharedUI.log.AppendText(text);
                }
            }
        }

        public static void Richlog(
            string msg,
            Color colour,
            bool isBold = false,
            bool NextLine = false
        )
        {
            if (Main.SharedUI.log.InvokeRequired)
            {
                Main.SharedUI.log.Invoke(
                    new Action(() =>
                    {
                        Main.SharedUI.log.SelectionStart = Main.SharedUI.log.Text.Length;
                        Color selectionColor = Main.SharedUI.log.SelectionColor;
                        Main.SharedUI.log.SelectionColor = colour;
                        if (isBold)
                        {
                            Main.SharedUI.log.SelectionFont = new Font(
                                Main.SharedUI.log.Font,
                                FontStyle.Bold
                            );
                        }
                        else
                        {
                            Main.SharedUI.log.SelectionFont = new Font(
                                Main.SharedUI.log.Font,
                                FontStyle.Regular
                            );
                        }
                        Main.SharedUI.log.AppendText(msg);
                        Main.SharedUI.log.SelectionColor = selectionColor;
                        if (NextLine)
                        {
                            if (Main.SharedUI.log.TextLength > 0)
                            {
                                Main.SharedUI.log.AppendText("\r\n");
                            }
                        }
                    })
                );
            }
            else
            {
                Main.SharedUI.log.SelectionStart = Main.SharedUI.log.Text.Length;
                Color selectionColor = Main.SharedUI.log.SelectionColor;
                Main.SharedUI.log.SelectionColor = colour;
                if (isBold)
                {
                    Main.SharedUI.log.SelectionFont = new Font(
                        Main.SharedUI.log.Font,
                        FontStyle.Bold
                    );
                }
                else
                {
                    Main.SharedUI.log.SelectionFont = new Font(
                        Main.SharedUI.log.Font,
                        FontStyle.Regular
                    );
                }
                Main.SharedUI.log.AppendText(msg);
                Main.SharedUI.log.SelectionColor = selectionColor;
                if (NextLine)
                {
                    if (Main.SharedUI.log.TextLength > 0)
                    {
                        Main.SharedUI.log.AppendText("\r\n");
                    }
                }
            }
        }
    }
}
