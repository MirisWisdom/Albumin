using System.ComponentModel;
using System.Runtime.CompilerServices;
using Gunloader.GUI.Annotations;

namespace Gunloader.GUI
{
  public class Track : INotifyPropertyChanged
  {
    private int _number;
    private string _start;
    private string _title;

    public int Number
    {
      get => _number;
      set
      {
        if (value == _number) return;
        _number = value;
        NotifyPropertyChanged();
      }
    }

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

    public string Start
    {
      get => _start;
      set
      {
        if (value == _start) return;
        _start = value;
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