/*
 *  Filename            : MainWindow.xaml.cs
 *  Name                : Jerry Goe
 *  Date                : 2020/09/28
 *  Description         : This file is the back end of the MainWindow.xaml file. Each part of the design roughly or entirely depends on this cs file. 
 *                        Functionalities such as closing the window and making sure any unsaved changes are saved before closing. The About modal box
 *                        and how it displays and operates when there is another window behind it. As well as other features you'd normally find in a 
 *                        text editor application like Notepad.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace A_02 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        Boolean isSaveNeeded = false;

        public MainWindow() {

            InitializeComponent();
        
        }

        
        private void funcAboutDialog(object sender, RoutedEventArgs e) {

            About abtBox = new About();
            abtBox.Owner = this; //Use in tandem to set the about box to open in the middle of the application window
            abtBox.ShowDialog(); //This will ensure that the user cannot press any window related to the application while the about box is open 

        }

        private void funcSaveFile(object sender, RoutedEventArgs e) {
             
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "";
            sfd.Filter = "Text file (*.txt)|*.txt";
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); //Set the default path that the dialog opens to as the Desktop
            
            if(sfd.ShowDialog() == true) {
                string fileName = sfd.FileName;
                System.IO.File.WriteAllText(fileName, txtEditor.Text);
                
            }
            
        }

        private void funcOpenFile(object sender, RoutedEventArgs e) {

            //If the text was changed then we prompt user if they would like to save their work first before continuing
            if (isSaveNeeded == true) {
                
                string msgPrompt = "You work has been changed, would you like to save before opening a file?";
                string msgBoxTitle = "Warning";
                MessageBoxResult msgResult = System.Windows.MessageBox.Show(msgPrompt, msgBoxTitle, MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                switch (msgResult) {

                    case MessageBoxResult.Yes:
                        funcSaveFile(sender, e);
                        break;
                    case MessageBoxResult.No:
                        break;
                    case MessageBoxResult.Cancel:
                        break;
                
                }

                OpenFileDialog ofd = new OpenFileDialog();
                ofd.FileName = "";
                ofd.Filter = "Text file (*.txt)|*.txt";
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                if (ofd.ShowDialog() == true) {

                    string fileName = ofd.FileName;
                    txtEditor.Text = System.IO.File.ReadAllText(fileName);
                
                }

                isSaveNeeded = false;

            }
            else {

                OpenFileDialog ofd = new OpenFileDialog();
                ofd.FileName = "";
                ofd.Filter = "Text file (*.txt)|*.txt";
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                if (ofd.ShowDialog() == true) {

                    string fileName = ofd.FileName;
                    txtEditor.Text = System.IO.File.ReadAllText(fileName);

                }

                isSaveNeeded = false;
            }
            
        }

        private void funcNewFile(object sender, RoutedEventArgs e) {
            
            //If the text was changed then we prompt user if they would like to save their work first before continuing
            if (isSaveNeeded == true) {

                
                string msgPrompt = "You work has been changed, would you like to save before creating a new file?";
                string msgBoxTitle = "Warning";
                MessageBoxResult msgResult = System.Windows.MessageBox.Show(msgPrompt, msgBoxTitle, MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                switch (msgResult) {
                    case MessageBoxResult.Yes:
                        funcSaveFile(sender, e);
                        txtEditor.Text = String.Empty;
                        break;
                    case MessageBoxResult.No:
                        txtEditor.Text = String.Empty;
                        break;
                    case MessageBoxResult.Cancel:
                        break;
                }
                
            }
            //If the text wasn't changed then just clear the text editor pane
            else {
                txtEditor.Text = String.Empty;
            }

            isSaveNeeded = false;
            
        }

        private void funcTextChanged(object sender, TextChangedEventArgs e) {
            
            isSaveNeeded = true; //Text has been changed, file needs to be saved before opening, creating or closing the file
            characterCount(); //Do a character count while the user is typing into the text editor

        }

        private void funcCloseProgram(object sender, RoutedEventArgs e) {
            
            this.Close(); //Run Window_Closing() function

        }

        //General function for when the user wants to close the application
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            e.Cancel = true;

            if (e.Cancel == true) {

                //If the text is changed then we can do the saving here
                string msgPrompt = "You work has been changed, would you like to save before closing the program?";
                string msgBoxTitle = "Warning";
                MessageBoxResult msgResult = System.Windows.MessageBox.Show(msgPrompt, msgBoxTitle, MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                switch (msgResult) {

                    case MessageBoxResult.Yes:

                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.FileName = "";
                        sfd.Filter = "Text file (*.txt)|*.txt";
                        sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); //

                        if (sfd.ShowDialog() == true) {

                            string fileName = sfd.FileName;
                            System.IO.File.WriteAllText(fileName, txtEditor.Text);
                            //Change the cancel attribute to false, meaning we can exit the application correctly.
                            e.Cancel = false;

                        }

                        break;

                    case MessageBoxResult.No:

                        e.Cancel = false;
                        break;

                    case MessageBoxResult.Cancel:

                        e.Cancel = false;
                        break;

                }
                
            }
            else {

                this.Close();

            }

        }

        private void characterCount() {

            int strCharCount = txtEditor.Text.Length;

            if (strCharCount == 0) {
                txtCharacterCount.Text = strCharCount + " characters";
            }
            else {
                txtCharacterCount.Text = strCharCount + " characters";
            }

        }

        //This Handler is for the X button in the top right corner of the application
        private void CloseWindowHandler(object sender, ExecutedRoutedEventArgs e) {

            this.Close(); //Run the Window_Closing() function

        }
    }

}
