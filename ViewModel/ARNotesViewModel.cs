using Apuntes.Models;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace Apuntes.ViewModel;

internal class ARNotesViewModel : IQueryAttributable
{
    public ObservableCollection<ViewModel.ARNoteViewModel> AllNotes { get; }
    public ICommand NewCommand { get; }
    public ICommand SelectNoteCommand { get; }

    public ARNotesViewModel()
    {
        AllNotes = new ObservableCollection<ViewModel.ARNoteViewModel>(Models.ARNote.LoadAll().Select(n => new ARNoteViewModel(n)));
        NewCommand = new AsyncRelayCommand(NewNoteAsync);
        SelectNoteCommand = new AsyncRelayCommand<ARNoteViewModel?>(SelectNoteAsync);

    }

    private async Task NewNoteAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.ARNotePage));
    }

    private async Task SelectNoteAsync(ViewModel.ARNoteViewModel note)
    {
        if (note != null)
            await Shell.Current.GoToAsync($"{nameof(Views.ARNotePage)}?Cargando={note.Identifier}");
    }

    void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("Borrado"))
        {
            string noteId = query["Borrado"].ToString();
            ARNoteViewModel matchedNote = AllNotes.FirstOrDefault((n) => n.Identifier == noteId);

            // If note exists, delete it
            if (matchedNote != null)
                AllNotes.Remove(matchedNote);
        }
        else if (query.ContainsKey("Guardado"))
        {
            string noteId = query["Guardado"].ToString();
            if (!string.IsNullOrEmpty(noteId)) // Verificar si noteId no es nulo ni vacío
            {
                ARNoteViewModel matchedNote = AllNotes.Where((n) => n.Identifier == noteId).FirstOrDefault();

                if (matchedNote != null)
                    AllNotes.Move(AllNotes.IndexOf(matchedNote), 0);
                else
                    AllNotes.Insert(0, new ARNoteViewModel(Models.ARNote.Load(noteId)));
            }
            else
            {
                // Manejar el caso en que noteId es nulo o vacío (opcional)
                Debug.WriteLine("El valor de noteId es nulo o vacío.");
            }
        }

    }
}