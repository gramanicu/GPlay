using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Drawing.Text;

namespace GPlay_Client
{
    public class uiOperations
    {
        public FlowLayoutPanel layoutPanel;

        public uiOperations()
        {
        }
            
        public void addGameTab(Image icon, string title, string time, string status, string buttonText)
        {
            Point _1 = new Point(3, 5);
            Point _2 = new Point(89, 26);
            Point _3 = new Point(331, 35);
            Point _4 = new Point(487, 28);
            Point _5 = new Point(653, 28);
            Point _6 = new Point(961, 38);

            Panel p = new Panel();

            p.Name = "panel" + title;
            p.Size = new System.Drawing.Size(1183, 90);
            p.TabIndex = 0;

            PictureBox pic = new PictureBox();
            pic.Image = icon;
            pic.SizeMode = PictureBoxSizeMode.StretchImage;
            pic.Name = "iconPictureBox" + title;
            pic.Location = _1;
            pic.Size = new System.Drawing.Size(80, 80);
            pic.TabIndex = 0;
            pic.TabStop = false;

            Label name = new Label();
            name.Text = title;
            name.Name = "nameLabel" + title;
            name.Location = _2;
            name.AutoSize = true;
            name.Font = new System.Drawing.Font("Roboto Black", 24F, System.Drawing.FontStyle.Bold);
            name.Size = new System.Drawing.Size(193, 38);
            name.TabIndex = 1;

            Label currentStatus = new Label();
            currentStatus.Text = status;
            currentStatus.Name = "statusLabel" + title;
            currentStatus.Location = _3;
            currentStatus.AutoSize = true;
            currentStatus.Font = new System.Drawing.Font("Roboto Light", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            currentStatus.Size = new System.Drawing.Size(81, 29);
            currentStatus.TabIndex = 1;

            Button action = new Button();
            action.Text = buttonText;
            action.Name = "actionLabel" + title;
            action.Location = _4;
            action.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            action.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(189)))), ((int)(((byte)(0)))));
            action.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            action.Size = new System.Drawing.Size(157, 42);
            action.TabIndex = 0;
            action.UseVisualStyleBackColor = true;

            ProgressBar progress = new ProgressBar();
            progress.Visible = false;
            progress.Name = "downloadProgressBar" + title;
            progress.Location = _5;
            progress.Size = new System.Drawing.Size(229, 42);
            progress.TabIndex = 2;

            Label currentTime = new Label();
            currentTime.Text = "Time played: "+ time;
            currentTime.Name = "timeLabel" + title;
            currentTime.Location = _6;
            currentTime.AutoSize = true;
            currentTime.Font = new System.Drawing.Font("Roboto Light", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            currentTime.Size = new System.Drawing.Size(122, 23);
            currentTime.TabIndex = 1;

            p.Controls.Add(pic);
            p.Controls.Add(name);
            p.Controls.Add(currentStatus);
            p.Controls.Add(action);
            p.Controls.Add(progress);
            p.Controls.Add(currentTime);

            layoutPanel.Controls.Add(p);
        }


        public void addGameTab(string icon, string title, string time, string status, string buttonText)
        {
            Point _1 = new Point(3, 5);
            Point _2 = new Point(89, 26);
            Point _3 = new Point(331, 35);
            Point _4 = new Point(487, 28);
            Point _5 = new Point(653, 28);
            Point _6 = new Point(961, 38);

            Panel p = new Panel();

            p.Name = "panel" + title;
            p.Size = new System.Drawing.Size(1183, 90);
            p.TabIndex = 0;

            PictureBox pic = new PictureBox();
            pic.ImageLocation = icon;
            pic.SizeMode = PictureBoxSizeMode.StretchImage;
            pic.Name = "iconPictureBox" + title;
            pic.Location = _1;
            pic.Size = new System.Drawing.Size(80, 80);
            pic.TabIndex = 0;
            pic.TabStop = false;

            Label name = new Label();
            name.Text = title;
            name.Name = "nameLabel" + title;
            name.Location = _2;
            name.AutoSize = true;
            name.Font = new System.Drawing.Font("Roboto Black", 24F, System.Drawing.FontStyle.Bold);
            name.Size = new System.Drawing.Size(193, 38);
            name.TabIndex = 1;

            Label currentStatus = new Label();
            currentStatus.Text = status;
            currentStatus.Name = "statusLabel" + title;
            currentStatus.Location = _3;
            currentStatus.AutoSize = true;
            currentStatus.Font = new System.Drawing.Font("Roboto Light", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            currentStatus.Size = new System.Drawing.Size(81, 29);
            currentStatus.TabIndex = 1;

            Button action = new Button();
            action.Text = buttonText;
            action.Name = "actionLabel" + title;
            action.Location = _4;
            action.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            action.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(132)))), ((int)(((byte)(189)))), ((int)(((byte)(0)))));
            action.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            action.Size = new System.Drawing.Size(157, 42);
            action.TabIndex = 0;
            action.UseVisualStyleBackColor = true;

            ProgressBar progress = new ProgressBar();
            progress.Visible = false;
            progress.Name = "downloadProgressBar" + title;
            progress.Location = _5;
            progress.Size = new System.Drawing.Size(229, 42);
            progress.TabIndex = 2;

            Label currentTime = new Label();
            currentTime.Text = "Time played: " + time;
            currentTime.Name = "timeLabel" + title;
            currentTime.Location = _6;
            currentTime.AutoSize = true;
            currentTime.Font = new System.Drawing.Font("Roboto Light", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            currentTime.Size = new System.Drawing.Size(122, 23);
            currentTime.TabIndex = 1;

            p.Controls.Add(pic);
            p.Controls.Add(name);
            p.Controls.Add(currentStatus);
            p.Controls.Add(action);
            p.Controls.Add(progress);
            p.Controls.Add(currentTime);

            layoutPanel.Controls.Add(p);
        }


        public Panel findTab(string title)
        {
            try
            {
                foreach (Panel p in layoutPanel.Controls.OfType<Panel>())
                {
                    if (p.Name == "panel" + title)
                    {
                        return p;
                    }
                }
            }
            catch { }
            return null;
        }

        public void changeImage(Image img, Panel p)
        {
            try
            {
                foreach (PictureBox picture in p.Controls.OfType<PictureBox>())
                {
                    if (picture.Name.StartsWith("iconPictureBox"))
                    {
                        picture.Image = img;
                    }
                }
            }
            catch { }
        }

        public void setProgressPercentage(int percentage, Panel p)
        {
            try
            {
                foreach (ProgressBar bar in p.Controls.OfType<ProgressBar>())
                {
                    if (bar.Name.StartsWith("downloadProgressBar"))
                    {
                        if (Main.ControlInvokeRequired(bar, () => setProgressPercentage(percentage,p))) return;
                        bar.Value = percentage;
                    }
                }
            }
            catch { }
        }

        public ProgressBar getProgressBar(Panel p)
        {
            try
            {
                foreach (ProgressBar bar in p.Controls.OfType<ProgressBar>())
                {
                    if (bar.Name.StartsWith("downloadProgressBar"))
                    {
                        return bar;
                    }
                }
            }
            catch { }
            return null;
        }

        public void _showProgressBar(Panel p)
        {
            try
            {
                foreach (ProgressBar bar in p.Controls.OfType<ProgressBar>())
                {
                    if (bar.Name.StartsWith("downloadProgressBar"))
                    {
                        if (Main.ControlInvokeRequired(bar, () => _showProgressBar(p))) return;
                        bar.Visible = true;
                    }
                }
            }
            catch { }
        }

        public void _hideProgressBar(Panel p)
        {
            try
            {
                foreach (ProgressBar bar in p.Controls.OfType<ProgressBar>())
                {
                    if (bar.Name.StartsWith("downloadProgressBar"))
                    {
                        if (Main.ControlInvokeRequired(bar, () => _hideProgressBar(p))) return;
                        bar.Visible = false;
                    }
                }
            }
            catch { }
        }

        public void _resetProgressBar(Panel p)
        {
            try
            {
                foreach (ProgressBar bar in p.Controls.OfType<ProgressBar>())
                {
                    if (bar.Name.StartsWith("downloadProgressBar"))
                    {
                        if (Main.ControlInvokeRequired(bar, () => _resetProgressBar(p))) return;
                        bar.Value = 0;
                    }
                }
            }
            catch(SyntaxErrorException e) {
                MessageBox.Show(e.ToString());
            }
        }

        public void changeTime(string time, Panel p)
        {
            try
            {
                foreach (Label l in p.Controls.OfType<Label>())
                {
                    if (l.Name.StartsWith("timeLabel"))
                    {
                        l.Text = "Time played: " + time;
                    }
                }
            }
            catch { }
        }

        public string getTitle(Panel p)
        {
            try
            {
                foreach (Label l in p.Controls.OfType<Label>())
                {
                    if (l.Name.StartsWith("nameLabel"))
                    {
                        return l.Text;
                    }
                }
            }
            catch { }
            return null;
        }

        public Button getButton(Panel p)
        {
            try
            {
                foreach (Button b in p.Controls.OfType<Button>())
                {
                    return b;
                }
            }
            catch { }
            return null;
        }

        public void changeButtonText(string newText, Panel p)
        {
            try
            {
                foreach (Button b in p.Controls.OfType<Button>())
                {
                    if (b.Name.StartsWith("actionLabel"))
                    {
                        if (Main.ControlInvokeRequired(b, () => changeButtonText(newText, p))) return;
                        b.Text = newText;
                    }
                }
            }
            catch { }
        }

        public void changeStatus(string status, Panel p)
        {
            try
            {
                foreach (Label l in p.Controls.OfType<Label>())
                {
                    if (l.Name.StartsWith("statusLabel"))
                    {
                        l.Text = status;
                    }
                }
            }
            catch { }
        }
    }
}
