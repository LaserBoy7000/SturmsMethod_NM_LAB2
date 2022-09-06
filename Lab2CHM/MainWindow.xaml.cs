using Lab2CHM.Calc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab2CHM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Program program;
        public MainWindow()
        {
            InitializeComponent();
            MouseDown += Drag;
            program = new Program();
        }

        void Err(string err)
        { 
            output.Text = $"Internal error: {err}"; 
        }

        void Drag(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        void Exit(object sender, RoutedEventArgs e) => Close();

        void Solve(object sender, RoutedEventArgs e)
        {
            try
            {
                output.Text = program.Main(input.Text, gap.Text);
            }
            catch (Exception ex) { Err(ex.Message); }
        }

        void Echo(object sender, RoutedEventArgs e)
        {
            try
            {
                output.Text = program.Echo(input.Text);
            }
            catch (Exception ex) { Err(ex.Message); }
        }
    }
}
