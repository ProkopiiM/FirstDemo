using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using System;
using System.Collections.Generic;

namespace MP4practic.Views
{
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
            string login = loginUser.Text;
            string password = passwordUser.Text;
            string loginpassword = login + "_" + password;
            CreateCapch();
            DataContext = new DataSourse.AurorizashionSourse();

        }
        
        public void CreateCapch()
        {
            Random random = new Random();
            
            int a = random.Next(1, 3);
                firstSim.FontSize = random.Next(8,25);
            secondSim.FontSize = random.Next(8,25);
            threeSim.FontSize = random.Next(8,25);
            fourSim.FontSize = random.Next(8, 25);
            if(a==1) {
                sim1panel.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            }
            else if (a == 2)
            {
                sim1panel.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
            }
            else if (a == 3)
            {
                sim1panel.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom;
            }
            panel1.Width = random.Next(2, 10);
            panel2.Width = random.Next(2, 10);
            panel3.Width = random.Next(2, 10);
            a = random.Next(1, 3);
            if (a == 1)
            {
                sim2panel.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            }
            else if (a == 3)
            {
                sim2panel.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
            }
            else if (a == 2)
            {
                sim2panel.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom;
            }
            a = random.Next(1, 3);
            if (a == 3)
            {
                sim3panel.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            }
            else if (a == 2)
            {
                sim3panel.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
            }
            else if (a == 1)
            {
                sim3panel.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom;
            }
            CreateText();
        }
        public void CreateText()
        {
            List<char> list = new List<char>();
            for (char c = 'a'; c <= 'z'; ++c)
            {
                list.Add(c);
            }
            for (char c = 'A'; c <= 'Z'; ++c)
            {
                list.Add(c);
            }
            for (char c = '0'; c <= '9'; ++c)
            {
                list.Add(c);
            }
            Random random = new Random();
            firstSim.Text = list[random.Next(62)].ToString();
            secondSim.Text = list[random.Next(62)].ToString();
            threeSim.Text = list[random.Next(62)].ToString();
            fourSim.Text = list[random.Next(62)].ToString();
        }
    }
}
