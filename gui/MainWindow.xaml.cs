using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using static System.Diagnostics.ProcessWindowStyle;
using static System.Environment;
using static System.IO.Path;
using static System.Text.RegularExpressions.Regex;

namespace Gunloader.GUI
{
  /// <summary>
  ///   Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private readonly Main _main;

    public MainWindow()
    {
      InitializeComponent();
      _main        =  (Main)DataContext;
      Console.Text += "Welcome!\n";
    }

    private void Add(object sender, RoutedEventArgs e)
    {
      /**
       * TODO: Have the validation occur in the Main.cs class, decoupled away from this method!
       */

      /**
       * Prevent empty values...
       */

      foreach (var (textBox, value) in new Dictionary<TextBox, string>
      {
        { Title, "title" },
        { Start, "starting time in the video" }
      })
      {
        if (!string.IsNullOrWhiteSpace(textBox.Text))
          continue;

        MessageBox.Show($"Please specify the track's {value}!");
        return;
      }

      /**
       * Prevent non-time values for Start property...
       */

      if (!Match(Start.Text, @"^(2[0-3]|[01]?[0-9]):([0-5]?[0-9]):([0-5]?[0-9])$").Success)
      {
        MessageBox.Show("Make sure the start time is in HH:MM:SS format! For example: 00:02:30");
        return;
      }

      _main.Tracks.Add(new Track
      {
        Number = _main.Tracks.Count + 1,
        Title  = Title.Text,
        Start  = Start.Text
      });
    }

    private void Process(object sender, RoutedEventArgs e)
    {
      /**
       * TODO: Unify the validation into Main.cs away from this method.
       */

      if (string.IsNullOrWhiteSpace(Album.Text))
      {
        MessageBox.Show("Please specify the album title!");
        return;
      }

      if (string.IsNullOrWhiteSpace(Video.Text))
      {
        MessageBox.Show("Please specify the video source!");
        return;
      }

      if (_main.Tracks.Count == 0)
      {
        MessageBox.Show("Please specify at least one track!");
        return;
      }

      using var records = new StreamWriter(Combine("records.txt"));

      records.WriteLine(_main.Title);
      records.WriteLine(_main.Source);
      records.WriteLine(string.Empty);

      foreach (var track in _main.Tracks)
      {
        var record = $"{track.Number} {track.Start} {track.Title}";
        records.WriteLine(record);
        Console.Text += $"Wrote record to file: {record}\n";
      }

      Console.Text += "Finished writing the records!\n";

      /**
       * TODO: Avoid this magic and use proper objects and bindings. This is absolutely NASTY!
       */
      var format = new[]
      {
        "flac",
        "lame",
        "opus",
        "vorbis",
        "raw"
      }[Format.SelectedIndex];

      Invoke("gunloader.exe", $"--no-prompt --format {format} --records records.txt", CurrentDirectory);
    }

    public void Invoke(string fileName, string arguments, string workingDirectory)
    {
      var process = new Process
      {
        StartInfo =
        {
          FileName               = fileName,
          Arguments              = arguments,
          UseShellExecute        = false,
          CreateNoWindow         = true,
          WindowStyle            = Hidden,
          RedirectStandardOutput = true,
          RedirectStandardError  = true,
          WorkingDirectory       = workingDirectory
        }
      };

      process.OutputDataReceived += (_, e) => Dispatcher.Invoke(() =>
      {
        Console.Text += $"{e.Data}\n";
        Console.ScrollToEnd();
      });

      process.Start();
      process.BeginOutputReadLine();
    }

    private void Browse(object sender, RoutedEventArgs e)
    {
      var openFileDialog = new OpenFileDialog();
      if (openFileDialog.ShowDialog() == true)
        Video.Text = openFileDialog.FileName;
    }

    private void About(object sender, RoutedEventArgs e)
    {
      /**
       * TODO: A much more pleasant about screen.
       */

      MessageBox.Show("Ultra rudimentary UI for Gunloader.\nVersion: 0.0.0.0.0.0.8 pre-alpha-alpha-alpha");
    }

    private void Update(object sender, RoutedEventArgs e)
    {
      /**
       * TODO: Notify when an update is available.
       */
      
      System.Diagnostics.Process.Start("https://github.com/yumiris/gunloader/releases/latest");
    }
  }
}