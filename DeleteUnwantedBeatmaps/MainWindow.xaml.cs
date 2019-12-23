using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;

namespace DeleteUnwantedBeatmaps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CommonOpenFileDialog dialog = new CommonOpenFileDialog();
        public MainWindow()
        {
            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.CenterScreen;


        }

        // select a path
        private void Browse(object sender, RoutedEventArgs e)
        {
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();
            Path.Text = dialog.FileName;
        }

        // if mania is checked, displays all mania specific settings
        private void Mania_Checked(object sender, RoutedEventArgs e)
        {
            ManiaKeysLabel.Visibility = Visibility.Visible;
            ManiaKeysGrid1.Visibility = Visibility.Visible;
            ManiaKeysGrid2.Visibility = Visibility.Visible;
            SelectAllBtn.Visibility = Visibility.Visible;
            SelectNoneBtn.Visibility = Visibility.Visible;
        }

        // if mania is not checked, hides all mania specific settings
        private void Mania_Unchecked(object sender, RoutedEventArgs e)
        {
            ManiaKeysLabel.Visibility = Visibility.Hidden;
            ManiaKeysGrid1.Visibility = Visibility.Hidden;
            ManiaKeysGrid2.Visibility = Visibility.Hidden;
            SelectAllBtn.Visibility = Visibility.Hidden;
            SelectNoneBtn.Visibility = Visibility.Hidden;
        }

        // selects all keymodes
        private void SelectAll(object sender, RoutedEventArgs e)
        {
            Keys1.IsChecked = true;
            Keys2.IsChecked = true;
            Keys3.IsChecked = true;
            Keys4.IsChecked = true;
            Keys5.IsChecked = true;
            Keys6.IsChecked = true;
            Keys7.IsChecked = true;
            Keys8.IsChecked = true;
            Keys9.IsChecked = true;
            Keys10.IsChecked = true;
            Keys11.IsChecked = true;
            Keys12.IsChecked = true;
            Keys13.IsChecked = true;
            Keys14.IsChecked = true;
            Keys15.IsChecked = true;
            Keys16.IsChecked = true;
            Keys17.IsChecked = true;
            Keys18.IsChecked = true;
        }

        // deselects all keymodes
        private void SelectNone(object sender, RoutedEventArgs e)
        {
            Keys1.IsChecked = false;
            Keys2.IsChecked = false;
            Keys3.IsChecked = false;
            Keys4.IsChecked = false;
            Keys5.IsChecked = false;
            Keys6.IsChecked = false;
            Keys7.IsChecked = false;
            Keys8.IsChecked = false;
            Keys9.IsChecked = false;
            Keys10.IsChecked = false;
            Keys11.IsChecked = false;
            Keys12.IsChecked = false;
            Keys13.IsChecked = false;
            Keys14.IsChecked = false;
            Keys15.IsChecked = false;
            Keys16.IsChecked = false;
            Keys17.IsChecked = false;
            Keys18.IsChecked = false;
        }

        // main process
        private void Start(object sender, RoutedEventArgs e)
        {
            // get user data //

            string path;

            // if no path has been selected, gives a warning to the user and returns
            try
            {
                path = dialog.FileName;
            }
            catch
            {
                System.Windows.MessageBox.Show("Please give a Path!");
                return;
            }

            // lists the numbers representing the game modes that have been selected
            List<int> gameModes = new List<int>();
            if (Standard.IsChecked ?? false) gameModes.Add(0);
            if (Taiko.IsChecked ?? false) gameModes.Add(1);
            if (CatchTheBeat.IsChecked ?? false) gameModes.Add(2);
            if (Mania.IsChecked ?? false) gameModes.Add(3);

            // if no game mode has been selected, gives a warning to the user and returns
            if (gameModes.Count < 1)
            {
                System.Windows.MessageBox.Show("Please check at least 1 Game Mode!");
                return;
            }

            // lists the numbers representing the key modes that have been selected
            List<int> maniaKeys = new List<int>();
            if (gameModes.Contains(3))
            {
                if (Keys1.IsChecked ?? false) maniaKeys.Add(1);
                if (Keys2.IsChecked ?? false) maniaKeys.Add(2);
                if (Keys3.IsChecked ?? false) maniaKeys.Add(3);
                if (Keys4.IsChecked ?? false) maniaKeys.Add(4);
                if (Keys5.IsChecked ?? false) maniaKeys.Add(5);
                if (Keys6.IsChecked ?? false) maniaKeys.Add(6);
                if (Keys7.IsChecked ?? false) maniaKeys.Add(7);
                if (Keys8.IsChecked ?? false) maniaKeys.Add(8);
                if (Keys9.IsChecked ?? false) maniaKeys.Add(9);
                if (Keys10.IsChecked ?? false) maniaKeys.Add(10);
                if (Keys11.IsChecked ?? false) maniaKeys.Add(11);
                if (Keys12.IsChecked ?? false) maniaKeys.Add(12);
                if (Keys13.IsChecked ?? false) maniaKeys.Add(13);
                if (Keys14.IsChecked ?? false) maniaKeys.Add(14);
                if (Keys15.IsChecked ?? false) maniaKeys.Add(15);
                if (Keys16.IsChecked ?? false) maniaKeys.Add(16);
                if (Keys17.IsChecked ?? false) maniaKeys.Add(17);
                if (Keys18.IsChecked ?? false) maniaKeys.Add(18);

                // if mania is checked and no key mode has been selected, gives a warning to the user and returns
                if (maniaKeys.Count < 1)
                {
                    System.Windows.MessageBox.Show("Please check at least 1 Mania Key mode!");
                    return;
                }
            }
            

            // confirmation window
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("The program will delete all folders that are empty, unnecesary or a duplicate and all beatmaps that do not match the info you provided.\n Are you sure you want to continue?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.No)
            {
                return;
            }

            // open the processWindow
            ProgressWindow progressWindow = new ProgressWindow();
            progressWindow.Show();


            // if a min/max length has been given, put it it in "minimumLength"/"maximumLength"
            int minimumLength;
            int maximumLength;
            if (int.TryParse(MinimumLength.Text, out int resultInt) && MinimumLength.Text != "")
            {
                minimumLength = Convert.ToInt32(MinimumLength.Text);
            }
            else
            {
                minimumLength = -1;
            }
            if (int.TryParse(MaximumLength.Text, out resultInt) && MaximumLength.Text != "")
            {
                maximumLength = Convert.ToInt32(MaximumLength.Text);
            }
            else
            {
                maximumLength = int.MaxValue;
            }

            // if a min/max difficulty has been given, put it it in "minimumStar"/"maximumStar"
            double minimumStar;
            double maximumStar;
            if (double.TryParse(MinimumStar.Text, out double resultDouble) && MinimumStar.Text != "")
            {
                minimumStar = Convert.ToDouble(MinimumStar.Text);
            }
            else
            {
                minimumStar = -1;
            }
            if (double.TryParse(MaximumStar.Text, out resultDouble) && MaximumStar.Text != "")
            {
                maximumStar = Convert.ToDouble(MaximumStar.Text);
            }
            else
            {
                maximumStar = double.MaxValue;
            }


            

            






            // start process //
            List<string> thingsToDel = new List<string>();


            // delete directories that do not contain numbers at the beginning or are shorter than 7 characters //
            int indexesToCheck = 7;
            string[] allDirs = Directory.GetDirectories(path);
            foreach (string dir in allDirs)
            {
                try
                {
                    dir.Substring(path.Length + 1, indexesToCheck);
                    Convert.ToInt16(dir.Substring(path.Length + 1, 4));
                }
                catch
                {
                    thingsToDel.Add(dir);
                }
            }
            foreach (string thing in thingsToDel)
            {
                Directory.Delete(thing, true);
            }
            thingsToDel.Clear();






            // delete duplicate directories //
            allDirs = Directory.GetDirectories(path);
            // the amount of characters (counting from the beginning of the dir name) that have to match before it's counted as a duplicate

            for (int i = 0; i < allDirs.Length; i++)
            {
                // makes a substring with 'indexesToCheck' amount of characters from the directory name
                string dirStart = allDirs[i].Substring(path.Length + 1, indexesToCheck);
                // the +1 is because the path allDirs[i] has an additional '/'

                // goes trough every dir above i and compares it to 'dirStart'
                for (int j = i + 1; j < allDirs.Length; j++)
                {
                    // makes another substring
                    string otherDirStart = allDirs[j].Substring(path.Length + 1, indexesToCheck);
                    // if they match, the dir of index i will be deleted later on
                    if (dirStart == otherDirStart)
                    {
                        thingsToDel.Add(allDirs[i]);
                        break;
                    }
                }
            }



            // deletes everything in 'thingsToDel'
            foreach (string thing in thingsToDel)
            {
                Directory.Delete(thing, true);
                // the true stands for deleting directories, even if they have subdirectories/files
            }
            thingsToDel.Clear();
            // displays the progress to the progresswindow
            progressWindow.DupDelMsg.Visibility = Visibility.Visible;




            



            // delete files that do not match the needs of the user needs //
            string[] allFiles = Directory.GetFiles(path, "*.osu", SearchOption.AllDirectories);
            // mode == game mode
            int mode = -1;
            // .osu files are comparable to .ini files

            bool deleteFile;
            int length = 0;

            // checks for each file, if it should be deleted
            foreach (string file in allFiles)
            {
                string text = File.ReadAllText(file);
                if (!text.Contains("Mode") || !text.Contains("CircleSize") || !text.Contains("[HitObjects]"))
                {
                    thingsToDel.Add(file);
                    continue;
                }

                deleteFile = false;

                string[] lines = File.ReadAllLines(file);

                // checks every line for certain keywords
                foreach (string line in lines)
                {
                    if (line.Contains("Mode") && line.LastIndexOf("Mode") <= 0)
                    {
                        mode = Convert.ToInt16(line.Substring(line.Length - 1, 1));
                        // checks if the game mode is contained in "gameModes"
                        if (!gameModes.Contains(mode))
                        {
                            // otherwise deletes it later
                            deleteFile = true;
                            break;
                        }
                    }
                    else if (line.Contains("CircleSize") && line.LastIndexOf("CircleSize") <= 0 && mode == 3)
                    {
                        int key = Convert.ToInt16(line.Substring(line.Length - 1, 1));
                        // checks if the key mode is contained in "maniaKeys"
                        if (!maniaKeys.Contains(key))
                        {
                            // otherwise deletes it later
                            deleteFile = true;
                            break;
                        }
                    }
                }

                // gets the length of the map
                try
                {
                    
                    text = text.Substring(text.IndexOf("[HitObjects]") + 12).Trim();
                    string[] rows = text.Split('\n');
                    string[] firstRow = rows[0].Split(',');
                    string[] lastRow = rows[rows.Length - 1].Split(',');
                    length = Convert.ToInt32(lastRow[2]) - Convert.ToInt32(firstRow[2]);
                    length = length / 1000;
                }
                catch
                {
                    deleteFile = true;
                }
                

                // if the length doesn't meet the requirements, it will be deleted later
                if (length < minimumLength || length > maximumLength)
                {
                    deleteFile = true;
                }


                if (deleteFile)
                {
                    thingsToDel.Add(file);
                }
            }

            foreach (string thing in thingsToDel)
            {
                File.Delete(thing);
            }
            thingsToDel.Clear();
            progressWindow.FileDelMsg.Visibility = Visibility.Visible;



            




            // delete directories without .osu files //
            allDirs = Directory.GetDirectories(path);

            foreach (string dir in allDirs)
            {
                // if a dir does not have at least 1 .osu file in it, it will be delete later
                if (Directory.GetFiles(dir, "*.osu").Length == 0)
                {
                    thingsToDel.Add(dir);
                }
            }

            foreach (string thing in thingsToDel)
            {
                Directory.Delete(thing, true);
            }
            thingsToDel.Clear();
            progressWindow.EmpFolDelMsg.Visibility = Visibility.Visible;

            progressWindow.DoneOwO.Visibility = Visibility.Visible;
            progressWindow.Close();
        }
    }
}
