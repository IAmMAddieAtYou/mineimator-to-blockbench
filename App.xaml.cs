using System.Windows;
using System;

namespace Mineamator_to_Blockbench_Converter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static MainWindow mainWindow; // Place this var out of the constructor
        public static App app = new App();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main() // Note: The 'public' keyword was missing in your previous correct version
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

             // Create an instance of the App class
            app.Run(mainWindow = new MainWindow());   // Run the application with the main window
        }
    }
}