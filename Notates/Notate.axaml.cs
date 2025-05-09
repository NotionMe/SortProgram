using System;
using Avalonia.Controls;
using Avalonia.Interactivity; // Required for RoutedEventArgs
using Avalonia.Markup.Xaml;
using Practika2_OPAM_Ubohyi_Stanislav.Services;
using Practika2_OPAM_Ubohyi_Stanislav.Auth;

namespace Practika2_OPAM_Ubohyi_Stanislav.Notates;

public partial class Notate : Window
{
    private readonly IAuthService? _authService;

    public Notate()
    {
        InitializeComponent();
        _authService = null;
    }

    public Notate(IAuthService authService)
    {
        InitializeComponent();
        _authService = authService;
        LoadNoteForSelectedSort();
    }

    private void SortComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        LoadNoteForSelectedSort();
    }

    public void LoadNoteForSelectedSort()
    {
        if (NoteTextBox == null || SortComboBox == null || _authService == null) return;

        User? currentUser = _authService.GetCurrentUser();
        if (currentUser == null || currentUser.Notes == null)
        {
            NoteTextBox.Text = string.Empty;
            return;
        }

        int selectedIndex = SortComboBox.SelectedIndex;
        string noteKey = string.Empty;

        switch (selectedIndex)
        {
            case 0: // Bubble Sort
                noteKey = "NotesBubbleSort";
                break;
            case 1: // Selection Sort
                noteKey = "NotesSelectionSort";
                break;
            case 2: // Quick Sort
                noteKey = "NotesQuickSort";
                break;
            case 3: // Insertion Sort
                noteKey = "NoteInsertionSort";
                break;
            case 4: // Merge Sort
                noteKey = "NoteMergeSort";
                break;
            case 5: // Heap Sort
                noteKey = "NoteHeapSort";
                break;
            case 6: // Radix Sort
                noteKey = "NoteRadixSort";
                break;
            case 7: // Linear Sorting
                noteKey = "NoteLinearSorting";
                break;
            case 8: // Binary Search
                noteKey = "NoteBinarySearch";
                break;
            case 9: // Jump Search
                noteKey = "NoteJumpSearch";
                break;
            default:
                NoteTextBox.Text = string.Empty;
                return;
        }

        if (currentUser.Notes.TryGetValue(noteKey, out string? noteValue))
        {
            NoteTextBox.Text = noteValue;
        }
        else
        {
            NoteTextBox.Text = string.Empty;
        }
    }

    private void SaveButton_Click(object? sender, RoutedEventArgs e)
    {
        if (_authService == null)
        {
            Console.WriteLine("AuthService not available.");
            return;
        }

        string noteContent = NoteTextBox.Text ?? string.Empty; 
        SaveNoteToFile(noteContent);
    }

    private void SaveNoteToFile(string noteContent) // noteContent is now non-nullable
    {
        if (SortComboBox == null || _authService == null) return;

        User? currentUser = _authService.GetCurrentUser(); 
        if (currentUser == null)
        {
            // Handle case where there is no current user
            Console.WriteLine("No current user to save note for.");
            return;
        }

        if (currentUser.Notes == null)
        {
            currentUser.Notes = new System.Collections.Generic.Dictionary<string, string>();
        }

        int selectedIndex = SortComboBox.SelectedIndex;
        string noteKey = string.Empty;

        switch (selectedIndex)
        {
            case 0: // Bubble Sort
                noteKey = "NotesBubbleSort";
                break;
            case 1: // Selection Sort
                noteKey = "NotesSelectionSort";
                break;
            case 2: // Quick Sort
                noteKey = "NotesQuickSort";
                break;
            case 3: // Insertion Sort
                noteKey = "NoteInsertionSort";
                break;
            case 4: // Merge Sort
                noteKey = "NoteMergeSort";
                break;
            case 5: // Heap Sort
                noteKey = "NoteHeapSort";
                break;
            case 6: // Radix Sort
                noteKey = "NoteRadixSort";
                break;
            case 7: // Linear Sorting
                noteKey = "NoteLinearSorting";
                break;
            case 8: // Binary Search
                noteKey = "NoteBinarySearch";
                break;
            case 9: // Jump Search
                noteKey = "NoteJumpSearch";
                break;
            default:
                return;
        }



        if (!string.IsNullOrEmpty(noteKey))
        {
            currentUser.Notes[noteKey] = noteContent;
            _authService.UpdateCurrentUser(currentUser);
        }
    }
}