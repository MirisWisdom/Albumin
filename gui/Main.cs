using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Gunloader.GUI.Annotations;

namespace Gunloader.GUI
{
  public class Main : INotifyPropertyChanged
  {
    private string                      _source = string.Empty;
    private string                      _title  = string.Empty;
    private ObservableCollection<Track> _tracks = new();

    public string Title
    {
      get => _title;
      set
      {
        if (value == _title) return;
        _title = value;
        NotifyPropertyChanged();
      }
    }

    public string Source
    {
      get => _source;
      set
      {
        if (value == _source) return;
        _source = value;
        NotifyPropertyChanged();
      }
    }

    public ObservableCollection<Track> Tracks
    {
      get => _tracks;
      set
      {
        if (Equals(value, _tracks)) return;
        _tracks = value;
        NotifyPropertyChanged();
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}