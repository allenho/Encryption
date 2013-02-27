using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//begin added references
using System.Windows.Forms; // FolderBrowserDialog, DialogResult
using System.IO; // Directory
using System.Collections; //ArrayList

namespace Encryption
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String currentDirectory;
        public MainWindow()
        {
            InitializeComponent();
        }

        /*
         * Simple Input File Folder Browsing
         * Allows user to select a directory from which to choose their file(s).
         * This is in preparation for the multiple files version..
         * Although I probably should have allowed for individual file selections first probably...
         */
        private void search_Click(object sender, RoutedEventArgs e)
        {
            //Creates a folder browsing object
            FolderBrowserDialog fb = new System.Windows.Forms.FolderBrowserDialog();
            DialogResult result;
            String folderName = "";

            // Reloads the most recently accessed folder. need to write this to the app.config. ON TODO LIST
            if (currentDirectory != "")
            {
                fb.SelectedPath = currentDirectory;
            }

            /* Loop brings up the folder browsing dialog and forces user to pick a folder.
             * If none is selected, it will re-prompt until they pick one*/
            while (folderName.Equals(""))
            {
                // ShowDialog brings up brings up the browsing window
                result = fb.ShowDialog();
                //get selected path
                folderName = fb.SelectedPath;
            }

            //Get the filePaths from the folder
            String[] filePaths = Directory.GetFiles(@folderName); //note: @ is used in front of folderName so that we do not need to escape the '\' symbols.
            currentDirectory = fb.SelectedPath;

            //Array to hold our filenames
            ArrayList fileNames = new ArrayList();
            //Get just the filenames
            foreach (String s in filePaths)
            {
                String[] split = s.Split('\\');
                fileNames.Add(split[split.Length - 1]);
            }

            //Clean out the filenames box.
            fileNameBox.Items.Clear();
            //Add the files to the combobox
            foreach (String s in fileNames)
            {
                fileNameBox.Items.Add(s);
            }
        }
        
        /*
         * Encryption Button Click.
         * Takes the user provided input (if not given, will alert user)
         * Generates an output name if necessary
         * passes information to Encrypter to perform the Encryption
         */
        private void encrypt_Click(object sender, RoutedEventArgs e)
        {
            //Generates the input FilePath
            String inputFileName = currentDirectory + "\\" + fileNameBox.SelectedValue;
            //Generates the output FilePath
            String outputFileName = currentDirectory + "\\" +  outputBox.Text;
            
            //If no output name is provided, we will generate one for the user (should we prompt?)
            if (outputBox.Text == "")
            {
                outputFileName += generateGenericName(fileNameBox.SelectedValue.ToString());
            }
            
            //Load Encrypter and pass information to begin encryption
            Encrypter en = new Encrypter();
            en.EncryptFile(@inputFileName, keyBox.Password.ToString(), @outputFileName);
        }

        /*
         * Method to Generate a generic output name in case one is not given
         */
        private String generateGenericName(String name)
        {
            //splits on the extension ( multiple . case handled )
            String[] split = name.Split('.');

            StringBuilder fileName = new StringBuilder();
            //For the multiple . case, will append all but the "extension"
            for (int i = 0; i < split.Length - 1; i++)
            {
                fileName.Append(split[i]);
                fileName.Append(".");
            }
            return fileName + "e";
        }

        private void testEncrypt_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
